using HalvesOfTria.Interfaces;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace HalvesOfTria.Classes
{
    // [note: just a rectangle at the moment;
    // maybe change name to 'AbsoluteRoomBoundary' and have a 'RoomBoundary' Class
    // with an 'AbsoluteRoomBoundary' component plus some floating blocks to make a more complexly-shaped room?]
    public class RoomBoundary : IStaticCollideable
    {
        #region Properties
        public readonly PhysicsProperties.MaterialType MaterialType;
        #endregion


        #region Fields
        private readonly Rectangle _boundary;
        private readonly Sprite _boundaryHitboxSprite;
        #endregion


        #region Constructor
        /// <summary>
        /// Creates a new RoomBoundary with the specified dimensions and material type at the centre of the screen.
        /// <para>By default, the material type is set to Stone.</para>
        /// </summary>
        /// <exception cref="ArgumentException">Thrown when the top is not less than the bottom or the left is not less than the right.</exception>"
        public RoomBoundary(GraphicsDevice graphicsDevice, int width, int height, PhysicsProperties.MaterialType materialType = PhysicsProperties.MaterialType.Stone)
        {
            if (height <= 0 || width <= 0)
            {
                throw new ArgumentException("Invalid boundary dimensions: size must be positive.");
            }
            _boundary = new Rectangle((PhysicsProperties.WindowWidth - width) / 2, (PhysicsProperties.WindowHeight - height) / 2, width, height);
            Texture2D _spriteTexture = TextureMaker.GenerateRectangleTexture(
                graphicsDevice,
                width,
                height,
                Color.White
            );
            Vector2 spriteCentre = new Vector2(PhysicsProperties.WindowWidth / 2, PhysicsProperties.WindowHeight / 2);
            _boundaryHitboxSprite = new Sprite(
                _spriteTexture,
                spriteCentre
            );
            MaterialType = materialType;
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
        public bool IsFloorInContactWith(EntityNode entityNode)
        {
            Rectangle aabb = entityNode.Hitbox.GetAABB();
            return aabb.Bottom >= _boundary.Bottom;
        }

        public bool IsCeilingInContactWith(EntityNode entityNode)
        {
            Rectangle aabb = entityNode.Hitbox.GetAABB();
            return aabb.Top <= _boundary.Top;
        }

        public bool IsHorizontalEdgeInContactWith(EntityNode entityNode)
        {
            Rectangle aabb = entityNode.Hitbox.GetAABB();
            return IsFloorInContactWith(entityNode) 
                || IsCeilingInContactWith(entityNode);
        }

        public bool IsLeftEdgeInContactWith(EntityNode entityNode)
        {
            Rectangle aabb = entityNode.Hitbox.GetAABB();
            return aabb.Left <= _boundary.Left;
        }

        public bool IsRightEdgeInContactWith(EntityNode entityNode)
        {
            Rectangle aabb = entityNode.Hitbox.GetAABB();
            return aabb.Right >= _boundary.Right;
        }

        public bool IsVerticalEdgeInContactWith(EntityNode entityNode)
        {
            Rectangle aabb = entityNode.Hitbox.GetAABB();
            return IsLeftEdgeInContactWith(entityNode) 
                || IsRightEdgeInContactWith(entityNode);
        }

        public bool IsEdgeInContactWith(EntityNode entityNode)
        {
            return IsHorizontalEdgeInContactWith(entityNode) 
                || IsVerticalEdgeInContactWith(entityNode);
        }
        #endregion


        #region Helper Methods
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
            if (!HorizontallyContains(aabb))
            {
                RespondToHorizontalCollision(entityNode);
            }
            if (!VerticallyContains(aabb))
            {
                RespondToVerticalCollision(entityNode);
            }
        }

        private void RespondToHorizontalCollision(EntityNode entityNode)
        {
            float clampedX = Math.Clamp(
                entityNode.Position.X,
                _boundary.Left + entityNode.Hitbox.Radius,
                _boundary.Right - entityNode.Hitbox.Radius
            );
            entityNode.Position = new Vector2(
                clampedX,
                entityNode.Position.Y
            );

            float restitution = PhysicsProperties.GetCombinedRestitution(
                MaterialType,
                entityNode.MaterialType
            );
            entityNode.Velocity = new Vector2(
                -restitution * entityNode.Velocity.X,
                entityNode.Velocity.Y
            );
        }

        private void RespondToVerticalCollision(EntityNode entityNode)
        {
            float clampedY = Math.Clamp(
                entityNode.Position.Y,
                _boundary.Top + entityNode.Hitbox.Radius,
                _boundary.Bottom - entityNode.Hitbox.Radius
            );
            entityNode.Position = new Vector2( 
                entityNode.Position.X,
                clampedY
            );

            float restitution = PhysicsProperties.GetCombinedRestitution(
                MaterialType,
                entityNode.MaterialType
            );
            entityNode.Velocity = new Vector2(
                entityNode.Velocity.X,
                -restitution * entityNode.Velocity.Y
            );
        }

        private bool HorizontallyContains(Rectangle aabb)
        {
            return (
                aabb.Left > _boundary.Left &&
                aabb.Right < _boundary.Right
            );
        }

        private bool VerticallyContains(Rectangle aabb)
        {
            return (
                aabb.Top > _boundary.Top &&
                aabb.Bottom < _boundary.Bottom
            );
        }
        #endregion

    }
}
