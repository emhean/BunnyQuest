using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BunnyQuest.ECS
{
    class System
    {
        List<Entity> entities;

        public System()
        {
            this.entities = new List<Entity>();
        }

        public void Update(GameTime gameTime)
        {
            float dt = (float)gameTime.ElapsedGameTime.TotalSeconds;

            for (int i = 0; i < entities.Count; ++i)
            {
                for (int j = 0; j < entities[i].components.Count; ++j)
                {
                    if (entities[i].components[j].IsUpdated)
                    {
                        entities[i].components[j].Update(dt);
                    }
                }
            }
        }

        public void Render(SpriteBatch spriteBatch)
        {
            for (int i = 0; i < entities.Count; ++i)
            {
                for(int j = 0; j < entities[i].components.Count; ++j)
                {
                    if (entities[i].components[j].IsRendered)
                    {
                        entities[i].components[j].Render(spriteBatch);
                    }
                }
            }
        }
    }
}
