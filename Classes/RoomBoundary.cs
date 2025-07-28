using HalvesOfTria.Interfaces;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace HalvesOfTria.Classes
{
    // [NOTE: currently only does elastic collisions, i.e. no energy loss]
    public class RoomBoundary : IStaticCollideable, IUpdateableObject, IDrawableObject
    {
        #region Fields
        private readonly Rectangle _boundary;
        private readonly Sprite _boundaryHitboxSprite;
        #endregion

        #region Constructor
        public RoomBoundary(GraphicsDevice graphicsDevice, int top, int bottom, int left, int right)
        {
            if (top >= bottom || left >= right)
            {
                throw new ArgumentException("Invalid boundary dimensions: top must be less than bottom and left must be less than right.");
            }
            _boundary = new Rectangle(left, top, right - left, bottom - top);
            Texture2D _spriteTexture = TextureMaker.GenerateRectangleTexture(
                graphicsDevice,
                right - left,
                bottom - top,
                Color.White
            );
            Vector2 spriteCentre = new Vector2((right + left) / 2, (bottom + top) / 2);
            _boundaryHitboxSprite = new Sprite(
                _spriteTexture,
                spriteCentre
            );
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

        #region Helper Methods
        public void HandleIncomingCollisions(List<EntityNode> entityNodes)
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
            else if (!VerticallyContains(aabb))
            {
                RespondToVerticalCollision(entityNode);
            }
        }

        private void RespondToHorizontalCollision(EntityNode entityNode)
        {
            System.Diagnostics.Debug.WriteLine($"EntityNode {entityNode} collided with horizontal boundary. BL: {_boundary.Left}, BR: {_boundary.Right}");
            float clampedX = Math.Clamp(
                entityNode.Position.X,
                _boundary.Left + entityNode.Hitbox.Radius,
                _boundary.Right - entityNode.Hitbox.Radius
            );
            entityNode.Position = new Vector2(
                clampedX,
                entityNode.Position.Y
            );

            entityNode.Velocity = new Vector2(
                -entityNode.Velocity.X,
                entityNode.Velocity.Y
            );
        }

        private void RespondToVerticalCollision(EntityNode entityNode)
        {
            System.Diagnostics.Debug.WriteLine($"EntityNode {entityNode} collided with vertical boundary.");
            float clampedY = Math.Clamp(
                entityNode.Position.Y,
                _boundary.Top + entityNode.Hitbox.Radius,
                _boundary.Bottom - entityNode.Hitbox.Radius
            );
            entityNode.Position = new Vector2( 
                entityNode.Position.X,
                clampedY
            );

            entityNode.Velocity = new Vector2(
                entityNode.Velocity.X,
                -entityNode.Velocity.Y
            );
        }

        private bool HorizontallyContains(Rectangle aabb)
        {
            return (
                aabb.Left >= _boundary.Left &&
                aabb.Right <= _boundary.Right
            );
        }

        private bool VerticallyContains(Rectangle aabb)
        {
            return (
                aabb.Top >= _boundary.Top &&
                aabb.Bottom <= _boundary.Bottom
            );
        }
        #endregion

    }
}
