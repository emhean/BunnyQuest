using Microsoft.Xna.Framework;

namespace BunnyQuest.ECS.Components
{
    class CmpCollider : Component
    {
        public Rectangle rect;

        public CmpCollider(Entity owner) : base(owner)
        {
        }

        public override void Update(float delta)
        {
            rect.X = (int)this.entity.pos.X;
            rect.Y = (int)this.entity.pos.Y;
            rect.Width = (int)this.entity.size.X;
            rect.Height = (int)this.entity.size.Y;

            base.Update(delta);
        }
    }
}
