using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using System;
using System.Collections.Generic;

namespace BunnyQuest.ECS
{
    class Entity
    {
        public readonly uint UUID;
        public List<Component> components;
        public Vector2 pos;
        public Vector2 size;

        public Vector2 GetCenterPosition() => pos + (size / 2);

        public Entity(uint UUID, ContentManager contentManager) : this(UUID)
        {
        }

        public Entity(uint UUID)
        {
            this.UUID = UUID;
            this.components = new List<Component>();
        }


        bool expired;
        /// <summary>
        /// If true, System will place this entity instance into the expired list and remove it from the entity list.
        /// </summary>
        public bool Expired
        {
            get => expired;
            set
            {
                // This means that OnExpired() is invoked only when before its set to true to avoid multiple expired event calls.
                if (expired == false && value == true)
                {
                    expired = true;
                    OnExpired();
                }
                else expired = value;
            }
        }
        private void OnExpired()
        {
            EntityExpired?.Invoke(this, new EntityArgs(this, null));
        }

        public event EventHandler<EntityArgs> EntityExpired;
        public event EventHandler<EntityArgs> ComponentAdded, ComponentRemoved;

        protected void OnComponentAdded(EntityArgs args)
        {
            ComponentAdded?.Invoke(this, args);
        }
        protected void OnComponentRemoved(EntityArgs args)
        {
            ComponentRemoved?.Invoke(this, args);
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
            OnComponentAdded(new EntityArgs(this, component));
        }

        /// <summary>
        /// Remove component by reference.
        /// </summary>
        public void RemoveComponent(Component component)
        {
            this.components.Remove(component);
            OnComponentRemoved(new EntityArgs(this, component));
        }
    }
}
