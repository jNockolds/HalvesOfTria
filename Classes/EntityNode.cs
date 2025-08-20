using HalvesOfTria.Interfaces;
using HalvesOfTria.Structs;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;

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
        public readonly int Radius;
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

        public RoomBoundary CurrentRoom => Game1._testRoomBoundary; // [TODO: change to be accurate for any room; updates every time the room changes or just reads the current room from Game1?]
        public readonly PhysicsProperties.RestitutionModifier RestitutionModifier;
        public readonly PhysicsProperties.FrictionModifier FrictionModifier;
        #endregion


        #region Fields
        private Vector2 _previousResultantForce = Vector2.Zero;
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
        public EntityNode(Texture2D texture, int radius, Vector2 initialPosition, float mass, float layerDepth = 0f, 
            PhysicsProperties.RestitutionModifier restitutionModifier = PhysicsProperties.RestitutionModifier.Low, PhysicsProperties.FrictionModifier frictionModifier = default)
        {
            //if (texture.Width != texture.Height)
            //{
            //    throw new ArgumentException("Texture must be square for this EntityNode constructor.");
            //}

            if (radius < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(radius), "Radius cannot be negative.");
            }
            _physicsObject = new PhysicsObject(initialPosition, mass);
            _sprite = new Sprite(texture, initialPosition, layerDepth);
            Radius = radius;
            RestitutionModifier = restitutionModifier;
            FrictionModifier = frictionModifier;
        }
        #endregion


        #region Update Method
        /// <summary>
        /// Updates the physics object by applying forces, integrating motion, and stopping it if it's sufficiently slow, 
        /// and syncs the sprite's position with the physics object's position.
        /// </summary>
        /// <param name="gameTime">Provides the elapsed time since the last update, used for time-based calculations.</param>
        /// <remarks>
        /// This method performs the following steps:
        /// <list type="number">
        /// <item><description>Handles friction.</description></item>
        /// <item><description>Calls <see cref="PhysicsObject.Update"/>, which accumulates basic forces acting on the object, 
        /// integrates the object's kinematics, and stops the object if it's almost not moving.</description></item>
        /// <item><description>Calls <see cref="SyncPositions"/> to sync the sprite's position with the physics object's position.</description></item>
        /// <item><description>Resets <see cref="PhysicsObject.ResultantForce"/>.</description></item>
        /// </list>
        /// When adding forces in an Update method, make sure to do so right before calling this method. Otherwise, the forces will be applied in the next frame.
        /// </remarks>
        public virtual void Update(GameTime gameTime)
        {
            HandleFriction(_physicsObject.PreviousResultantForce);
            _physicsObject.ApplyForce(GetDrag()); // [replace with HandleDrag() when implemented]

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
        private void HandleFriction(Vector2 previousResultantForce)
        {
            if (_physicsObject.Velocity.X == 0
             && _physicsObject.Velocity.Y == 0)
            {
                return;
            }

            float frictionCoefficient = PhysicsProperties.GetFriction(FrictionModifier, CurrentRoom.FrictionModifier);
            float frictionMagnitude;

            
            if (CurrentRoom.IsFloorInContactWith(this)
                && previousResultantForce.Y > 0)
            {
                frictionMagnitude = frictionCoefficient * previousResultantForce.Y;
                if (_physicsObject.Velocity.X < 0)
                {
                    Debug.WriteLine("Applying friction");
                    _physicsObject.ApplyForce(new Vector2(frictionMagnitude, 0));
                }
                else if (_physicsObject.Velocity.X > 0)
                {
                    Debug.WriteLine("Applying friction");
                    _physicsObject.ApplyForce(new Vector2(-frictionMagnitude, 0));
                }
            }

            if (CurrentRoom.IsCeilingInContactWith(this)
                && previousResultantForce.Y < 0)
            {
                frictionMagnitude = - frictionCoefficient * previousResultantForce.Y;
                if (_physicsObject.Velocity.X < 0)
                {
                    Debug.WriteLine("Applying friction");
                    _physicsObject.ApplyForce(new Vector2(frictionMagnitude, 0));
                }
                else if (_physicsObject.Velocity.X > 0)
                {
                    Debug.WriteLine("Applying friction");
                    _physicsObject.ApplyForce(new Vector2(-frictionMagnitude, 0));
                }
            }

            if (CurrentRoom.IsLeftEdgeInContactWith(this)
                && previousResultantForce.X < 0)
            {
                frictionMagnitude = - frictionCoefficient * previousResultantForce.X;
                if (_physicsObject.Velocity.Y < 0)
                {
                    Debug.WriteLine("Applying friction");
                    _physicsObject.ApplyForce(new Vector2(0, frictionMagnitude));
                }
                else if (_physicsObject.Velocity.Y > 0)
                {
                    Debug.WriteLine("Applying friction");
                    _physicsObject.ApplyForce(new Vector2(0, -frictionMagnitude));
                }
            }

            if (CurrentRoom.IsRightEdgeInContactWith(this)
                && previousResultantForce.X > 0)
            {
                frictionMagnitude = frictionCoefficient * previousResultantForce.X;
                if (_physicsObject.Velocity.Y < 0)
                {
                    Debug.WriteLine("Applying friction");
                    _physicsObject.ApplyForce(new Vector2(0, frictionMagnitude));
                }
                else if (_physicsObject.Velocity.Y > 0)
                {
                    Debug.WriteLine("Applying friction");
                    _physicsObject.ApplyForce(new Vector2(0, -frictionMagnitude));
                }
            }
        }

        private Vector2 GetDrag()
        {
            float horizontalDrag = -PhysicsProperties.DragCoefficient * Velocity.X * Math.Abs(Velocity.X);
            float verticalDrag = -PhysicsProperties.DragCoefficient * Velocity.Y * Math.Abs(Velocity.Y);

            Debug.WriteLine($"Horizontal Drag: {horizontalDrag}, Vertical Drag: {verticalDrag}");
            return new Vector2(horizontalDrag, verticalDrag);
        }

        private void SyncPositions()
        {
            _sprite.Position = _physicsObject.Position;
        }
        #endregion
    }
}
