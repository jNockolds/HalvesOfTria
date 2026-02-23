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
        public Vector2 CollisionNormal { get; set; }
        public float PenetrationDistance { get; set; }

        public CollisionData(int entityId1, int entityId2, Vector2 collisionNormal, float penetrationDistance)
        {
            EntityId1 = entityId1;
            EntityId2 = entityId2;
            CollisionNormal = collisionNormal;
            PenetrationDistance = penetrationDistance;
        }
    }
}
