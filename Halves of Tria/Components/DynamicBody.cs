using Halves_of_Tria.Configuration;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Halves_of_Tria.Components
{
    // [TODO: remove some of the logic (i.e. methods) from here; maybe split this up into separate components?]
    internal class DynamicBody
    {
        #region Properties
        private float _mass;
        public float Mass
        {
            get => _mass;
            set
            {
                if (value <= 0)
                    throw new ArgumentOutOfRangeException("Mass must be greater than zero.", nameof(value));
                _mass = value;
                UpdateMassRelatedProperties(value);
            }
        }
        /// <summary>
        /// Represents the inverse of the object's mass; i.e. 1/<see cref="Mass"/>.
        /// </summary>
        /// <remarks>It's only updated when <see cref="Mass"/> is, 
        /// so using it means less divisions are computed, usually.</remarks>
        public float InverseMass { get; private set; }
        public Vector2 Velocity { get; set; }
        public Vector2 Acceleration { get; set; }
        public Vector2 ResultantForce { get; set; }
        public Vector2 UnspentImpulse { get; set; }

        public List<Force> Forces { get; private set; }
        #endregion

        public DynamicBody(float mass)
        {
            Forces = new();

            Force gravitationalForce = new(ForceType.Gravitational, mass, Config.GravitationalAcceleration);
            Forces.Add(gravitationalForce);
            Force linearDrag = new(ForceType.LinearDrag, -Config.DefaultLinearDragCoefficient * Velocity);
            Forces.Add(linearDrag);
            Force normal = new(ForceType.Normal, Vector2.Zero);
            Forces.Add(normal);

            Mass = mass; // mass updates the gravitational force when changed, so this needs to be assigned after the gravitational force is added
            Velocity = Vector2.Zero;
            Acceleration = Vector2.Zero;
            UnspentImpulse = Vector2.Zero;
        }

        #region Helper Methods
        private void UpdateMassRelatedProperties(float newMass)
        {
            InverseMass = 1f / newMass;

            int gravitationalForceIndex = Forces.FindIndex(x => x.Type == ForceType.Gravitational);
            if (gravitationalForceIndex >= 0) // if Forces contains gravitational force
            {
                Force newGravitationalForce = new(ForceType.Gravitational, newMass, Config.GravitationalAcceleration);
                Forces[gravitationalForceIndex] = newGravitationalForce;
            }
        }
        #endregion
    }
}
