using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace BunnyQuest.ECS.Components
{
    class CmpParticleEmitter : Component
    {
        class Particle
        {
            Vector2 pos;
        }

        public CmpParticleEmitter(Entity owner) : base(owner)
        {
        }

        public override void Update(float delta)
        {
            
        }

        public override void Render(SpriteBatch spriteBatch)
        {
            
        }
    }
}
