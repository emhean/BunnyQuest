using BunnyQuest.ECS;
using BunnyQuest.ECS.Components;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace BunnyQuest.Entities
{
    class Tree : Entity
    {
        public Tree(int UUID, ContentManager content) : base(UUID)
        {
            this.size = new Vector2(32, 64);

            var collider = new CmpCollider(this, content.Load<Texture2D>("etc/pixel"));
            collider.offset.Height = -48;
            collider.offset.Y = 64;
            this.AddComponent(collider);


            var sprites = new Rectangle[][]
            {
                new Rectangle[] { new Rectangle(0, 0, 32, 64) }
            };

            AddComponent(new CmpAnim(this, content.Load<Texture2D>("spritesheets/tree"), sprites));
        }
    }
}
