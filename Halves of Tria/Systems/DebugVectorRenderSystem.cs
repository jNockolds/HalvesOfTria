using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Halves_of_Tria.Components;
using Halves_of_Tria.Input;
using Halves_of_Tria.Textures;
using MonoGame.Extended;
using MonoGame.Extended.ECS;
using MonoGame.Extended.ECS.Systems;
using System.Diagnostics;

namespace Halves_of_Tria.Systems
{
    public enum VectorType
    {
        None,
        Forces,
        ResultantForce,
        Acceleration,
        Velocity
    }
    internal class DebugVectorRenderSystem : EntityDrawSystem
    {
        // [TODO]:
        //     - After a debug menu is implemented, add force names (using ForceType) on each force arrow, and values on every arrow
        //     - Have some cap on the size of arrows (depending on the largest arrow) and resize all other arrows to be less than that, keeping their relative sizes.
        //     - Think about how to handle multiple forces acting in the same direction on the same object.
        //         - Maybe each arrow can be offset slightly from the centre of the object, so they don't completely overlap and become indistinguishable.
        //         - Todo: figure out how to dynamically handle this with any number of forces, and with forces at any rotation.

        #region Fields and Components
        private ComponentMapper<Transform> _transformMapper;
        private ComponentMapper<PhysicsBody> _physicsBodyMapper;

        private GraphicsDevice _graphicsDevice;
        private SpriteBatch _spriteBatch;
        private Texture2D _pixel;

        private VectorType _vectorsShown;


        // Arrow details fields:

        private Color _forcesColour = Color.Orange;
        private Color _resultantForceColour = Color.Red;
        private Color _accelerationColour = Color.Lime;
        private Color _velocityColour = Color.Blue;

        private float _forceScaleFactor = 10f;
        private float _velocityScaleFactor = 1f;

        private float _arrowThickness = 1f;
        private float _arrowheadLength = 5f;
        private float _arrowheadAngleOffset = (float)((5f/6f) * Math.PI); // 150 (or 30) degrees from the arrow's direction
        #endregion

        public DebugVectorRenderSystem(GraphicsDevice graphicsDevice, SpriteBatch spriteBatch)
            : base(Aspect.All(typeof(Transform), typeof(PhysicsBody)))
        {
            _graphicsDevice = graphicsDevice;
            _spriteBatch = spriteBatch;
        }

        #region Game Loop Methods
        public override void Initialize(IComponentMapperService mapperService)
        {
            _transformMapper = mapperService.GetMapper<Transform>();
            _physicsBodyMapper = mapperService.GetMapper<PhysicsBody>();

            _vectorsShown = 0;
            _pixel = TextureGenerator.Pixel(_graphicsDevice, Color.White);
        }

        public override void Draw(GameTime gameTime)
        {
            if (InputHandler.WasActionJustPressed(InputAction.CycleDebugVectors))
            {
                CycleVectorsShown();
            }

            if (_vectorsShown == VectorType.None)
            {
                return;
            }

            _spriteBatch.Begin();

            foreach (int entityId in ActiveEntities)
            {
                Transform transform = _transformMapper.Get(entityId);
                PhysicsBody physicsBody = _physicsBodyMapper.Get(entityId);

                switch (_vectorsShown)
                {
                    case VectorType.Forces:
                        foreach (Vector2 value in physicsBody.Forces.Values)
                        {
                            DrawArrow(transform.Position, value, _forcesColour, _forceScaleFactor);
                        }
                        break;
                    case VectorType.ResultantForce:
                        DrawArrow(transform.Position, physicsBody.ResultantForce, _resultantForceColour, _forceScaleFactor);
                        break;
                    case VectorType.Velocity:
                        DrawArrow(transform.Position, physicsBody.Velocity, _velocityColour, _velocityScaleFactor);
                        break;
                }
            }

            _spriteBatch.End();
        }
        #endregion

        #region Helper Methods
        /// <summary>
        /// Cycles the current vector display mode to the next available option, resetting to none after the last mode.
        /// </summary>
        private void CycleVectorsShown()
        {
            _vectorsShown++;
            if ((int)_vectorsShown >= 5)
            {
                _vectorsShown = VectorType.None;
            }
        }

        /// <summary>
        /// Draws an arrow from the specified origin in the direction and magnitude defined by the given vector, using
        /// the specified color and scale factor.
        /// </summary>
        /// <param name="origin">The starting point of the arrow in world or screen coordinates.</param>
        /// <param name="arrowVector">A vector representing the direction and length of the arrow before scaling. If the vector has zero length,
        /// no arrow is drawn.</param>
        /// <param name="color">The color to use when rendering the arrow.</param>
        /// <param name="scaleFactor">A multiplier applied to the length of the arrow. Must be a positive value to produce a visible arrow.</param>
        private void DrawArrow(Vector2 origin, Vector2 arrowVector, Color color, float scaleFactor)
        {
            if (arrowVector.Length() > 0)
            {
                Vector2 scaledVector = scaleFactor * arrowVector;
                float arrowLength = scaledVector.Length();
                float shaftAngle = (float)Math.Atan2(arrowVector.Y, arrowVector.X);

                DrawArrowShaft(origin, arrowLength, shaftAngle, color);
                DrawArrowhead(scaledVector, origin, arrowLength, shaftAngle, color);
            }
        }

        private void DrawArrowShaft(Vector2 origin, float length, float orientation, Color color)
        {
            _spriteBatch.Draw(
                _pixel,
                origin,
                null,
                color,
                orientation,
                Vector2.Zero,
                new Vector2(length, _arrowThickness),
                SpriteEffects.None,
                0f
            );
        }

        private void DrawArrowhead(Vector2 scaledArrowVector, Vector2 origin, float arrowLength, float orientation, Color color)
        {
            Vector2 arrowEndpoint = origin + scaledArrowVector;

            float angle1 = orientation + _arrowheadAngleOffset;
            float angle2 = orientation - _arrowheadAngleOffset;

            _spriteBatch.Draw(
                _pixel,
                arrowEndpoint,
                null,
                color,
                angle1,
                Vector2.Zero,
                new Vector2(_arrowheadLength, _arrowThickness),
                SpriteEffects.None,
                0f
            );

            _spriteBatch.Draw(
                _pixel,
                arrowEndpoint,
                null,
                color,
                angle2,
                Vector2.Zero,
                new Vector2(_arrowheadLength, _arrowThickness),
                SpriteEffects.None,
                0f
            );
        }
        #endregion
    }
}