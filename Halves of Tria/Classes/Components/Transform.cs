using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;


namespace Halves_of_Tria.Classes.Components
{
    /// <summary>
    /// Represents the position, rotation, scale, and layer depth of an entity in 2D space.
    /// </summary>
    /// <remarks>The default values correspond to the origin position, no rotation, unit scale,
    /// and a layer depth of 0.</remarks>
    internal class Transform : Component
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
