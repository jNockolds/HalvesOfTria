using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace Halves_of_Tria.Classes
{
    class System<T> where T : Component
    {
        protected static List<T> components = new List<T>();
        
        public static void Register(T component)
        {
            components.Add(component);
        }

        public static void Update(GameTime gameTime)
        {
            foreach (T component in components)
            {
                component.Update(gameTime);
            }
        }
    }
}
