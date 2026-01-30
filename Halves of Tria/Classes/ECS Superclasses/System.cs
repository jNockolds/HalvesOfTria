using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Halves_of_Tria.Classes.Components;


namespace Halves_of_Tria.Classes
{
    /// <summary>
    /// Provides a base class for managing and updating a collection of components of a specified type.
    /// </summary>
    internal class System<T> where T : Component
    {
        protected static List<T> Components = new List<T>();

        /// <summary>
        /// Registers the specified component with the component collection.
        /// </summary>
        /// <param name="component">The component instance to register. Cannot be null.</param>
        public static void Register(T component)
        {
            Components.Add(component);
        }

        public static void Update(GameTime gameTime)
        {
            foreach (T component in Components)
            {
                component.Update(gameTime);
            }
        }
    }

    class TransformSystem : System<Transform> { }
    class SpriteSystem : System<Sprite>
    {
        public static void Draw(SpriteBatch spriteBatch)
        {
            foreach (Sprite sprite in Components)
            {
                sprite.Draw(spriteBatch);
            }
        }
    }
}