using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace BunnyQuest.ECS
{
    class Entity
    {
        public readonly int UUID;
        public List<Component> components;
        public Vector2 pos;
        public Vector2 size;

        public Entity(int UUID)
        {
            this.UUID = UUID;
            this.components = new List<Component>();
        }

        /// <summary>
        /// Gets component of type.
        /// </summary>
        public Component GetComponent<T>()
        {
            for(int i = 0; i < components.Count; ++i)
            {
                if(components[i] is T)
                {
                    return components[i];
                }
            }

            return null;
        }

        /// <summary>
        /// Add component.
        /// </summary>
        public void AddComponent(Component component)
        {
            this.components.Add(component);
        }

        /// <summary>
        /// Remove component by reference.
        /// </summary>
        public void RemoveComponent(Component component)
        {
            this.components.Remove(component);
        }
    }
}
