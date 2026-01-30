using Microsoft.Xna.Framework;

namespace Halves_of_Tria.Classes
{
    /// <summary>
    /// Represents a modular unit of behavior that can be attached to an <see cref="Halves_of_Tria.Classes.Entity"/>.
    /// </summary>
    internal class Component
    {
        public Entity Entity;

        public virtual void Update(GameTime gameTime) { }
    }
}
