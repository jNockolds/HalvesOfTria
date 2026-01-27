using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Halves_of_Tria.Classes.Components
{
    class Sprite : Component
    {
        #region Fields
        private Texture2D Texture;
        #endregion


        #region Constructor
        /// <summary>
        /// Creates a new Sprite with the specified texture, initial position, and layer depth.
        /// <para>
        /// Position is at its centre.
        /// </para>
        /// <para>
        /// Layer depth is used to determine the rendering order of sprites. Lower values are rendered above higher ones.
        /// </para>
        /// </summary>
        public Sprite(Texture2D texture)
        {
            SpriteSystem.Register(this);

            Texture = texture;
        }
        #endregion


        #region Methods
        public void Draw(SpriteBatch spriteBatch)
        {
            Transform transform = Entity.GetComponent<Transform>();
            Vector2 spriteCentre = new Vector2(Texture.Width / 2, Texture.Height / 2);

            spriteBatch.Draw(
                Texture,
                transform.Position,
                null,
                Color.White,
                transform.Rotation,
                spriteCentre,
                transform.Scale,
                SpriteEffects.None,
                transform.LayerDepth
            );
        }
        #endregion
    }
}
