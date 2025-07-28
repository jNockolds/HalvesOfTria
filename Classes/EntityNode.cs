using HalvesOfTria.Interfaces;
using HalvesOfTria.Structs;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace HalvesOfTria.Classes
{
    public class EntityNode : IUpdateableObject, IDrawableObject
    {
        #region Components
        protected PhysicsObject _physicsObject;
        protected Sprite _sprite;
        #endregion

        #region Properties
        public Circle Hitbox => new Circle(_physicsObject.Position, (int)_sprite.Size.X / 2);
        public Vector2 Position
        {
            get => _physicsObject.Position;
            set
            {
                _physicsObject.Position = value;
                SyncPositions();
            }
        }
        public Vector2 Velocity
        {
            get => _physicsObject.Velocity;
            set
            {
                _physicsObject.Velocity = value;
            }
        }
        #endregion

        #region Constructor
        /// <summary>
        /// Creates a new EntityNode with the specified texture, initial position, mass, and optional hitbox properties.
        /// </summary>
        /// <remarks>
        /// If radius is not specified, it defaults to half the width of the texture.
        /// If hitboxColour is not specified, it defaults to white.
        /// </remarks>
        /// <exception cref="ArgumentException">Thrown if texture isn't square.</exception>
        /// <exception cref="ArgumentOutOfRangeException">Thrown if radius is less than zero.</exception>
        public EntityNode(Texture2D texture, int radius, Vector2 initialPosition, float mass, float layerDepth = 0f)
        {
            if (texture.Width != texture.Height)
            {
                throw new ArgumentException("Texture must be square for this EntityNode constructor.");
            }

            if (radius < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(radius), "Radius cannot be negative.");
            }
            _physicsObject = new PhysicsObject(initialPosition, mass);
            _sprite = new Sprite(texture, initialPosition, layerDepth);
        }
        #endregion

        #region Update Method
        public virtual void Update(GameTime gameTime)
        {
            _physicsObject.Update(gameTime);
            SyncPositions();
        }
        #endregion

        #region Draw Method
        public virtual void Draw(SpriteBatch spriteBatch)
        {
            _sprite.Draw(spriteBatch);
        }
        #endregion

        #region Helper Methods
        private void SyncPositions()
        {
            _sprite.Position = _physicsObject.Position;
        }
        #endregion
    }
}
