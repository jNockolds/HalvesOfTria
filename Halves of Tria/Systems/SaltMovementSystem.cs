using MonoGame.Extended.ECS;
using MonoGame.Extended.ECS.Systems;
using Halves_of_Tria.Components;
using Microsoft.Xna.Framework;
using Halves_of_Tria.Input;
using MonoGame.Extended;
using System.Diagnostics;

namespace Halves_of_Tria.Systems
{
    internal class SaltMovementSystem : EntityProcessingSystem
    {
        #region Fields and Components
        private ComponentMapper<Transform2> _transformMapper;
        private ComponentMapper<Speed> _speedMapper;
        #endregion

        public SaltMovementSystem() 
            : base(Aspect.All(typeof(Tags.Players.Salt), typeof(Transform2), typeof(Speed))) { }

        #region Game Loop Methods
        public override void Initialize(IComponentMapperService mapperService)
        {
            _transformMapper = mapperService.GetMapper<Transform2>();
            _speedMapper = mapperService.GetMapper<Speed>();
        }

        public override void Process(GameTime gameTime, int entityId)
        {
            Transform2 transform = _transformMapper.Get(entityId);
            Speed speed = _speedMapper.Get(entityId);

            DebugMoveOnInput(transform);
            MoveOnInput(gameTime, transform, speed);
        }
        #endregion

        #region Helper Methods
        private void DebugMoveOnInput(Transform2 transform)
        {
            Debug.WriteLine("Attempting Debug Mpve");
            if (InputHandler.IsActionDown(InputAction.SaltDebugMove))
            {
                Vector2 mousePosition = InputHandler.GetMousePosition();
                transform.Position = mousePosition;
            }
        }
        private void MoveOnInput(GameTime gameTime, Transform2 transform, Speed speed)
        {

            if (InputHandler.IsActionDown(InputAction.SaltWalkLeft))
            {
                transform.Position -= (float)gameTime.ElapsedGameTime.TotalSeconds * new Vector2(speed.Value, 0);
            }

            if (InputHandler.IsActionDown(InputAction.SaltWalkRight))
            {
                transform.Position += (float)gameTime.ElapsedGameTime.TotalSeconds * new Vector2(speed.Value, 0);
            }
        }
        #endregion
    }
}
