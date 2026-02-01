using MonoGame.Extended.ECS;
using MonoGame.Extended.ECS.Systems;
using Halves_of_Tria.Components;
using Microsoft.Xna.Framework;
using Halves_of_Tria.Input;

namespace Halves_of_Tria.Systems
{
    internal class PlayerSystem : EntityProcessingSystem
    {
        private ComponentMapper<Player> _playerMapper;

        public PlayerSystem() 
            : base(Aspect.All(typeof(Player))) { }

        public override void Initialize(IComponentMapperService mapperService)
        {
            _playerMapper = mapperService.GetMapper<Player>();
        }

        public override void Process(GameTime gameTime, int entityId)
        {
            Player player = _playerMapper.Get(entityId);

            if (InputManager.IsActionDown(InputAction.SaltWalkLeft))
            {
                player.Position.X -= player.Speed * (float)gameTime.ElapsedGameTime.TotalSeconds;
            }

            if (InputManager.IsActionDown(InputAction.SaltWalkRight))
            {
                player.Position.X += player.Speed * (float)gameTime.ElapsedGameTime.TotalSeconds;
            }
        }
    }
}
