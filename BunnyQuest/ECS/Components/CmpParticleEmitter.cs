using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace BunnyQuest.ECS.Components
{
    class CmpParticleEmitter : Component
    {
        class Particle
        {
            public Particle(Vector2 pos, float fade_time, Vector2 velocity, Vector2 dir)
            {
                this.pos = pos;
                this.fade_time = fade_time;
                this.velocity = velocity;
                this.dir = dir;
            }
            Vector2 pos;
            float fade_time;
            Vector2 velocity;
            Vector2 dir;
        }

        public CmpParticleEmitter(Entity owner, Texture2D spritesheet, Vector2 position) : base(owner)
        {
        }

        public override void Update(float delta)
        {
            
        }

        public override void Render(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw();
        }

        public void Emit()
        {

        }
            

    }

    
}
