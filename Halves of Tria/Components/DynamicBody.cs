using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

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
                InverseMass = 1f / value;
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
            Mass = mass;
            Velocity = Vector2.Zero;
            Acceleration = Vector2.Zero;
            ResultantImpulse = Vector2.Zero;
        }

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
                NonVelocityDependentForces.Add(force);
            }
        }

        public void ApplyImpulse(Vector2 impulse)
        {
            ResultantImpulse += impulse;
        }

        public void ClearImpulses()
        {
            ResultantImpulse = Vector2.Zero;
        }
    }
}
