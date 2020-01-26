using BunnyQuest.ECS;
using BunnyQuest.ECS.Components;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace BunnyQuest.Entities
{
    class EvilBunny : Entity
    {
        public EvilBunny(int UUID, ContentManager content) : base(UUID)
        {
            this.AddComponent(new CmpCollider(this,  content.Load<Texture2D>("etc/pixel")));

            var patrol_points = new List<Vector2>()
            {
                Vector2.Zero, new Vector2(100,40), new Vector2(60, 200)
            };

            this.AddComponent(new CmpAi(this, Vector2.One, true, false, patrol_points));

            var sprites = new Rectangle[][]
            {
                new Rectangle[] { new Rectangle(0, 0, 32, 32), new Rectangle(32, 0, 32, 32) },
                new Rectangle[] { new Rectangle(0, 32, 32, 32), new Rectangle(32, 32, 32, 32) },
                new Rectangle[] { new Rectangle(0, 64, 32, 32), new Rectangle(32, 64, 32, 32) },
                new Rectangle[] { new Rectangle(0, 96, 32, 32), new Rectangle(32, 96, 32, 32) },
            };

            AddComponent(new CmpAnim(this, content.Load<Texture2D>("spritesheets/evilbunny"), sprites));
        }
    }
}
