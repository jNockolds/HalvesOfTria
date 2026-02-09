using Microsoft.Xna.Framework;

namespace Halves_of_Tria.Components
{
    internal class DynamicBody
    {
        public Vector2 ResultantForce;
        public Vector2 ResultantImpulse;

        public DynamicBody()
        {
            ResultantForce = Vector2.Zero;
            ResultantImpulse = Vector2.Zero;
        }
    }
}
