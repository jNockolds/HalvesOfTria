using System.Collections.Generic;

namespace Halves_of_Tria.Classes
{
    /// <summary>
    /// Represents an object that can contain a collection of <see cref="Component"/>s.
    /// </summary>
    internal class Entity
    {
        List<Component> Components = new List<Component>();

        /// <summary>
        /// Adds the specified component to the entity and establishes the association between them.
        /// </summary>
        /// <remarks>After calling this method, the component's Entity property is set to this entity. A
        /// component can only be associated with one entity at a time.</remarks>
        /// <param name="component">The component to add to the entity. Cannot be null.</param>
        public void AddComponent(Component component)
        {
            Components.Add(component);
            component.Entity = this;
        }

        /// <summary>
        /// Retrieves the first component of the specified type from the collection.
        /// </summary>
        /// <remarks>If multiple components of type T exist in the collection, only the first one
        /// encountered is returned. The search is performed in the order the components appear in the
        /// collection.</remarks>
        /// <typeparam name="T">The type of component to retrieve. Must derive from Component.</typeparam>
        /// <returns>The first component of type T if found; otherwise, null.</returns>
        public T GetComponent<T>() where T : Component
        {
            foreach (Component component in Components)
            {
                if (component is T)
                {
                    return (T)component;
                }
            }
            return null;
        }
    }
}
