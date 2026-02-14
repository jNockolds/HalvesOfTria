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
using System.Collections.Generic;
using MonoGame.Extended.Collections;



namespace Halves_of_Tria.Systems
{
    internal class DebugVectorRenderSystem : EntityDrawSystem
    {
        #region Fields and Components
        private ComponentMapper<Transform2> _transformMapper;
        private ComponentMapper<DynamicBody> _dynamicBodyMapper;

        private bool _showForces;
        private GraphicsDevice _graphicsDevice;
        private SpriteBatch _spriteBatch;
        private Texture2D _pixel;

        private float _arrowThickness = 5f;
        private float _scaleFactor = 1f;
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

            _showForces = false;
            _pixel = TextureGenerator.Pixel(_graphicsDevice, Color.White);
        }

        public override void Draw(GameTime gameTime)
        {
            if (InputHandler.WasActionJustPressed(InputAction.ShowDebugInfo))
            {
                _showForces = !_showForces;
            }

            if (!_showForces)
            {
                return;
            }

            _spriteBatch.Begin();

            foreach (var entityId in ActiveEntities)
            {
                Transform2 transform = _transformMapper.Get(entityId);
                DynamicBody dynamicBody = _dynamicBodyMapper.Get(entityId);

                Vector2 arrowOrigin = transform.Position;
                Vector2 velocity = dynamicBody.Velocity;
                Vector2 acceleration = dynamicBody.Acceleration;
                Vector2 resultantForce = dynamicBody.ResultantForce;

                // Draw velocity arrow (Blue)
                if (velocity.Length() > 0)
                {
                    float angle = (float)Math.Atan2(velocity.Y, velocity.X);
                    Debug.WriteLine($"Velocity: {velocity}, Angle: {angle}");
                    DrawArrow(arrowOrigin, _scaleFactor * velocity.Length(), angle, Color.Blue);
                }
            }

            _spriteBatch.End();
        }
        #endregion

        #region Helper Methods
        private void DrawArrow(Vector2 origin, float length, float orientation, Color color)
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
        #endregion

        // This system is responsible for rendering visual arrows representing vectors for each DynamicBody.
        // Such vectors include forces, velocities, and accelerations. This is purely for debugging purposes to help visualize the physics interactions in the game.
        // Note that one object may have multiple force vectors acting on it, all displayed, with a resultant force arrow as well. (This will not be necessary for acceleration or velocity.)
        // Colour will vary based on the type of the vector.
        // MAgnitude will be represented by the length of the arrow, but also by a numerical label.

        // Implementation details:
        // - This system should run after the DynamicBodySystem, so that it can access the updated physics data for each DynamicBody.
        // - Required components:
        //     - Transform2
        //     - DynamicBody
        // - Initialize():
        //     - Generate a 1 pixel white stock texture that can be resized procedurally to create a dynamic arrow for all vectors, scaling and rotating it appropriately for each vector's magnitude and direction.
        //         - It can be resized, rotated, and recoloured as part of SpriteBatch.Draw().
        //         - To make an arrow, just stretch the pixel to its length, and add two short stretched pixels (i.e. rectangles) on the end (45 or 30 degrees from the shaft? Although remember MonoGame works in radians (I think)).
        // - Draw():
        //     - [Note: Add most of the below logic to a DrawArrow() function]
        //     - [Note: needs to be toggelable, via a keypress or something (maybe it cycles between On -> Forces -> Accelerations -> Velocities -> On -> ...).]
        //         - Look into whether it's possible to toggle the system on and off without having to add/remove it from the world, as that would be more efficient than having to add/remove it every time the user wants to toggle it.
        //     - For each DynamicBody, retrieve its velocity, acceleration, and forces acting upon it, calculating the resultant force accordingly.
        //     - For each vector, calculate the appropriate length and direction for the arrow based on its magnitude and direction.
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