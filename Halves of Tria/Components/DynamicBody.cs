using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using Halves_of_Tria.Configuration;
using System.Diagnostics;

namespace Halves_of_Tria.Components
{
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
        public Vector2 ResultantForce => Mass * Acceleration;
        public Vector2 ResultantImpulse { get; private set; }

        public List<Force> NonVelocityDependentForces { get; private set; }
        public List<Force> VelocityDependentForces { get; private set; }
        #endregion

        public DynamicBody(float mass)
        {
            NonVelocityDependentForces = new();
            VelocityDependentForces = new();

            Force gravitationalForce = new(ForceType.Gravitational, mass, Config.GravitationalAcceleration);
            AddForce(gravitationalForce);
            Force linearDrag = new(ForceType.LinearDrag, -Config.DefaultLinearDragCoefficient * Velocity);
            AddForce(linearDrag);

            Mass = mass; // mass updates the gravitational force when changed, so this needs to be assigned after the gravitational force is added
            Velocity = Vector2.Zero;
            Acceleration = Vector2.Zero;
            ResultantImpulse = Vector2.Zero;
        }

        #region Methods
        public void AddForce(Force force)
        {
            if (force.IsVelocityDependent)
            {
                VelocityDependentForces.Add(force);
            }
            else
            {
                NonVelocityDependentForces.Add(force);
            }
        }

        public void RemoveForce(Force force)
        {
            if (force.IsVelocityDependent)
            {
                VelocityDependentForces.Remove(force);
            }
            else
            {
                NonVelocityDependentForces.Remove(force);
            }
        }

        public void UpdateForce(Force newForce)
        {
            Force oldForce;
            if (newForce.IsVelocityDependent)
            {
                oldForce = VelocityDependentForces.Find(x => x.Type == newForce.Type);
            }
            else
            {
                oldForce = NonVelocityDependentForces.Find(x => x.Type == newForce.Type);
            }

            RemoveForce(oldForce);
            AddForce(newForce);
        }

        public void UpdateNonVelocityDependentForces()
        {
            // [No VelocityDependentForces currently need updating dynamically]
        }
        public void UpdateNonVelocityDependentForces(Vector2 position)
        {
            // [No VelocityDependentForces currently need updating dynamically]
        }

        public void UpdateVelocityDependentForces(Vector2 overrideVelocity)
        {
            Force newLinearDrag = new(ForceType.LinearDrag, -Config.DefaultLinearDragCoefficient * overrideVelocity);
            UpdateForce(newLinearDrag);
        }
        public void UpdateVelocityDependentForces()
        {
            Force newLinearDrag = new(ForceType.LinearDrag, -Config.DefaultLinearDragCoefficient * Velocity);
            UpdateForce(newLinearDrag);
        }

        public void ApplyImpulse(Vector2 impulse)
        {
            ResultantImpulse += impulse;
        }

        public void ClearImpulses()
        {
            ResultantImpulse = Vector2.Zero;
        }
        #endregion

        #region Helper Methods
        private void UpdateMassRelatedProperties(float newMass)
        {
            InverseMass = 1f / newMass;

            Force newGravitationalForce = new(ForceType.Gravitational, newMass, Config.GravitationalAcceleration);
            UpdateForce(newGravitationalForce);
        }
        #endregion
    }
}
