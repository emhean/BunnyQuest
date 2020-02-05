using BunnyQuest.Camera;
using BunnyQuest.ECS;
using BunnyQuest.ECS.Components;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace BunnyQuest
{
    static class Utilities
    {
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

        static public void RotateAnimToDirection(CmpAnim anim, Vector2 direction)
        {
            // There is probably a smarter way to do this...
            if (direction.X == 1 && direction.Y == -1)
            {
                anim.currentSpriteCollection = 0;
                anim.rotation = 0.45f;
            }
            else if (direction.X == -1 && direction.Y == -1)
            {
                anim.currentSpriteCollection = 0;
                anim.rotation = -0.45f;
            }
            else if (direction.X == 1 && direction.Y == 1)
            {
                anim.currentSpriteCollection = 1;
                anim.rotation = 0.90f;
            }
            else if (direction.X == -1 && direction.Y == 1)
            {
                anim.currentSpriteCollection = 3;
                anim.rotation = -0.90f;
            }
            else anim.rotation = 0f;
        }

        /// <summary>
        /// Enumerate the entities and get those who collide with the cursor
        /// </summary>
        static public IEnumerable<Entity> EnumerateCursorCollisioners(ECS.System system, Rectangle cursor_rect, Camera2D camera, GraphicsDevice graphicsDevice)
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
