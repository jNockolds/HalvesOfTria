using Microsoft.Xna.Framework;
using System;

namespace Halves_of_Tria.Components
{
    internal class CircleCollider
    {
        private float _radius;
        public float Radius
        {
            get => _radius;
            set
            {
                if (value <= 0)
                    throw new ArgumentOutOfRangeException("Radius must be greater than zero.", nameof(value));
                _radius = value;
                RadiusSquared = value * value;
            }
        }
        public float RadiusSquared { get; private set; }

        public CircleCollider(float radius)
        {
            Radius = radius;
        }
    }
}
