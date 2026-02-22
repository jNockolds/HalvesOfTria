using System;

namespace Halves_of_Tria.Components
{
    internal class AxisAlignedRectCollider
    {
        private float _width;
        public float Width
        {
            get => _width;
            set
            {
                if (value <= 0)
                    throw new ArgumentOutOfRangeException("Width must be greater than zero.", nameof(value));
                _width = value;
            }
        }

        private float _height;
        public float Height
        {
            get => _height;
            set
            {
                if (value <= 0)
                    throw new ArgumentOutOfRangeException("Height must be greater than zero.", nameof(value));
                _height = value;
            }
        }

        public AxisAlignedRectCollider(float width, float height)
        {
            Width = width;
            Height = height;
        }
    }
}
