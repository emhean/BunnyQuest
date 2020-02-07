namespace BunnyQuest.ECS
{
    /// <summary>
    /// A wrapper for Entity events.
    /// </summary>
    class EntityArgs
    {
        public EntityArgs(Entity entity, Component component)
        {
            this.Entity = entity;
            this.Component = component;
        }

        /// <summary>
        /// The entity that invoked the event from one of its components.
        /// </summary>
        public Entity Entity { get; }

        /// <summary>
        /// The component that invoked the event.
        /// </summary>
        public Component Component { get; }
    }
}
