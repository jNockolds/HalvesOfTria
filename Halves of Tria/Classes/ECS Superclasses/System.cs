using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace Halves_of_Tria.Classes
{
    class System<T> where T : Component
    {
        protected static List<T> Components = new List<T>();

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

    class TransformSystem : System<Components.Transform> { }
    class SpriteSystem : System<Components.Sprite> { }
}