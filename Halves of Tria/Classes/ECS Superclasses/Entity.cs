using System.Collections.Generic;

namespace Halves_of_Tria.Classes
{
    class Entity
    {
        List<Component> Components = new List<Component>();

        public void AddComponent(Component component)
        {
            Components.Add(component);
            component.Entity = this;
        }

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
