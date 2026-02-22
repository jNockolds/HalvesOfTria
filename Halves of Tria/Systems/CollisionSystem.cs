using Halves_of_Tria.Components;
using Microsoft.Xna.Framework;
using MonoGame.Extended;
using MonoGame.Extended.ECS;
using MonoGame.Extended.ECS.Systems;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Halves_of_Tria.Systems
{
    internal class CollisionSystem : EntityUpdateSystem
    {

        #region Fields and Components
        private ComponentMapper<AxisAlignedRectCollider> _axisAlignedRectColliderMapper;
        private ComponentMapper<CircleCollider> _circleColliderMapper;
        private ComponentMapper<CapsuleCollider> _capsuleColliderMapper;
        private ComponentMapper<Transform2> _transformMapper;
        #endregion

        public CollisionSystem()
            : base(Aspect.All(typeof(Transform2))
                  .One(typeof(AxisAlignedRectCollider), typeof(CircleCollider), typeof(CapsuleCollider))) { }
        
        #region Game Loop Methods
        public override void Initialize(IComponentMapperService mapperService)
        {
            _axisAlignedRectColliderMapper = mapperService.GetMapper<AxisAlignedRectCollider>();
            _circleColliderMapper = mapperService.GetMapper<CircleCollider>();
            _capsuleColliderMapper = mapperService.GetMapper<CapsuleCollider>();
            _transformMapper = mapperService.GetMapper<Transform2>();
        }

        public override void Update(GameTime gameTime)
        {
            
        }
        #endregion

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
