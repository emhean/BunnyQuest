using BunnyQuest.ECS;
using BunnyQuest.ECS.Components;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace BunnyQuest.Entities
{
    class EvilBunny : Entity
    {
        public EvilBunny(int UUID, ContentManager content) : base(UUID)
        {
            this.AddComponent(new CmpCollider(this,  content.Load<Texture2D>("etc/pixel")));

            this.AddComponent(new CmpAi(this, Vector2.One));

            var anim = new CmpAnim(this, content.Load<Texture2D>("spritesheets/evilbunny"));
            this.AddComponent(anim);

            anim.sprites = new Rectangle[]
            {
                new Rectangle(0,0, 32, 32),
                new Rectangle(32, 0, 32, 32)
            };
        }
    }
}
