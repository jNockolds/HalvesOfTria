using Halves_of_Tria.Components;
using Microsoft.Xna.Framework;
using MonoGame.Extended;
using MonoGame.Extended.ECS;
using MonoGame.Extended.ECS.Systems;
using System.Diagnostics;
using System;

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
            

            for (int entityId1 = 0; entityId1 < ActiveEntities.Count - 1; entityId1++)
            {
                Transform2 transform1 = _transformMapper.Get(entityId1);

                for (int entityId2 = entityId1 + 1; entityId2 < ActiveEntities.Count; entityId2++)
                {
                    Transform2 transform2 = _transformMapper.Get(entityId2);

                    // Rect-Rect:
                    if (_axisAlignedRectColliderMapper.TryGet(entityId1, out AxisAlignedRectCollider rect1)
                        && _axisAlignedRectColliderMapper.TryGet(entityId1, out AxisAlignedRectCollider rect2))
                    {
                        if (Intersects(transform1, rect1, transform2, rect2))
                        {
                            Debug.WriteLine($"Collision detected between entities {entityId1} and {entityId2}.");
                        }
                    }

                    // Circle-Circle:
                    else if (_circleColliderMapper.TryGet(entityId1, out CircleCollider circle1)
                        && _circleColliderMapper.TryGet(entityId1, out CircleCollider circle2))
                    {
                        if (Intersects(transform1, circle1, transform2, circle2))
                        {
                            Debug.WriteLine($"Collision detected between entities {entityId1} and {entityId2}.");
                        }
                    }

                    // Capsule-Capsule:
                    else if (_capsuleColliderMapper.TryGet(entityId1, out CapsuleCollider capsule1)
                        && _capsuleColliderMapper.TryGet(entityId1, out CapsuleCollider capsule2))
                    {
                        if (Intersects(transform1, capsule1, transform2, capsule2))
                        {
                            Debug.WriteLine($"Collision detected between entities {entityId1} and {entityId2}.");
                        }
                    }

                    // Rect-Circle:
                    else if (_axisAlignedRectColliderMapper.TryGet(entityId1, out AxisAlignedRectCollider rect_RC)
                        && _circleColliderMapper.TryGet(entityId2, out CircleCollider circle_RC))
                    {
                        if (Intersects(transform1, rect_RC, transform2, circle_RC))
                        {
                            Debug.WriteLine($"Collision detected between entities {entityId1} and {entityId2}.");
                        }
                    }

                    // Circle-Rect:
                    else if (_circleColliderMapper.TryGet(entityId1, out CircleCollider circle_CR)
                             && _axisAlignedRectColliderMapper.TryGet(entityId2, out AxisAlignedRectCollider rect_CR))
                    {
                        if (Intersects(transform2, rect_CR, transform1, circle_CR))
                        {
                            Debug.WriteLine($"Collision detected between entities {entityId1} and {entityId2}.");
                        }
                    }

                    // Rect-Capsule:
                    else if (_axisAlignedRectColliderMapper.TryGet(entityId1, out AxisAlignedRectCollider rectR_Ca)
                             && _capsuleColliderMapper.TryGet(entityId2, out CapsuleCollider capsule_RCa))
                    {
                        if (Intersects(transform2, rectR_Ca, transform1, capsule_RCa))
                        {
                            Debug.WriteLine($"Collision detected between entities {entityId1} and {entityId2}.");
                        }
                    }

                    // Capsule-Rect:
                    else if (_capsuleColliderMapper.TryGet(entityId1, out CapsuleCollider capsule_CaR)
                             && _axisAlignedRectColliderMapper.TryGet(entityId2, out AxisAlignedRectCollider rect_CaR))
                    {
                        if (Intersects(transform1, rect_CaR, transform2, capsule_CaR))
                        {
                            Debug.WriteLine($"Collision detected between entities {entityId1} and {entityId2}.");
                        }
                    }

                    // Circle-Capsule:
                    else if (_circleColliderMapper.TryGet(entityId1, out CircleCollider circle_CCa)
                             && _capsuleColliderMapper.TryGet(entityId2, out CapsuleCollider capsule_CCa))
                    {
                        if (Intersects(transform2, circle_CCa, transform1, capsule_CCa))
                        {
                            Debug.WriteLine($"Collision detected between entities {entityId1} and {entityId2}.");
                        }
                    }

                    // Capsule-Circle:
                    else if (_capsuleColliderMapper.TryGet(entityId1, out CapsuleCollider capsule_CaC)
                             && _circleColliderMapper.TryGet(entityId2, out CircleCollider circle_CaC))
                    {
                        if (Intersects(transform1, circle_CaC, transform2, capsule_CaC))
                        {
                            Debug.WriteLine($"Collision detected between entities {entityId1} and {entityId2}.");
                        }
                    }
                }
            }
        }
        #endregion

        #region Collision Dectection Methods
        // To Implement later on: 
        // - Broad-phase and then narrow-phase checks for collisions (for optimisation, but only if needed)

        // To Implement:
        // - Rect-Rect
        // - Capsule-Capsule
        // - Rect-Capsule
        // - Circle-Capsule

        public bool Intersects(Transform2 transform1, AxisAlignedRectCollider rect1, Transform2 transform2, AxisAlignedRectCollider rect2)
        {
            float horizontalDistance = Math.Abs(transform1.Position.X - transform2.Position.X);
            float verticalDistance = Math.Abs(transform1.Position.Y - transform2.Position.Y);

            return horizontalDistance <= 0.5f * (rect1.Width + rect2.Width) 
                && verticalDistance <= 0.5f * (rect1.Height + rect2.Height);
        }

        public bool Intersects(Transform2 transform1, CircleCollider circle1, Transform2 transform2, CircleCollider circle2)
        {
            float distanceSquared = Vector2.DistanceSquared(transform1.Position, transform2.Position);
            float radiusSum = circle1.Radius + circle2.Radius;
            return distanceSquared <= radiusSum * radiusSum;
        }

        public bool Intersects(Transform2 transform1, CapsuleCollider capsule1, Transform2 transform2, CapsuleCollider capsule2)
        {
            return false; // [placeholder]
        }

        public bool Intersects(Transform2 rectTransform, AxisAlignedRectCollider rect, Transform2 circleTransform, CircleCollider circle)
        {
            Vector2 rectTopLeft = TopLeft(rectTransform, rect);
            Vector2 rectBottomRight = BottomRight(rectTransform, rect);
            Vector2 closestPointInRectToCircle = Vector2.Clamp(circleTransform.Position, rectTopLeft, rectBottomRight);
            return Contains(circleTransform, circle, closestPointInRectToCircle);
        }

        public bool Intersects(Transform2 rectTransform, AxisAlignedRectCollider rect, Transform2 capsuleTransform, CapsuleCollider capsule)
        {
            return false; // [placeholder]
        }

        public bool Intersects(Transform2 circleTransform, CircleCollider circle, Transform2 capsuleTransform, CapsuleCollider capsule)
        {
            return false; // [placeholder]
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
