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

            UpdateWorldCollision();
            UpdateCollision();
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

        public void UpdateWorldCollision()
        {
            for(int i = 0; i < entities.Count; ++i)
            {
                if (entities[i].pos.X < 0)
                    entities[i].pos.X = 0;

                if (entities[i].pos.Y < 0)
                    entities[i].pos.Y = 0;


            }
        }

        public void UpdateCollision()
        {
            for (int i = 0; i < entities.Count; i++)
            {
                var c1 = entities[i].GetComponent<CmpCollider>();
                if (c1 != null)
                {
                    for (int j = 0; j < entities.Count; ++j)//int j = i; j < entities.Count - i; j++)
                    {
                        if (i == j)
                            continue;

                        var c2 = entities[j].GetComponent<CmpCollider>();
                        if (c2 != null)
                        {
                            //Creates variables for the pos of left and right sides of each hitbox
                            int Ax1 = c1.rect.X;
                            int Ax2 = Ax1 + c1.rect.Width;
                            int Bx1 = c2.rect.X;
                            int Bx2 = Bx1 + c2.rect.Width;

                            //Creates variables for the pos of top and bottom of each hitbox
                            int Ay1 = c1.rect.Y;
                            int Ay2 = Ay1 + c1.rect.Height;
                            int By1 = c2.rect.Y;
                            int By2 = By1 + c2.rect.Height;


                            //Checks if any of the sides of one hitbox are within the other on both axes
                            // TODO: Fix null-state
                            if ((Ax1 < Bx1 && Bx1 < Ax2)
                                || (Ax1 < Bx2 && Bx2 < Ax2))
                            {
                                if ((Ay1 < By1 && By1 < Ay2)
                                || (Ay1 < By2 && By2 < Ay2))
                                {
                                    
                                    if (Math.Abs(By1 - Ay1) >= Math.Abs(By1 - Ay2))
                                    {
                                        if (Math.Abs(Bx1 - Ax1) >= Math.Abs(Bx1 - Ax2))
                                        {
                                            if(Math.Abs(Bx1-Ax2) > Math.Abs(By1-Ay2))
                                            {
                                                c1.SetPosition(c1.rect.X, By2 - c1.rect.Height);
                                                Console.WriteLine("1");
                                            }
                                            else
                                            {
                                                c1.SetPosition(Bx1 - c1.rect.Width, c1.rect.Y);
                                                Console.WriteLine("2");
                                            }
                                            //Console.WriteLine("1");
                                            //c1.SetPosition(c1.rect.X, By2 - c1.rect.Height);
                                            //c1.SetPosition(Bx1 - c1.rect.Width, c1.rect.Y);


                                        }
                                        else
                                        {
                                            if (Math.Abs(Bx1 - Ax2) > Math.Abs(By1 - Ay2))
                                            {
                                                c1.SetPosition(c1.rect.X, By2 - c1.rect.Height);
                                                Console.WriteLine("3");

                                            }

                                            else
                                            {
                                                c1.SetPosition(Bx2, c1.rect.Y);
                                                Console.WriteLine("4");
                                            }
                                            //c1.SetPosition(c1.rect.X, By2 - c1.rect.Height);
                                            //Console.WriteLine("2");
                                        }
                                    }
                                    else
                                    {
                                        if (Math.Abs(Bx1 - Ax1) >= Math.Abs(Bx1 - Ax2))
                                        {
                                            if(Math.Abs(Bx1-Ax2) > Math.Abs(By1-Ay1))
                                            {
                                                c1.SetPosition(c1.rect.X, c1.rect.Y);
                                                Console.WriteLine("5");

                                            }
                                            else
                                            {
                                                c1.SetPosition(c1.rect.X, By2);
                                                Console.WriteLine("6");

                                            }
                                            //Console.WriteLine("3");
                                            //c1.SetPosition(c1.rect.X, By1);
                                            //c1.SetPosition(Bx2, c1.rect.Y);
                                        }
                                        else
                                        {
                                            if(Math.Abs(Bx1-Ax1) > Math.Abs(By1-Ay1))
                                            {
                                                c1.SetPosition(c1.rect.X, By2 + c2.rect.Height);
                                                Console.WriteLine("7");

                                            }
                                            else
                                            {
                                                c1.SetPosition(c1.rect.X, By2);
                                                Console.WriteLine("8");

                                            }
                                            //c1.SetPosition(Bx2, c1.rect.Y);
                                            //Console.WriteLine("4");
                                        }
                                    }

                                   
                                    //if (Math.Abs(Bx1 - Ax1) >= Math.Abs(Bx1 - Ax2))
                                    //{
                                    //    Console.WriteLine("1");
                                    //    c1.SetPosition(Bx1 - c1.rect.Width, c1.rect.Y);
                                    //}
                                    //else
                                    //{
                                    //    Console.WriteLine("2");
                                    //    c1.SetPosition(Bx2, c1.rect.Y);
                                    //}
                                }


                            }
                        }

                    }
                }
            }




        }
    }

}

