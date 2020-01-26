using BunnyQuest.ECS;
using BunnyQuest.ECS.Components;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace BunnyQuest.Entities
{
    class Player : Entity
    {
        public Vector2 speed = new Vector2(2.5f, 2.5f);
        public Vector2 direction = Vector2.Zero;

        public Player(int UUID, ContentManager content) : base(UUID)
        {
            this.AddComponent(new CmpCollider(this, content.Load<Texture2D>("etc/pixel")) { GetsPushed = true });

            this.AddComponent(new CmpStats(this, 10, 0));

            var sprites = new Rectangle[][]
            {
                new Rectangle[] { new Rectangle(0, 0, 32, 32),new Rectangle(32, 0, 32, 32) },
                new Rectangle[] { new Rectangle(0, 32, 32, 32),new Rectangle(32, 32, 32, 32) },
                new Rectangle[] { new Rectangle(0, 64, 32, 32),new Rectangle(32, 64, 32, 32) },
                new Rectangle[] { new Rectangle(0, 96, 32, 32),new Rectangle(32, 96, 32, 32) },
            };

            AddComponent(new CmpAnim(this, content.Load<Texture2D>("spritesheets/bunny"), sprites));
        }
    }
}
