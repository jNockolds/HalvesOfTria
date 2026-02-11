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
        private ComponentMapper<Transform2> _transformMapper;
        private ComponentMapper<Speed> _speedMapper;

        public SaltMovementSystem() 
            : base(Aspect.All(typeof(Transform2)).All(typeof(Speed))) { }

        public override void Initialize(IComponentMapperService mapperService)
        {
            _transformMapper = mapperService.GetMapper<Transform2>();
            _speedMapper = mapperService.GetMapper<Speed>();
        }

        public override void Process(GameTime gameTime, int entityId)
        {

            Transform2 transform = _transformMapper.Get(entityId);
            Speed speed = _speedMapper.Get(entityId);

            if (InputManager.IsActionDown(InputAction.SaltWalkLeft))
            {
                transform.Position -= (float)gameTime.ElapsedGameTime.TotalSeconds * new Vector2(speed.Value, 0);
            }

            if (InputManager.IsActionDown(InputAction.SaltWalkRight))
            {
                transform.Position += (float)gameTime.ElapsedGameTime.TotalSeconds * new Vector2(speed.Value, 0);
            }


        }
    }
}
