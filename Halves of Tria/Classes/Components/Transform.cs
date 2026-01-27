using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;


namespace Halves_of_Tria.Classes.Components
{
    class Transform : Component
    {
        public Vector2 Position = Vector2.Zero;
        public float Rotation = 0f;
        public Vector2 Scale = Vector2.One;
        public float LayerDepth = 0f;

        public Transform()
        {
            TransformSystem.Register(this);
        }
    }
}
