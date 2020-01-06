using Microsoft.Xna.Framework.Graphics;

namespace BunnyQuest.ECS
{
    class Component
    {
        /// <summary>
        /// The entity that holds this component.
        /// </summary>
        public Entity entity;

        /// <summary>
        /// Creates a new Component instance and sets the entity field to the parameter.
        /// </summary>
        public Component(Entity owner)
        {
            this.entity = owner;
        }

        public bool IsUpdated;
        public bool IsRendered;


        /// <summary>
        /// Updates the component.
        /// </summary>
        public void Update(float delta)
        {
        }

        /// <summary>
        /// Renders the component.
        /// </summary>
        public void Render(SpriteBatch spriteBatch)
        {
        }
    }
}
