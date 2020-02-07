using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BunnyQuest.ECS.Components
{
    /// <summary>
    /// A component that is a texture and is rendered for a while. Give lifespan -99 for a splash and it's infinite.
    /// </summary>
    class CmpSplashes : Component
    {
        public Texture2D texture;
        public float lifeSpan;

        List<Splash> splashes;

        class Splash
        {
            public float lifeSpan;
            public Vector2 pos;

            public bool infinite;

            public Splash(Vector2 pos, float lifeSpan)
            {
                this.pos = pos;
                this.lifeSpan = lifeSpan;

                if (lifeSpan == -99)
                    infinite = true;
            }
        }

        public CmpSplashes(Entity owner, Texture2D texture, float lifeSpan_seconds) : base(owner)
        {
            this.splashes = new List<Splash>();
            this.texture = texture;
            this.lifeSpan = lifeSpan_seconds;
        }

        public void CreateSplash()
        {
            splashes.Add(new Splash(parent.pos, this.lifeSpan));
        }

        /// <summary>
        /// Give lifespan -99 for a splash and it's infinite.
        /// </summary>
        public void CreateSplash(float lifeSpan)
        {
            splashes.Add(new Splash(parent.pos, lifeSpan));
        }

        public void CreateSplash(Vector2 pos, float lifeSpan)
        {
            splashes.Add(new Splash(pos, lifeSpan));
        }

        public void CreateSplash(Vector2 pos)
        {
            splashes.Add(new Splash(pos, this.lifeSpan));
        }

        public void ClearSplashes()
        {
            splashes.Clear();
        }

        public override void Update(float delta)
        {
            for(int i = 0; i < splashes.Count; ++i)
            {
                if (splashes[i].infinite)
                    continue;

                splashes[i].lifeSpan -= delta;
                if (splashes[i].lifeSpan < 0)
                    splashes.RemoveAt(i);
            }
        }

        public override void Render(SpriteBatch spriteBatch)
        {
            for (int i = 0; i < splashes.Count; ++i)
            {
                spriteBatch.Draw(texture, splashes[i].pos, Color.White);
            }
        }
    }
}
