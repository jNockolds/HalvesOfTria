using Microsoft.Xna.Framework;

namespace Halves_of_Tria
{
    public enum ForceType
    {
        Gravitational,
        LinearDrag,
        Normal
    }

    internal struct Force
    {
        public ForceType Type { get; }
        public Vector2 Value { get; }
        public bool IsVelocityDependent { get; }

        public Force(ForceType type, Vector2 value)
        {
            Type = type;
            Value = value;

            switch (type)
            {
                case ForceType.LinearDrag:
                    IsVelocityDependent = true;
                    break;
                case ForceType.Gravitational:
                    IsVelocityDependent = false;
                    break;
                default:
                    IsVelocityDependent = false;
                    break;
            }
        }

        public Force(ForceType type, float mass, Vector2 acceleration)
            : this(type, mass * acceleration) { }
    }
}
