using HalvesOfTria.Interfaces;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace HalvesOfTria.Classes
{
    public class StaticBlock : IStaticCollideable
    {
        #region Properties
        public readonly PhysicsProperties.RestitutionModifier RestitutionModifier;
        public readonly PhysicsProperties.FrictionModifier FrictionModifier;
        #endregion


        #region Fields
        private readonly Rectangle _boundary;
        private readonly Sprite _boundaryHitboxSprite;
        private Vector2 _centre => new Vector2(
            _boundary.X + 0.5f * _boundary.Width,
            _boundary.Y + 0.5f * _boundary.Height
        );
        #endregion


        #region Constructor
        /// <summary>
        /// Creates a new StaticBlock with the specified dimensions, position, and material type.
        /// <para>By default, the material type is set to Stone.</para>
        /// </summary>
        /// <exception cref="ArgumentException">Thrown when the top is not less than the bottom or the left is not less than the right.</exception>"
        public StaticBlock(GraphicsDevice graphicsDevice, Vector2 centre, int width, int height,
            PhysicsProperties.RestitutionModifier restitutionModifier = PhysicsProperties.RestitutionModifier.Low, PhysicsProperties.FrictionModifier frictionModifier = default)
        {
            if (height <= 0 || width <= 0)
            {
                throw new ArgumentException("Invalid boundary dimensions: size must be positive.");
            }
            if (!IsWithinScreenBounds(centre, width, height))
            {
                throw new ArgumentException("Invalid position: StaticBlock must be within the screen bounds.");
            }
            _boundary = new Rectangle((int)centre.X - (width / 2), (int)centre.Y - (height / 2), width, height);
            Texture2D _spriteTexture = TextureMaker.GenerateRectangleTexture(
                graphicsDevice,
                width,
                height,
                Color.White,
                true
            );
            _boundaryHitboxSprite = new Sprite(
                _spriteTexture,
                centre
            );
            RestitutionModifier = restitutionModifier;
            FrictionModifier = frictionModifier;
        }
        #endregion


        #region Update Method
        public void Update(GameTime gameTime)
        {
            HandleIncomingCollisions(Game1.EntityNodes);
        }
        #endregion


        #region Draw Method
        public void Draw(SpriteBatch spriteBatch)
        {
            _boundaryHitboxSprite.Draw(spriteBatch);
        }
        #endregion


        #region Boolean Methods
        public bool IsTopInContactWith(EntityNode entityNode)
        {
            Rectangle aabb = entityNode.Hitbox.GetAABB();
            return TopHalfIntersectsAabbBottom(aabb);
        }

        public bool IsBottomInContactWith(EntityNode entityNode)
        {
            Rectangle aabb = entityNode.Hitbox.GetAABB();
            return BottomHalfIntersectsAabbTop(aabb);
        }

        public bool IsLeftEdgeInContactWith(EntityNode entityNode)
        {
            Rectangle aabb = entityNode.Hitbox.GetAABB();
            return LeftHalfIntersectsAabbRight(aabb);
        }

        public bool IsRightEdgeInContactWith(EntityNode entityNode)
        {
            Rectangle aabb = entityNode.Hitbox.GetAABB();
            return RightHalfIntersectsAabbLeft(aabb);
        }
        #endregion


        #region Helper Methods
        private bool IsWithinScreenBounds(Vector2 centre, int width, int height)
        {
            return !(
                centre.X - 0.5 * width < 0
                || centre.Y - 0.5 * height < 0
                || centre.X + 0.5 * width > PhysicsProperties.WindowWidth
                || centre.Y + 0.5 * height > PhysicsProperties.WindowHeight
            );
        }

        private bool TopHalfIntersectsAabbBottom(Rectangle aabb)
        {
            return (
                aabb.Bottom <= _boundary.Top
                && aabb.Bottom >= _boundary.Top - 0.5 * _boundary.Height
                && aabb.Right >= _boundary.Left
                && aabb.Left <= _boundary.Right
            );
        }

        private bool BottomHalfIntersectsAabbTop(Rectangle aabb)
        {
            return (
                aabb.Top >= _boundary.Bottom - 0.5 * _boundary.Height
                && aabb.Top <= _boundary.Bottom
                && aabb.Right >= _boundary.Left
                && aabb.Left <= _boundary.Right
            );
        }

        private bool RightHalfIntersectsAabbLeft(Rectangle aabb)
        {
            return (
                aabb.Left <= _boundary.Right
                && aabb.Left >= _boundary.Right - 0.5 * _boundary.Width
                && aabb.Top <= _boundary.Bottom
                && aabb.Bottom >= _boundary.Top
            );
        }

        private bool LeftHalfIntersectsAabbRight(Rectangle aabb)
        {
            return (
                aabb.Right >= _boundary.Left
                && aabb.Right <= _boundary.Left + 0.5 * _boundary.Width
                && aabb.Top <= _boundary.Bottom
                && aabb.Bottom >= _boundary.Top
            );
        }

        private void HandleIncomingCollisions(List<EntityNode> entityNodes)
        {
            foreach (EntityNode entityNode in entityNodes)
            {
                HandleCollision(entityNode);
            }
        }

        private void HandleCollision(EntityNode entityNode)
        {
            Rectangle aabb = entityNode.Hitbox.GetAABB();
            if (!_boundary.Intersects(aabb))
            {
                return;
            }

            Vector2 closestPoint = GetClosestPointInBlock(entityNode);

            float distanceX = entityNode.Position.X - closestPoint.X;
            float distanceY = entityNode.Position.Y - closestPoint.Y;
            if ((distanceX * distanceX + distanceY * distanceY) < entityNode.Radius * entityNode.Radius)
            {
                float distanceFromLeft = Math.Abs(entityNode.Position.X - _boundary.Left);
                float distanceFromRight = Math.Abs(_boundary.Right - entityNode.Position.X);
                float distanceFromTop = Math.Abs(entityNode.Position.Y - _boundary.Top);
                float distanceFromBottom = Math.Abs(_boundary.Bottom - entityNode.Position.Y);

                float minDistance = Math.Min(
                    Math.Min(distanceFromLeft, distanceFromRight),
                    Math.Min(distanceFromTop, distanceFromBottom)
                );

                if (EqualWithinEpsilon(minDistance, distanceFromLeft))
                {
                    RespondToLeftCollision(entityNode);
                }
                else if (EqualWithinEpsilon(minDistance, distanceFromRight))
                {
                    RespondToRightCollision(entityNode);
                }
                else if (EqualWithinEpsilon(minDistance, distanceFromTop))
                {
                    RespondToTopCollision(entityNode);
                }
                else if (EqualWithinEpsilon(minDistance, distanceFromBottom))
                {
                    RespondToBottomCollision(entityNode);
                }
            }
        }

        private static bool EqualWithinEpsilon(float a, float b, float epsilon = 0.0001f)
        {
            return Math.Abs(a - b) <= epsilon;
        }

        private Vector2 GetClosestPointInBlock(EntityNode entityNode)
        {
            float closestX = MathHelper.Clamp(entityNode.Position.X, _boundary.Left, _boundary.Right);
            float closestY = MathHelper.Clamp(entityNode.Position.Y, _boundary.Top, _boundary.Bottom);
            return new Vector2(closestX, closestY);
        }

        private void RespondToLeftCollision(EntityNode entityNode)
        {
            entityNode.Position = new Vector2(
                _boundary.Left - entityNode.Radius,
                entityNode.Position.Y
            );

            float restitution = PhysicsProperties.GetRestitution(
                RestitutionModifier,
                entityNode.RestitutionModifier
            );
            entityNode.Velocity = new Vector2(
                restitution * -entityNode.Velocity.X,
                entityNode.Velocity.Y
            );
        }

        private void RespondToRightCollision(EntityNode entityNode)
        {
            entityNode.Position = new Vector2(
                _boundary.Right + entityNode.Radius,
                entityNode.Position.Y
            );
            float restitution = PhysicsProperties.GetRestitution(
                RestitutionModifier,
                entityNode.RestitutionModifier
            );
            entityNode.Velocity = new Vector2(
                restitution * -entityNode.Velocity.X,
                entityNode.Velocity.Y
            );
        }

        private void RespondToTopCollision(EntityNode entityNode)
        {
            entityNode.Position = new Vector2(
                entityNode.Position.X,
                _boundary.Top - entityNode.Radius
            );
            float restitution = PhysicsProperties.GetRestitution(
                RestitutionModifier,
                entityNode.RestitutionModifier
            );
            entityNode.Velocity = new Vector2(
                entityNode.Velocity.X,
                restitution * -entityNode.Velocity.Y
            );
        }

        private void RespondToBottomCollision(EntityNode entityNode)
        {
            entityNode.Position = new Vector2(
                entityNode.Position.X,
                _boundary.Bottom + entityNode.Radius
            );
            float restitution = PhysicsProperties.GetRestitution(
                RestitutionModifier,
                entityNode.RestitutionModifier
            );
            entityNode.Velocity = new Vector2(
                entityNode.Velocity.X,
                restitution * -entityNode.Velocity.Y
            );
        }
        #endregion
    }
}