using BunnyQuest.Camera;
using BunnyQuest.ECS;
using BunnyQuest.ECS.Components;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace BunnyQuest
{
    /// <summary>
    /// Utilities class.
    /// </summary>
    static class Utils
    {
        public static bool InRange(Vector2 from, Vector2 to, float range)
        {
            return (range > GetDistance(from, to));
        }

        public static Vector2 GetDirection(Vector2 from, Vector2 to)
        {
            return Vector2.Normalize(Vector2.Subtract(to, from));
        }

        public static float GetDistance(Vector2 from, Vector2 to)
        {
            return Vector2.Distance(to, from);
        }

        static public Vector2 GetEntityScreenPosition(Entity entity, Camera2D camera, GraphicsDevice graphicsDevice)
        {
            return Vector2.Transform(entity.pos, camera.GetTransformation(graphicsDevice));
        }

        static public Rectangle GetEntityScreenIntersection(Entity entity, Vector2 entity_screenPosition, Rectangle screen_rect)
        {
            //var screenPos = GetEntityScreenPosition(entity, graphicsDevice);
            var ent_rect = new Rectangle((int)entity_screenPosition.X, (int)entity_screenPosition.Y, (int)entity.size.X, (int)entity.size.Y);

            return Rectangle.Intersect(ent_rect, screen_rect);
        }

        static public void SetAnimBasedOfDirection(CmpAnim anim, Vector2 direction)
        {
            // We check which absolute value that is bigger to determine which axis to prioritize
            if(Math.Abs(direction.Y) < Math.Abs(direction.X))
            {
                if (direction.X > 0) // Right
                {
                    anim.currentSpriteCollection = 1;
                }
                else if (direction.X < 0) // Left
                {
                    anim.currentSpriteCollection = 3;
                }
            }
            else
            {
                if (direction.Y < 0) // Up
                {
                    anim.currentSpriteCollection = 0;
                }
                else if (direction.Y > 0) // Down
                {
                    anim.currentSpriteCollection = 2;
                }
            }
            // There is probably a smarter way to do this...
            //else if (direction.X == 1 && direction.Y == -1)
            //{
            //    anim.currentSpriteCollection = 0;
            //    anim.rotation = 0.45f;
            //}
            //else if (direction.X == -1 && direction.Y == -1)
            //{
            //    anim.currentSpriteCollection = 0;
            //    anim.rotation = -0.45f;
            //}
            //else if (direction.X == 1 && direction.Y == 1)
            //{
            //    anim.currentSpriteCollection = 1;
            //    anim.rotation = 0.90f;
            //}
            //else if (direction.X == -1 && direction.Y == 1)
            //{
            //    anim.currentSpriteCollection = 3;
            //    anim.rotation = -0.90f;
            //}
            //else anim.rotation = 0f;
        }

        /// <summary>
        /// Enumerate the entities and get those who collide with the cursor
        /// </summary>
        static public IEnumerable<Entity> EnumerateCursorCollisioners(ECS.Engine system, Rectangle cursor_rect, Camera2D camera, GraphicsDevice graphicsDevice)
        {
            Entity ent;
            //Vector2 v_cursor = new Vector2(cursor_rect.X, cursor_rect.Y);

            for (int i = 0; i < system.GetEntityCount(); ++i)
            {
                ent = system.GetEntityFromIndex(i);

                //if (Vector2.Distance(ent.GetCenterPosition(), v_cursor) > 255)
                //    continue;

                if (GetEntityScreenIntersection(ent, GetEntityScreenPosition(ent, camera, graphicsDevice), cursor_rect) != Rectangle.Empty)
                    yield return ent;
            }

            // Used to be:
            //foreach (BetaBunny bb in system.EnumerateEntities<BetaBunny>(1, 100))
            //{
            //    if (GetEntityScreenIntersection(bb, cursor_rect) != Rectangle.Empty)
            //    {
            //        if (bb.ai.State != CmpAI_Follower.STATE_CmpAI_Follower.Following)
            //        {
            //            bb.Follow(player);
            //        }
            //    }
            //}
        }


    }
}
