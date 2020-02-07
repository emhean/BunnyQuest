using Microsoft.Xna.Framework.Graphics;
using System;

namespace BunnyQuest.ECS
{
    class Component
    {
        /// <summary>
        /// The entity that holds this component.
        /// </summary>
        public Entity parent;

        /// <summary>
        /// Creates a new Component instance and sets the entity field to the parameter.
        /// </summary>
        public Component(Entity owner)
        {
            this.parent = owner;
        }

        public bool IsUpdated = true;
        public bool IsRendered = true;


        /// <summary>
        /// Updates the component.
        /// </summary>
        public virtual void Update(float delta) { }

        /// <summary>
        /// Renders the component.
        /// </summary>
        public virtual void Render(SpriteBatch spriteBatch) { }

        public event EventHandler<EntityArgs> ComponentExpired;

        bool expired;
        public bool Expired
        {
            get => expired;
            set
            {
                expired = value;
                if (expired)
                    OnExpired();
            }
        }

        private void OnExpired()
        {
            ComponentExpired?.Invoke(this, new EntityArgs(parent, this));
        }
    }
}
