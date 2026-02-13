using Microsoft.Xna.Framework;
using System;

namespace Halves_of_Tria.Components
{
    internal class DynamicBody
    {
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
        /// Represents the inverse of the object's mass; i.e. 1 / <see cref="Mass"/>.
        /// </summary>
        public float InverseMass;
        public Vector2 Velocity;
        public Vector2 Acceleration;
        public Vector2 ResultantForce => Mass * Acceleration;
        public Vector2 ResultantImpulse;

        public DynamicBody(float mass)
        {
            Mass = mass;
            Velocity = Vector2.Zero;
            Acceleration = Vector2.Zero;
            ResultantImpulse = Vector2.Zero;
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
