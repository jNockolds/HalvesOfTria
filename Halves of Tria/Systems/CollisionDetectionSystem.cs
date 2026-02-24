using Halves_of_Tria.Caches;
using Halves_of_Tria.Components;
using Microsoft.Xna.Framework;
using MonoGame.Extended;
using MonoGame.Extended.ECS;
using MonoGame.Extended.ECS.Systems;
using System.Diagnostics;
using System;

namespace Halves_of_Tria.Systems
{
    internal class CollisionDetectionSystem : EntityUpdateSystem
    {
        #region Fields and Components
        private ComponentMapper<AxisAlignedRectCollider> _axisAlignedRectColliderMapper;
        private ComponentMapper<CircleCollider> _circleColliderMapper;
        private ComponentMapper<Transform2> _transformMapper;

        private readonly CollisionsCache _collisionsCache;
        #endregion

        public CollisionDetectionSystem(CollisionsCache collisionsCache)
            : base(Aspect.All(typeof(Transform2))
                  .One(typeof(AxisAlignedRectCollider), typeof(CircleCollider)))
        {
            _collisionsCache = collisionsCache;
        }
        
        #region Game Loop Methods
        public override void Initialize(IComponentMapperService mapperService)
        {
            _axisAlignedRectColliderMapper = mapperService.GetMapper<AxisAlignedRectCollider>();
            _circleColliderMapper = mapperService.GetMapper<CircleCollider>();
            _transformMapper = mapperService.GetMapper<Transform2>();
        }

        public override void Update(GameTime gameTime)
        {
            for (int entityId1 = 0; entityId1 < ActiveEntities.Count - 1; entityId1++)
            {
                Transform2 transform1 = _transformMapper.Get(entityId1);

                for (int entityId2 = entityId1 + 1; entityId2 < ActiveEntities.Count; entityId2++)
                {
                    //if (!_dynamicBodyMapper.Has(entityId1)
                    //    && !_dynamicBodyMapper.Has(entityId2)) // if neither has a DynamicBody, then neither needs to be updated
                    //{
                    //    continue;
                    //}

                    Transform2 transform2 = _transformMapper.Get(entityId2);

                    // Rect-Rect:
                    if (_axisAlignedRectColliderMapper.TryGet(entityId1, out AxisAlignedRectCollider rect1)
                        && _axisAlignedRectColliderMapper.TryGet(entityId2, out AxisAlignedRectCollider rect2))
                    {
                        if (Intersects(transform1, rect1, transform2, rect2))
                        {
                            Debug.WriteLine($"Collision detected between entities {entityId1} and {entityId2}.");
                            CollisionData collisionData = new(); // todo: calculate collision normal and collision depth
                            _collisionsCache.ConfirmedCollisions.Add(collisionData);
                        }
                    }

                    // Circle-Circle:
                    else if (_circleColliderMapper.TryGet(entityId1, out CircleCollider circle1)
                        && _circleColliderMapper.TryGet(entityId2, out CircleCollider circle2))
                    {
                        if (Intersects(transform1, circle1, transform2, circle2))
                        {
                            Debug.WriteLine($"Collision detected between entities {entityId1} and {entityId2}.");
                            CollisionData collisionData = new(); // todo: calculate collision normal and collision depth
                            _collisionsCache.ConfirmedCollisions.Add(collisionData);
                        }
                    }

                    // Rect-Circle:
                    else if (_axisAlignedRectColliderMapper.TryGet(entityId1, out AxisAlignedRectCollider rect_RC)
                        && _circleColliderMapper.TryGet(entityId2, out CircleCollider circle_RC))
                    {
                        if (Intersects(transform1, rect_RC, transform2, circle_RC))
                        {
                            Debug.WriteLine($"Collision detected between entities {entityId1} and {entityId2}.");
                            CollisionData collisionData = new(); // todo: calculate collision normal and collision depth
                            _collisionsCache.ConfirmedCollisions.Add(collisionData);
                        }
                    }

                    // Circle-Rect:
                    else if (_circleColliderMapper.TryGet(entityId1, out CircleCollider circle_CR)
                             && _axisAlignedRectColliderMapper.TryGet(entityId2, out AxisAlignedRectCollider rect_CR))
                    {
                        if (Intersects(transform2, rect_CR, transform1, circle_CR))
                        {
                            Debug.WriteLine($"Collision detected between entities {entityId1} and {entityId2}.");
                            CollisionData collisionData = new(); // todo: calculate collision normal and collision depth
                            _collisionsCache.ConfirmedCollisions.Add(collisionData);
                        }
                    }
                }
            }
        }
        #endregion

        #region Collision Dectection Methods
        // To Implement later on: 
        // - Broad-phase and then narrow-phase checks for collisions (for optimisation, but only if needed)


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

        public bool Intersects(Transform2 rectTransform, AxisAlignedRectCollider rect, Transform2 circleTransform, CircleCollider circle)
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

        // Collision normal getters:

        private Vector2 GetCollisionNormal(Transform2 transform1, AxisAlignedRectCollider rect1, Transform2 transform2, AxisAlignedRectCollider rect2)
        {
            return Vector2.Zero; // placeholder value
        }

        private Vector2 GetCollisionNormal(Transform2 transform1, CircleCollider circle1, Transform2 transform2, CircleCollider circle2)
        {
            return Vector2.Zero; // placeholder value
        }

        private Vector2 GetCollisionNormal(Transform2 rectTransform, AxisAlignedRectCollider rect, Transform2 circleTransform, CircleCollider circle)
        {
            return Vector2.Zero; // placeholder value
        }
        #endregion
    }
}
