using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace BunnyQuest.ECS.Components
{
    class CmpAnim : Component
    {
        Texture2D spriteSheet;
        public Rectangle[][] sprites;
        public int currentSprite;
        public int currentSpriteCollection;

        public float animSpeed = 0.3f;
        public float t;

        public CmpAnim(Entity entity, Texture2D spriteSheet) : base(entity)
        {
            this.spriteSheet = spriteSheet;
        }
        public CmpAnim(Entity entity, Texture2D spriteSheet, Rectangle[][] sprites) : base(entity)
        {
            this.spriteSheet = spriteSheet;
            this.sprites = sprites;
        }

        public override void Update(float delta)
        {
            t += delta;

            if(t > animSpeed)
            {
                currentSprite += 1;
                if (currentSprite == sprites[currentSpriteCollection].Length)
                {
                    currentSprite = 0;
                }
                t = 0;
            }
        }

        public override void Render(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(spriteSheet, entity.pos, sprites[currentSpriteCollection][currentSprite], Color.White);
        }
    }
}
