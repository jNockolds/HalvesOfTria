using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace Halves_of_Tria.Components
{
    internal class Transform
    {
        public Vector2 PreviousPosition { get; set; }
        public Vector2 Position { get; set; }
        public float Rotation { get; set; }
        public Vector2 Scale { get; set; }

        public Transform(Vector2 position, float rotation = 0f, Vector2? scale = null)
        {
            PreviousPosition = position;
            Position = position;
            Rotation = rotation;
            Scale = scale ?? Vector2.One;
        }
        public Transform()
            : this(Vector2.Zero, 0f, Vector2.One) { }
    }
}
