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
        }

        public void Update(GameTime gameTime)
        {
            for(int i = 0; i < entities.Count; ++i)
            {

            }
        }

        public void Render(SpriteBatch spriteBatch)
        {
            for (int i = 0; i < entities.Count; ++i)
            {
                entities[i].Render(spriteBatch);
            }
        }
    }
}
