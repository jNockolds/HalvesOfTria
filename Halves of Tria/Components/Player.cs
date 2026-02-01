using Microsoft.Xna.Framework;

namespace Halves_of_Tria.Components
{
    internal class Player
    {
        public int Speed;
        public Vector2 Position;

        public Player(int speed, Vector2 initialPosition)
        {
            Speed = speed;
            Position = initialPosition;
        }
    }
}
