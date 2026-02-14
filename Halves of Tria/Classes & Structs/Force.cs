using Microsoft.Xna.Framework;

namespace Halves_of_Tria
{
    internal struct Force
    {
        public string Name { get; }
        public Vector2 Value { get; }
        public bool IsVelocityDependent { get; }

        public Force(string name, Vector2 value, bool isVelocityDependent = false)
        {
            Name = name;
            Value = value;
            IsVelocityDependent = isVelocityDependent;
        }
    }
}
