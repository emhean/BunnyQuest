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
        /// Gets component of T type.
        /// </summary>
        public T GetComponent<T>() where T : Component
        {
            for(int i = 0; i < components.Count; ++i)
            {
                if(components[i] is T)
                {
                    return (T)components[i];
                }
            }

            return null;
        }


        /// <summary>
        /// Returns true if Entity has component of T Type, returns false if not.
        /// </summary>
        public bool HasComponent<T>() where T : Component => (GetComponent<T>() != null);

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
