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
        #region Fields and Components
        private ComponentMapper<Transform2> _transformMapper;
        private ComponentMapper<DynamicBody> _dynamicBodyMapper;

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
            : base(Aspect.All(typeof(Transform2), typeof(DynamicBody)))
        {
            _graphicsDevice = graphicsDevice;
            _spriteBatch = spriteBatch;
        }

        #region Game Loop Methods
        public override void Initialize(IComponentMapperService mapperService)
        {
            _transformMapper = mapperService.GetMapper<Transform2>();
            _dynamicBodyMapper = mapperService.GetMapper<DynamicBody>();

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

            foreach (var entityId in ActiveEntities)
            {
                Transform2 transform = _transformMapper.Get(entityId);
                DynamicBody dynamicBody = _dynamicBodyMapper.Get(entityId);

                switch (_vectorsShown)
                {
                    case VectorType.Forces:
                        foreach (Force force in dynamicBody.Forces)
                        {
                            DrawArrow(transform.Position, force.Value, _forcesColour, _forceScaleFactor);
                        }
                        break;
                    case VectorType.ResultantForce:
                        DrawArrow(transform.Position, dynamicBody.ResultantForce, _resultantForceColour, _forceScaleFactor);
                        break;
                    case VectorType.Acceleration:
                        float accelerationScaleFactor = _forceScaleFactor * dynamicBody.InverseMass;
                        DrawArrow(transform.Position, dynamicBody.Acceleration, _accelerationColour, accelerationScaleFactor);
                        break;
                    case VectorType.Velocity:
                        DrawArrow(transform.Position, dynamicBody.Velocity, _velocityColour, _velocityScaleFactor);
                        break;
                }
            }

            _spriteBatch.End();
        }
        #endregion

        #region Helper Methods
        private void CycleVectorsShown()
        {
            _vectorsShown++;
            if ((int)_vectorsShown >= 5)
            {
                _vectorsShown = VectorType.None;
            }
        }

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

        // This system is responsible for rendering visual arrows representing vectors for each DynamicBody.
        // Such vectors include forces, velocities, and accelerations. This is purely for debugging purposes to help visualize the physics interactions in the game.
        // Note that one object may have multiple force vectors acting on it, all displayed, with a resultant force arrow as well. (This will not be necessary for acceleration or velocity.)
        // Colour will vary based on the type of the scaledArrowVector.
        // MAgnitude will be represented by the arrowLength of the arrow, but also by a numerical label.

        // Implementation details:
        // - This system should run after the DynamicBodySystem, so that it can access the updated physics data for each DynamicBody.
        // - Required components:
        //     - Transform2
        //     - DynamicBody
        // - Initialize():
        //     - Generate a 1 pixel white stock texture that can be resized procedurally to create a dynamic arrow for all vectors, scaling and rotating it appropriately for each scaledArrowVector's magnitude and direction.
        //         - It can be resized, rotated, and recoloured as part of SpriteBatch.Draw().
        //         - To make an arrow, just stretch the pixel to its arrowLength, and add two short stretched pixels (i.e. rectangles) on the end (45 or 30 degrees from the shaft? Although remember MonoGame works in radians (I think)).
        // - Draw():
        //     - [Note: Add most of the below logic to a DrawArrowShaft() function]
        //     - [Note: needs to be toggelable, via a keypress or something (maybe it cycles between On -> Forces -> Accelerations -> Velocities -> On -> ...).]
        //         - Look into whether it's possible to toggle the system on and off without having to add/remove it from the world, as that would be more efficient than having to add/remove it every time the user wants to toggle it.
        //     - For each DynamicBody, retrieve its velocity, acceleration, and forces acting upon it, calculating the resultant force accordingly.
        //     - For each scaledArrowVector, calculate the appropriate arrowLength and direction for the arrow based on its magnitude and direction.
        //         - Have some cap on the size of arrows (depending on the largest arrow) and resize all other arrows to be less than that, keeping their relative sizes.
        //     - Render the arrow at the position of the DynamicBody.
        //     - Arrow Colours:
        //         - Forces: Red
        //         - Acceleration: Green
        //         - Velocity: Blue

        // - Think about how to handle multiple forces acting in the same direction on the same object.
        // - Maybe each arrow can be offset slightly from the centre of the object, so they don't completely overlap and become indistinguishable.
        //     - Todo: figure out how to dynamically handle this with any number of forces, and with forces at any rotation.
        // - The resultant force arrow can be rendered directly on top of the object, as it's the most important one to visualize.
    }
}