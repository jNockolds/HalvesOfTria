using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Halves_of_Tria.Classes.Components
{
    /// <summary>
    /// Represents a 2D sprite component that can be rendered with a specified texture and transform.
    /// </summary>
    /// <remarks>A Sprite must be attached to an entity with a Transform component to determine its position,
    /// rotation, scale, and layer depth during rendering. Sprites are drawn in order based on their layer 
    /// depth, with lower values appearing above higher ones.</remarks>
    internal class Sprite : Component
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
        /// <summary>
        /// Submits the sprite to the specified sprite batch, using the entity's current transform.
        /// </summary>
        /// <remarks>The sprite is drawn centered on its texture.
        /// Call this method within a SpriteBatch.Begin() SpriteBatch.End() block.</remarks>
        /// <param name="spriteBatch">The sprite batch used to render the sprite. Cannot be null.</param>
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
