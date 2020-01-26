using BunnyQuest.ECS.Components;
using BunnyQuest.World;
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
        Map map;

        static int uuid_count;
        static int GetAvailableUUID()
        {
            int foo = uuid_count;
            uuid_count += 1;
            return foo;
        }

        public System(Map map)
        {
            this.map = map;
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

            UpdateCollision(map); // TODO: Fix this or implement method somewhere else?
            EnemyMovement();
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

        public void UpdateCollision(Map map)
        {
            for (int i = 0; i < entities.Count; ++i)
            {
                #region World collision so that no entity moves outside the world
                if (entities[i].pos.X < 0)
                    entities[i].pos.X = 0;
                else if (entities[i].pos.X + entities[i].size.X > map.mapWidth * 32)
                    entities[i].pos.X = (map.mapWidth * 32 - entities[i].size.X);

                if (entities[i].pos.Y < 0)
                    entities[i].pos.Y = 0;
                else if (entities[i].pos.Y + entities[i].size.Y > map.mapHeight * 32)
                    entities[i].pos.Y = (map.mapHeight * 32 - entities[i].size.Y);
                #endregion


                var collider = entities[i].GetComponent<CmpCollider>();
                if (collider != null)
                {
                    foreach (var row in map.tileGrid.grid)
                    {
                        foreach (var tile in row)
                        {
                            if (tile.collidable != true || !tile.rect.Intersects(collider.rect))
                                continue;

                            Rectangle intersection = Rectangle.Intersect(collider.rect, tile.rect);

                            COLLISION_SIDE side = this.GetIntersectionSide(collider.rect, tile.rect);

                            var e = entities[i];
                            if (side == COLLISION_SIDE.Top)// && collider.rect.Top < intersection.Top
                            {
                                e.pos.Y = tile.rect.Y - e.size.Y;
                                //entities[i].pos.Y -= intersection.Height;
                                //collider.OnCollided(new CollisionArgs(side));
                                //collider.Update(delta);
                                continue;
                            }
                            else if (side == COLLISION_SIDE.Bottom)// && collider.rect.Bottom > intersection.Top
                            {
                                e.pos.Y = tile.rect.Y + tile.rect.Height;
                                //entities[i].pos.Y += (intersection.Height);
                                //collider.OnCollided(new CollisionArgs(side));
                                //collider.Update(delta);
                                continue;
                            }

                            if (side == COLLISION_SIDE.Left) //&& collider.rect.Y > tile.rect.Y
                            {
                                e.pos.X = tile.rect.X - e.size.X;
                                //entities[i].pos.X -= intersection.Width;
                                //collider.OnCollided(new CollisionArgs(side));
                                //collider.Update(delta);
                                continue;
                            }
                            else if (side == COLLISION_SIDE.Right)//((collider.rect.Bottom) > tile.rect.Top && side == COLLISION_SIDE.Right)  //&& collider.rect.Bottom < tile.rect.Top
                            {
                                e.pos.X = tile.rect.X + tile.rect.Width;
                                //entities[i].pos.X += intersection.Width;
                                //collider.OnCollided(new CollisionArgs(side));
                                //collider.Update(delta);
                                continue;
                            }
                        }

                    }

                    for (int j = 0; j < entities.Count; ++j)// (int j = i; j < entities.Count - i; ++j)
                    {
                        if (j == i)
                            continue;

                        var other = entities[j].GetComponent<CmpCollider>();
                        if (other != null)
                        {
                            Rectangle intersection = Rectangle.Intersect(collider.rect, other.rect);

                            COLLISION_SIDE side = GetIntersectionSide(collider.rect, other.rect);

                            if (side != COLLISION_SIDE.None)
                            {
                                if (side == COLLISION_SIDE.Top)
                                {
                                    entities[i].pos.Y -= intersection.Height;
                                    continue;
                                }
                                else if (side == COLLISION_SIDE.Bottom)
                                {
                                    entities[i].pos.Y += (intersection.Height);
                                    continue;
                                }

                                if (side == COLLISION_SIDE.Left)
                                {
                                    entities[i].pos.X -= intersection.Width;
                                    continue;
                                }
                                else if (side == COLLISION_SIDE.Right)
                                {
                                    entities[i].pos.X += intersection.Width;
                                    continue;
                                }
                            }
                        }
                    }
                }
            }
        }

        public void EnemyMovement()
        {
            // First we create a Vector2 containing the player's position, so we know where to go
            Vector2 dest = Vector2.Zero;


            // Sets destination of AI to player's position.
            for (int i = 0; i < entities.Count; i++)
            {
                if (entities[i] is Entities.Player p)
                {
                    dest = entities[i].pos;
                    break;
                }
            }

            // Then we give the position to each enemy with an AI component. Their movement is managed within the class
            for (int i = 0; i < entities.Count; i++)
            {
                var ai = entities[i].GetComponent<CmpAi>();

                if (ai != null)
                {
                    if (ai.is_chasing)
                    {
                        ai.set_destination(dest);
                    }
                    else if (ai.is_patrolling)
                    {
                        if ((ai.patrol_timer <= 0 ) && (Vector2.Distance(entities[i].pos, ai.patrol_points[ai.which_point]) < 10))
                        {
                            //ai.set_ai_type("chasing");
                            Console.WriteLine(entities[0].pos);
                            ai.cycle_patrol_points();
                            ai.set_destination(ai.patrol_points[ai.which_point]);
                            ai.set_patrol_timer(1);
                        }
                        else
                        {
                            ai.set_destination(ai.patrol_points[ai.which_point]);
                        }

                    }

                }
            }
        }


        public enum COLLISION_SIDE
        {
            /// <summary>No collision occurred.</summary>
            None = 0,
            /// <summary>Collision occurred at the top side.</summary>
            Top = 1,
            /// <summary>Collision occurred at the bottom side.</summary>
            Bottom = 2,
            /// <summary>Collision occurred at the left side.</summary>
            Left = 4,
            /// <summary>Collision occurred at the right side.</summary>
            Right = 8
        }

        /// <summary>
        /// AABB math to get side of intersection.
        /// </summary>
        public COLLISION_SIDE GetIntersectionSide(Rectangle rect, Rectangle other)
        {
            Rectangle intersection = Rectangle.Intersect(rect, other);
            if (intersection == Rectangle.Empty)
                return COLLISION_SIDE.None;


            COLLISION_SIDE side;

            float wy = (rect.Width + other.Width) * (rect.Center.Y - other.Center.Y);
            float hx = (rect.Height + other.Height) * (rect.Center.X - other.Center.X);


            if (wy > hx)
            {
                if (wy > -hx)
                    side = COLLISION_SIDE.Bottom;
                else
                    side = COLLISION_SIDE.Left;
            }
            else
            {
                if (wy > -hx)
                    side = COLLISION_SIDE.Right;
                else
                    side = COLLISION_SIDE.Top;
            }

            return side;
        }
    }
}
