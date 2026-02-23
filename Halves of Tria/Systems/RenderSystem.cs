using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using MonoGame.Extended.ECS;
using MonoGame.Extended.ECS.Systems;

namespace Halves_of_Tria.Systems
{
    public class RenderSystem : EntityDrawSystem
    {
        #region Fields and Components
        private ComponentMapper<Texture2D> _textureMapper;
        private ComponentMapper<Transform2> _transformMapper;

        private readonly SpriteBatch _spriteBatch;
        #endregion

        public RenderSystem(SpriteBatch spriteBatch)
            : base(Aspect.All(typeof(Texture2D), typeof(Transform2)))
        {
            _spriteBatch = spriteBatch;
        }

        #region Game Loop Methods
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
                Vector2 topLeft = transform.Position - 0.5f * new Vector2(texture.Width, texture.Height);
                _spriteBatch.Draw(texture, topLeft, Color.White);
            }

            _spriteBatch.End();
        }
        #endregion
    }
}
