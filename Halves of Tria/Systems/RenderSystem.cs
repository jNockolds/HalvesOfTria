using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using MonoGame.Extended.ECS;
using MonoGame.Extended.ECS.Systems;

namespace Halves_of_Tria.Systems
{
    public class RenderSystem : EntityDrawSystem
    {
        private readonly SpriteBatch _spriteBatch;
        private ComponentMapper<Texture2D> _textureMapper;
        private ComponentMapper<Transform2> _transformMapper;

        public RenderSystem(SpriteBatch spriteBatch)
            : base(Aspect.All(typeof(Texture2D), typeof(Transform2)))
        {
            _spriteBatch = spriteBatch;
        }

        public override void Initialize(IComponentMapperService mapperService)
        {
            _textureMapper = mapperService.GetMapper<Texture2D>();
            _transformMapper = mapperService.GetMapper<Transform2>();
        }

        public override void Draw(GameTime gameTime)
        {
            _spriteBatch.Begin();

            foreach (int entityId in ActiveEntities)
            {
                Texture2D texture = _textureMapper.Get(entityId);
                Transform2 transform = _transformMapper.Get(entityId);
                _spriteBatch.Draw(texture, transform.Position, Color.White);
            }

            _spriteBatch.End();
        }








    }
}
