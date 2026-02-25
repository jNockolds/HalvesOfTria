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
    // To Implement later on: 
    // - Broad-phase and then narrow-phase checks for collisions (for optimisation, but only if needed)
    internal class CollisionConstraintSystem : EntityUpdateSystem
    {
        #region Fields and Components
        private ComponentMapper<AxisAlignedRectCollider> _axisAlignedRectColliderMapper;
        private ComponentMapper<CircleCollider> _circleColliderMapper;
        private ComponentMapper<Transform> _transformMapper;
        private ComponentMapper<PhysicsBody> _physicsBodyMapper;

        private readonly CollisionsCache _collisionsCache;
        private const int _iterationsPerFrame = 6;
        #endregion

        public CollisionConstraintSystem(CollisionsCache collisionsCache)
            : base(Aspect.All(typeof(Transform))
                  .One(typeof(AxisAlignedRectCollider), typeof(CircleCollider)))
        {
            _collisionsCache = collisionsCache;
        }
        
        #region Game Loop Methods
        public override void Initialize(IComponentMapperService mapperService)
        {
            _axisAlignedRectColliderMapper = mapperService.GetMapper<AxisAlignedRectCollider>();
            _circleColliderMapper = mapperService.GetMapper<CircleCollider>();
            _transformMapper = mapperService.GetMapper<Transform>();
            _physicsBodyMapper = mapperService.GetMapper<PhysicsBody>();
        }

        public override void Update(GameTime gameTime)
        {
            for (int _ = 0; _ < _iterationsPerFrame; _++)
            {
                SolveAllConstraints();
            }
        }
        #endregion


        #region Helper Methods
        private void SolveAllConstraints()
        {
            for (int entityId1 = 0; entityId1 < ActiveEntities.Count - 1; entityId1++)
            {
                Transform transform1 = _transformMapper.Get(entityId1);

                // if the first collider doesn't have a body:
                bool hasBody1;
                if (_physicsBodyMapper.TryGet(entityId1, out PhysicsBody body1))
                    hasBody1 = true;
                else
                    hasBody1 = false;


                for (int entityId2 = entityId1 + 1; entityId2 < ActiveEntities.Count; entityId2++)
                {
                    Transform transform2 = _transformMapper.Get(entityId2);

                    // if the second collider doesn't have a body:
                    bool hasBody2;
                    if (_physicsBodyMapper.TryGet(entityId2, out PhysicsBody body2))
                        hasBody2 = true;
                    else
                        hasBody2 = false;

                    // Circle-Circle:
                    if (_circleColliderMapper.TryGet(entityId1, out CircleCollider circle1)
                        && _circleColliderMapper.TryGet(entityId2, out CircleCollider circle2))
                    {
                        Debug.WriteLine("investigating collision (Circle-Circle)");
                        CollisionData? collisionData = GetCollisionData(
                            entityId1, transform1, circle1,
                            entityId2, transform2, circle2);
                        if (collisionData is CollisionData data) // pattern matching to avoid null data
                        {
                            Debug.WriteLine($"Collision detected between entities {entityId1} and {entityId2}.");
                            _collisionsCache.ConfirmedCollisions.Add(data);

                            if (hasBody1 && hasBody2)
                            {
                                ResolvePhysicalCollision(transform1, body1, transform2, body2, data);
                            }
                        }
                    }

                    // Circle-Rect:
                    else if (_circleColliderMapper.TryGet(entityId1, out CircleCollider circle_CR)
                             && _axisAlignedRectColliderMapper.TryGet(entityId2, out AxisAlignedRectCollider rect_CR))
                    {
                        Debug.WriteLine("investigating collision (Circle-Rect)");
                        CollisionData? collisionData = GetCollisionData(
                            entityId1, transform1, circle_CR,
                            entityId2, transform2, rect_CR);
                        if (collisionData is CollisionData data) // pattern matching to avoid null data
                        {
                            Debug.WriteLine($"Collision detected between entities {entityId1} and {entityId2}.");
                            _collisionsCache.ConfirmedCollisions.Add(data);

                            if (hasBody1 && hasBody2)
                            {
                                ResolvePhysicalCollision(transform1, body1, transform2, body2, data);
                            }
                        }
                    }

                    // Rect-Circle:
                    else if (_axisAlignedRectColliderMapper.TryGet(entityId1, out AxisAlignedRectCollider rect_RC)
                        && _circleColliderMapper.TryGet(entityId2, out CircleCollider circle_RC))
                    {
                        Debug.WriteLine("investigating collision (Rect-Circle)");
                        CollisionData? collisionData = GetCollisionData(
                            entityId2, transform2, circle_RC,
                            entityId1, transform1, rect_RC);
                        if (collisionData is CollisionData data) // pattern matching to avoid null data
                        {
                            Debug.WriteLine($"Collision detected between entities {entityId1} and {entityId2}.");
                            _collisionsCache.ConfirmedCollisions.Add(data);

                            if (hasBody1 && hasBody2)
                            {
                                ResolvePhysicalCollision(transform2, body2, transform1, body1, data);
                            }
                        }
                    }

                    // Rect-Rect:
                    else if (_axisAlignedRectColliderMapper.TryGet(entityId1, out AxisAlignedRectCollider rect1)
                        && _axisAlignedRectColliderMapper.TryGet(entityId2, out AxisAlignedRectCollider rect2))
                    {
                        Debug.WriteLine("investigating collision (Rect-Rect)");
                        CollisionData? collisionData = GetCollisionData(
                            entityId1, transform1, rect1,
                            entityId2, transform2, rect2);
                        if (collisionData is CollisionData data) // pattern matching to avoid null data
                        {
                            Debug.WriteLine($"Collision detected between entities {entityId1} and {entityId2}.");
                            _collisionsCache.ConfirmedCollisions.Add(data);

                            if (hasBody1 && hasBody2)
                            {
                                ResolvePhysicalCollision(transform1, body1, transform2, body2, data);
                            }
                        }
                    }
                }
            }
        }

        private void ResolvePhysicalCollision(Transform transform1, PhysicsBody body1, Transform transform2, PhysicsBody body2, CollisionData collisionData)
        {
            float coeff1 = -body1.InverseMass / (body1.InverseMass + body2.InverseMass);
            float coeff2 = body2.InverseMass / (body1.InverseMass + body2.InverseMass);

            Vector2 deltaPosition1 = coeff1 * collisionData.Depth * collisionData.Normal;
            Vector2 deltaPosition2 = coeff2 * collisionData.Depth * collisionData.Normal;

            transform1.Position += deltaPosition1;
            transform2.Position += deltaPosition2;
        }


        // Collision Dectection Methods:

        // circle-circle:
        private CollisionData? GetCollisionData(
            int entityId1, Transform transform1, CircleCollider circle1,
            int entityId2, Transform transform2, CircleCollider circle2)
        {
            Vector2 relativePosition = transform2.Position - transform1.Position;
            float distance = relativePosition.Length();
            float depth = circle1.Radius + circle2.Radius - distance;

            if (depth <= 0)
            {
                return null;
            }

            Vector2 normal;

            if (distance > 0f)
            {
                normal = relativePosition.NormalizedCopy();
            }
            else
            {
                normal = Vector2.UnitY;
            }

            return new(entityId1, entityId2, normal, depth);
        }

        // circle-rect:
        private CollisionData? GetCollisionData(
            int entityId1, Transform circleTransform, CircleCollider circle,
            int entityId2, Transform rectTransform, AxisAlignedRectCollider rect)
        {
            Vector2 rectTopLeft = TopLeft(rectTransform, rect);
            Vector2 rectBottomRight = BottomRight(rectTransform, rect);
            Vector2 closestPointInRectToCircle = Vector2.Clamp(circleTransform.Position, rectTopLeft, rectBottomRight);

            Vector2 fromRectToCircle = circleTransform.Position - closestPointInRectToCircle;
            float distance = fromRectToCircle.Length();

            if (distance >= circle.Radius)
            {
                return null;
            }

            Vector2 normal;
            float depth;

            if (distance > 0f)
            {
                normal = Vector2.Normalize(fromRectToCircle);
                depth = circle.Radius - distance;
            }
            else
            {
                float horizontalDepth = rect.HalfWidth - Math.Abs(circleTransform.Position.X - rectTransform.Position.X);
                float verticalDepth = rect.HalfHeight - Math.Abs(circleTransform.Position.Y - rectTransform.Position.Y);

                if (horizontalDepth < verticalDepth)
                {
                    depth = horizontalDepth;
                    normal = new Vector2(Math.Sign(circleTransform.Position.X - rectTransform.Position.X), 0f);
                }
                else
                {
                    depth = verticalDepth;
                    normal = new Vector2(0f, Math.Sign(circleTransform.Position.Y - rectTransform.Position.Y));
                }
            }

            return new(entityId1, entityId2, normal, depth);
        }

        // rect-rect:
        private CollisionData? GetCollisionData(
            int entityId1, Transform transform1, AxisAlignedRectCollider rect1,
            int entityId2, Transform transform2, AxisAlignedRectCollider rect2)
        {
            float overlapX = rect1.HalfWidth + rect2.HalfWidth - Math.Abs(transform1.Position.X - transform2.Position.X);
            if (overlapX <= 0)
            {
                return null;
            }

            float overlapY = rect1.HalfHeight + rect2.HalfHeight - Math.Abs(transform1.Position.Y - transform2.Position.Y);
            if (overlapY <= 0)
            {
                return null;
            }

            Vector2 normal;
            float depth;

            if (overlapX < overlapY)
            {
                normal = new Vector2(Math.Sign(transform2.Position.X - transform1.Position.X), 0f);
                depth = overlapX;
            }
            else
            {
                normal = new Vector2(0f, Math.Sign(transform2.Position.Y - transform1.Position.Y));
                depth = overlapY;
            }

            return new(entityId1, entityId2, normal, depth);
        }

        private Vector2 TopLeft(Transform transform, AxisAlignedRectCollider rect)
        {
            return transform.Position - 0.5f * new Vector2(rect.Width, rect.Height);
        }
        private Vector2 BottomRight(Transform transform, AxisAlignedRectCollider rect)
        {
            return transform.Position + 0.5f * new Vector2(rect.Width, rect.Height);
        }
        #endregion
    }
}
