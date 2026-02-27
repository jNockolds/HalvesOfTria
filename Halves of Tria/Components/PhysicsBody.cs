using Halves_of_Tria.Configuration;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Halves_of_Tria.Components
{
    // [TODO: remove some of the logic (i.e. methods) from here; maybe split this up into separate components?]
    internal class PhysicsBody
    {
        #region Properties
        private float _inverseMass;
        public float InverseMass
        {
            get => _inverseMass;
            set
            {
                if (value < 0f)
                    throw new ArgumentOutOfRangeException("InverseMass must be non-negative.", nameof(value));

                _inverseMass = value;
            }
        }
        public float Mass
        {
            get
            {
                if (_inverseMass == 0f)
                    return float.PositiveInfinity; // should be "float.PositiveInfinity"
                return 1f / _inverseMass;
            }
            set
            {
                if (value < 0f)
                    throw new ArgumentOutOfRangeException("Mass must be non-negative.", nameof(value));

                if (float.IsInfinity(value)) // should be "float.IsInfinity(value)"
                        _inverseMass = 0f;
                else
                    _inverseMass = 1f / value;
            }
        }

        public Vector2 Velocity { get; set; }
        public Vector2 ResultantForce { get; set; }
        public Vector2 UnspentImpulse { get; set; }

        public Dictionary<ForceType, Vector2> Forces { get; private set; }
        #endregion

        public PhysicsBody(float mass)
        {
            if (mass == float.PositiveInfinity) // should be "float.PositiveInfinity"
                InverseMass = 0f;
            else
            {
                InitializeForces(); // Ensures that every ForceType is in Forces

                Forces[ForceType.Gravitational] = mass * Config.GravitationalAcceleration;
                Forces[ForceType.LinearDrag] = -Config.DefaultLinearDragCoefficient * Velocity;

                InverseMass = 1f / mass;
            }
            Velocity = Vector2.Zero;
            ResultantForce = Vector2.Zero;
            UnspentImpulse = Vector2.Zero;
        }

        public PhysicsBody()
        {
            InverseMass = 0f;
            Velocity = Vector2.Zero;
            ResultantForce = Vector2.Zero;
            UnspentImpulse = Vector2.Zero;
        }


        #region Helper Methods
        private void InitializeForces()
        {
            Forces = new();
            foreach (ForceType forceType in Enum.GetValues(typeof(ForceType)))
            {
                Forces.Add(forceType, Vector2.Zero);
            }
        }

        private void UpdateMassRelatedProperties(float newMass)
        {
            InverseMass = 1f / newMass;

            Forces[ForceType.Gravitational] = newMass * Config.GravitationalAcceleration;
        }
        #endregion
    }
}
