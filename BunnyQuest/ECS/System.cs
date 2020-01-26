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

            UpdateWorldCollision(8 * 32, 8 * 32); // TODO: Fix this or implement method somewhere else?
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

        public void UpdateWorldCollision(int worldWidth, int worldHeight)
        {
            for (int i = 0; i < entities.Count; ++i)
            {
                if (entities[i].pos.X < 0)
                    entities[i].pos.X = 0;
                else if (entities[i].pos.X + entities[i].size.X > worldWidth)
                    entities[i].pos.X = (worldWidth - entities[i].size.X);

                if (entities[i].pos.Y < 0)
                    entities[i].pos.Y = 0;
                else if (entities[i].pos.Y + entities[i].size.Y  > worldHeight)
                    entities[i].pos.Y  = (worldHeight - entities[i].size.Y);
            }
        }

        public void UpdateCollision()
        {
        }

        public void enemy_movement()
        {
            Vector2 dest = new Vector2();

            for (int i = 0; i< entities.Count; i++)
            {
                if (entities[i] is Entities.Player p)
                {
                    dest = entities[i].pos;
                    break;
                }   
            }

            for (int i = 0; i < entities.Count; i++)
            {
                var ai = entities[i].GetComponent<CmpAi>();

                if(ai != null)
                {
                    ai.set_destination(dest);
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