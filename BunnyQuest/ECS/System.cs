using BunnyQuest.ECS.Components;
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

        static int uuid_count;
        static int GetAvailableUUID()
        {
            int foo = uuid_count;
            uuid_count += 1;
            return foo;
        }

        public System()
        {
            this.entities = new List<Entity>();
        }

        public Entity CreateEntity()
        {
            var ent = new Entity(GetAvailableUUID());
            return ent;
        }

        public void AddEntity(Entity entity)
        {
            this.entities.Add(entity);
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
                for (int j = 0; j < entities[i].components.Count; ++j)
                {
                    if (entities[i].components[j].IsRendered)
                    {
                        entities[i].components[j].Render(spriteBatch);
                    }
                }
            }
        }

        public void UpdateCollision()
        {

            for (int i = 0; i < entities.Count; i++)
            {
                var c1 = entities[i].GetComponent<CmpCollider>();

                if (c1 != null)
                {
                    for (int j = i; j < entities.Count - i; j++)
                    {
                        var c2 = entities[j].GetComponent<CmpCollider>();

                        if (c2 != null)
                        {
                            float Ax1 = c1.rect.X;
                            float Ax2 = Ax1 + c1.rect.Width;
                            float Bx1 = c2.rect.X;
                            float Bx2 = Bx1 + c2.rect.Width;

                            if( ( Ax1 < Bx1 && Bx1 < Ax2 )
                                || (Ax1 < Bx2 && Bx2 < Ax2))
                            {
                                float Ay1 = c1.rect.Y;
                                float Ay2 = Ay1 + c1.rect.Height;
                                float By1 = c2.rect.Y;
                                float By2 = By1 + c2.rect.Height;

                                if ((Ay1 < By1 && By1 <Ay2)
                                || (Ay1 < By2 && By2 < Ay2))
                                {
                                    if (Math.Abs(Bx1 - Ax1) < Math.Abs(Bx1 - Ax2))
                                        Ax2 = Bx1;
                                    else
                                        Ax1 = Bx2;

                                    if (Math.Abs(By1 - Ay1) < Math.Abs(By1 - Ay2))
                                        Ay1 = By2;
                                    else
                                        Ay2 = By1;
                                }
                            }
                        }
                    }
                }




            }
        }

    }
}
