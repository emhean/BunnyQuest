using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace BunnyQuest.ECS.Components
{
    class CmpCollider : Component
    {
        public Rectangle rect;

        Texture2D tex_px;
        public bool Debug { get; set; } = true;

        public CmpCollider(Entity owner) : base(owner)
        {
        }

        public CmpCollider(Entity owner, Texture2D debug_texture) : base(owner)
        {
            this.tex_px = debug_texture;
        }

        public override void Update(float delta)
        {
            rect.X = (int)this.entity.pos.X;
            rect.Y = (int)this.entity.pos.Y;
            rect.Width = (int)this.entity.size.X;
            rect.Height = (int)this.entity.size.Y;

            base.Update(delta);
        }

        public override void Render(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(tex_px, rect, Color.Red * 0.33f);
        }

        public void SetPosition(int x, int y)
        {
            if (x < 0)
                x = 0;
            if (y < 0)
                y = 0;

            entity.pos.X = x;
            entity.pos.Y = y;

            rect.X = x;
            rect.Y = y;
        }
    }
}
