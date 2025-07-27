using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using HalvesOfTria.Interfaces;

namespace HalvesOfTria.Classes
{
    /// <summary>
    /// Represents a Texture at a Position and layer.
    /// </summary>
    public class Sprite : IDrawableObject
    {

        #region Properties
        public Vector2 Position { get; set; }
        #endregion


        #region Fields
        private Texture2D Texture;
        private float LayerDepth;
        #endregion


        #region Constructor
        /// <summary>
        /// Creates a new Sprite with the specified texture, initial position, and layer depth.
        /// <para>
        /// Position is 
        /// </para>
        /// <para>
        /// Layer depth is used to determine the rendering order of sprites. Lower values are rendered above higher ones.
        /// </para>
        /// </summary>
        public Sprite(Texture2D texture, Vector2 initialPosition, float layerDepth = 0f)
        {
            Texture = texture;
            Position = initialPosition;
            LayerDepth = layerDepth;
        }
        #endregion


        #region Public Methods
        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(
                Texture,
                Position,
                null,
                Color.White,
                0f,
                new Vector2(Texture.Width / 2, Texture.Height / 2),
                Vector2.One,
                SpriteEffects.None,
                LayerDepth
            );
        }
        #endregion
    }
}
