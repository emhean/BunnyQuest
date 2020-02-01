using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

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
            this.spritesheet = spritesheet;
            this.position = position;
            particle_list = new List<Particle>();

        }

        public Texture2D spritesheet;
        public Vector2 position;
        private List<Particle> particle_list;
        public float emission_timer;

        

        public override void Update(float delta)
        {
            
        }

        public override void Render(SpriteBatch spriteBatch)
        {
            //spriteBatch.Draw();
        }

        public void Emit()
        {
            
        }
            

    }

    
}
