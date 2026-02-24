using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace Halves_of_Tria
{
    internal struct CollisionData
    {
        public int EntityId1 { get; set; }
        public int EntityId2 { get; set; }
        public Vector2 Normal { get; set; }
        public float Depth { get; set; }

        public CollisionData(int entityId1, int entityId2, Vector2 normal, float depth)
        {
            EntityId1 = entityId1;
            EntityId2 = entityId2;
            Normal = normal;
            Depth = depth;
        }
    }
}
