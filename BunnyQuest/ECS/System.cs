using BunnyQuest.ECS.Components;
using BunnyQuest.World;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace BunnyQuest.ECS
{
    #region UUID Stuff
    partial class System
    {
        static int uuid_count;

        static int GetAvailableUUID()
        {
            int foo = uuid_count;
            uuid_count += 1;
            return foo;
        }
    }
    #endregion


    #region Entity Methods
    partial class System
    {
        /// <summary>
        /// Gets the entity of specified UUID. 
        /// </summary>
        public Entity GetEntity(int uuid)
        {
            for (int i = 0; i < entities.Count; ++i)
            {
                if (entities[i].UUID == uuid)
                    return entities[i];
            }

            throw new Exception("CRITICAL!!! An Entity of that UUID does not exist!!");
        }

        /// <summary>
        /// Gets an entity from index of list. Is not equivalent to UUID!
        /// </summary>
        public Entity GetEntityFromIndex(int index)
        {
            return entities[index];
        }

        /// <summary>
        /// Creates an instance of Entity. Does NOT add it to the list.
        /// </summary>
        public Entity CreateEntity()
        {
            var ent = new Entity(GetAvailableUUID());
            return ent;
        }

        /// <summary>
        /// Adds an entity to the list of entities.
        /// </summary>
        public void AddEntity(Entity entity)
        {
            this.entities.Add(entity);
        }

        /// <summary>
        /// Adds the entity to the expired list and removes entity from active list.
        /// </summary>
        public void ExpireEntity(Entity entity)
        {
            entities_expired.Add(entity);
            entities.Remove(entity);
        }
        
        /// <summary>
        /// Gets the total entity count.
        /// </summary>
        public int GetEntityCount()
        {
            return entities.Count;
        }
    }
    #endregion


    #region Fields and Constructor
    partial class System
    {
        private List<Entity> entities;
        private List<Entity> entities_expired;
        private Map2D map;

        public System(Map2D map)
        {
            this.map = map;
            this.entities = new List<Entity>();
            this.entities_expired = new List<Entity>();
        }
    }
    #endregion


    #region Update Logic
    partial class System
    {
        /// <summary>
        /// The main update loop that runs at 60 times per second.
        /// </summary>
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

                if (entities[i].expired)
                {
                    ExpireEntity(entities[i]);
                    continue;
                }
            }

            UpdateCollision(map); // TODO: Fix this or implement method somewhere else?
            EnemyMovement();
        }

        public void UpdateCollision(Map2D map)
        {
            for (int i = 0; i < entities.Count; ++i)
            {
                CheckMapBoundsCollision(entities[i]);

                var collider = entities[i].GetComponent<CmpCollider>();
                if (collider != null)
                {
                    foreach (var row in map.tileGrid.grid)
                    {
                        foreach (var tile in row)
                        {
                            if (!tile.collidable || !tile.rect.Intersects(collider.rect))
                                continue;

                            ResolveCollision(collider, tile.rect);
                        }

                    }

                    for (int j = 0; j < entities.Count; ++j)
                    {
                        if (j == i)
                            continue;

                        var other = entities[j].GetComponent<CmpCollider>();
                        if (other != null)
                        {
                            Rectangle intersection = Rectangle.Intersect(collider.rect, other.rect);

                            COLLISION_SIDE side = GetIntersectionSide(collider.rect, other.rect);

                            if (collider.GetsPushed && side != COLLISION_SIDE.None)
                            {
                                var first_unit = entities[i].GetComponent<CmpStats>();
                                var second_unit = entities[j].GetComponent<CmpStats>();

                                if ((first_unit != null) && (second_unit != null))
                                {
                                    if (first_unit.iframes <= 0)
                                    {
                                        first_unit.TakeDamage(second_unit.damage);
                                        first_unit.iframes = 1;
                                    }
                                }

                                if (side == COLLISION_SIDE.Top)
                                {
                                    //entities[i].pos.Y -= intersection.Height;
                                    continue;
                                }
                                else if (side == COLLISION_SIDE.Bottom)
                                {
                                    //entities[i].pos.Y += (intersection.Height);
                                    continue;
                                }

                                if (side == COLLISION_SIDE.Left)
                                {
                                    //entities[i].pos.X -= intersection.Width;
                                    continue;
                                }
                                else if (side == COLLISION_SIDE.Right)
                                {
                                    //entities[i].pos.X += intersection.Width;
                                    continue;
                                }
                            }
                        }
                    }
                }
            }
        }

    }
    #endregion


    #region Render Logic
    partial class System
    {
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
    }
    #endregion


    #region Collision Logic
    partial class System
    {
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


        bool ResolveCollision(CmpCollider collider, CmpCollider other)
        {
            return ResolveCollision(collider, other.rect);
        }

        bool ResolveCollision(CmpCollider collider, Rectangle other)
        {
            Rectangle intersection = Rectangle.Intersect(collider.rect, other);
            COLLISION_SIDE side = this.GetIntersectionSide(collider.rect, other);

            if (side == COLLISION_SIDE.Top)
            {
                collider.SetPosition(collider.rect.X, other.Y - collider.rect.Height);
                return true;
            }
            else if (side == COLLISION_SIDE.Bottom)
            {
                collider.SetPosition(collider.rect.X, other.Y + other.Height);
                return true;
            }

            if (side == COLLISION_SIDE.Left)
            {
                collider.SetPosition(other.X - collider.rect.Width, collider.rect.Y);
                return true;
            }
            else if (side == COLLISION_SIDE.Right)
            {
                collider.SetPosition(other.X + other.Width, collider.rect.Y);
                return true;
            }

            return false;
        }

        /// <summary>
        /// Prevents the entity to move outside the boundaries of the map.
        /// </summary>
        public void CheckMapBoundsCollision(Entity entity)
        {
            if (entity.pos.X < 0)
                entity.pos.X = 0;
            else if (entity.pos.X + entity.size.X > map.mapWidth * 32)
                entity.pos.X = (map.mapWidth * 32 - entity.size.X);

            if (entity.pos.Y < 0)
                entity.pos.Y = 0;
            else if (entity.pos.Y + entity.size.Y > map.mapHeight * 32)
                entity.pos.Y = (map.mapHeight * 32 - entity.size.Y);
        }
    }
    #endregion

    #region AI
    partial class System
    {
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
                        if (Vector2.Distance(dest, entities[i].pos) >= 100)
                        {
                            ai.set_ai_type("patrolling");
                        }
                        else
                        {
                            ai.set_destination(dest);
                        }
                    }
                    else if (ai.is_patrolling)
                    {
                        if (Vector2.Distance(dest, entities[i].pos) < 100)
                        {
                            ai.set_ai_type("chasing");
                        }
                        else
                        {
                            if ((ai.patrol_timer <= 0) && (Vector2.Distance(entities[i].pos, ai.patrol_points[ai.which_point]) < 10))
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
        }
    }
    #endregion
}
