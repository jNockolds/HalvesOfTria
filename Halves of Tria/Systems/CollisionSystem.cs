using Halves_of_Tria.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using MonoGame.Extended;

namespace Halves_of_Tria.Systems
{
    internal class CollisionSystem
    {







        #region Collision Dectection Methods
        public bool Intersects(Transform2 transform1, CircleCollider circle1, Transform2 transform2, CircleCollider circle2)
        {
            float distanceSquared = Vector2.DistanceSquared(transform1.Position, transform2.Position);
            float radiusSum = circle1.Radius + circle2.Radius;
            return distanceSquared <= radiusSum * radiusSum;
        }

        public bool Intersects(Transform2 circleTransform, CircleCollider circle, Transform2 rectTransform, AxisAlignedRectCollider rect)
        {
            Vector2 rectTopLeft = TopLeft(rectTransform, rect);
            Vector2 rectBottomRight = BottomRight(rectTransform, rect);
            Vector2 closestPointInRectToCircle = Vector2.Clamp(circleTransform.Position, rectTopLeft, rectBottomRight);
            return Contains(circleTransform, circle, closestPointInRectToCircle);
        }
        #endregion

        #region Helper Methods
        private bool Contains(Transform2 transform, CircleCollider circle, Vector2 point)
        {
            float distanceSquared = Vector2.DistanceSquared(transform.Position, point);
            return distanceSquared <= circle.RadiusSquared;
        }

        private Vector2 TopLeft(Transform2 transform, AxisAlignedRectCollider rect)
        {
            return transform.Position - 0.5f * new Vector2(rect.Width, rect.Height);
        }
        private Vector2 BottomRight(Transform2 transform, AxisAlignedRectCollider rect)
        {
            return transform.Position + 0.5f * new Vector2(rect.Width, rect.Height);
        }
        #endregion
    }
}
