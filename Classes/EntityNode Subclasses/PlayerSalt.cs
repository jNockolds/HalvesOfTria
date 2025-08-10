using HalvesOfTria;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Diagnostics;

namespace HalvesOfTria.Classes
{
    /// <summary>
    /// [note: currently made from one EntityNode, but should be made from multiple EntityNodes]
    /// </summary>
    public class PlayerSalt : EntityNode
    {
        #region Constructor
        public PlayerSalt(Texture2D texture, Vector2 initialPosition, float layerDepth = 0f)
            : base(texture, (int)texture.Height / 2, initialPosition, PhysicsProperties.PlayerMass, layerDepth, 
                  PhysicsProperties.RestitutionModifier.Low, PhysicsProperties.FrictionModifier.Medium)
        {
            
        }
        #endregion

        #region Update Method
        public override void Update(GameTime gameTime)
        {
            HandlePlayerActions();
            base.Update(gameTime);
        }
        #endregion

        #region Debug Methods
        /// <summary>
        /// Moves player to position and resets Velocity to zero.
        /// </summary>
        /// <remarks>
        /// Called in <see cref="Update"/> if RMB is pressed.
        /// </remarks>
        /// <param name="position"> The Position to set the player to.</param>
        private void DebugMove(Vector2 position)
        {
            Position = position;
            Velocity = Vector2.Zero;
        }
        #endregion

        #region Helper Methods
        private void HandlePlayerActions()
        {
            if (InputManager.IsActionHeld(InputAction.WalkLeft))
            {
                WalkLeft();
            }
            if (InputManager.IsActionHeld(InputAction.WalkRight))
            {
                WalkRight();
            }
            if (InputManager.IsActionJustPressed(InputAction.Jump)
                && CurrentRoom.IsFloorInContactWith(this))
            {
                Jump();
            }
            if (InputManager.IsActionJustPressed(InputAction.Jump)
                && CurrentRoom.IsVerticalEdgeInContactWith(this))
            {
                WallJump();
            }
            if (InputManager.IsActionJustReleased(InputAction.Jump)
                && Velocity.Y < 0
                && !CurrentRoom.IsFloorInContactWith(this))
            {
                CutJump();
            }
#if DEBUG
            if (InputManager.IsActionHeld(InputAction.DebugMove))
            {
                DebugMove(InputManager.MousePosition);
            }
#endif
        }

        private void WalkLeft()
        {
            _physicsObject.ApplyForce(new Vector2(-PhysicsProperties.PlayerWalkingForce, 0));
        }
        private void WalkRight()
        {
            _physicsObject.ApplyForce(new Vector2(PhysicsProperties.PlayerWalkingForce, 0));
            Debug.WriteLine($"Walking right with force: {PhysicsProperties.PlayerWalkingForce}");
        }
        private void Jump()
        {
            _physicsObject.ApplyImpulse(new Vector2(0, -PhysicsProperties.PlayerJumpForce));
        }
        private void WallJump()
        {
            // Implement wall jump logic; use ApplyImpulse
        }
        private void CutJump()
        {
            _physicsObject.Velocity = new Vector2(_physicsObject.Velocity.X, _physicsObject.Velocity.Y * PhysicsProperties.PlayerJumpCutFactor);
        }
        #endregion
    }
}
