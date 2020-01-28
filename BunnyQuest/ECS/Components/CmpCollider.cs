using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace BunnyQuest.ECS.Components
{
    class CmpCollider : Component
    {
        public Rectangle rect;
        public Rectangle offset;

        public bool GetsPushed = false;

        Texture2D tex_px; // Debug texture
        public bool Debug { get; set; } = true;

        public CmpCollider(Entity owner) : base(owner)
        {
        }

        public CmpCollider(Entity owner, Texture2D debug_texture) : base(owner)
        {
            this.tex_px = debug_texture;
        }

        public void UpdateColliderPosition()
        {
            rect.X = (int)this.entity.pos.X;
            rect.Y = (int)this.entity.pos.Y;
            rect.Width = (int)this.entity.size.X;
            rect.Height = (int)this.entity.size.Y;

            rect.X += offset.X;
            rect.Y += offset.Y;
            rect.Width += offset.Width;
            rect.Height += offset.Height;
        }

        public override void Update(float delta)
        {
            UpdateColliderPosition();

            base.Update(delta);
        }

        public override void Render(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(tex_px, rect, Color.Red * 0.33f);
        }

        /// <summary>
        /// Sets the position of the entity and the collider.
        /// </summary>
        public void SetPosition(int x, int y)
        {
            entity.pos.X = x;
            entity.pos.Y = y;

            UpdateColliderPosition();
        }
        /// <summary>
        /// Sets the position of the entity and the collider.
        /// </summary>
        public void SetPosition(float x, float y)
        {
            entity.pos.X = x;
            entity.pos.Y = y;

            UpdateColliderPosition();
        }
    }
}
