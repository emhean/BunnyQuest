using BunnyQuest.ECS;
using BunnyQuest.ECS.Components;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace BunnyQuest.Entities
{
    class BetaBunny : Entity
    {
        public CmpAI_Follower ai;
        public CmpAnim anim;

        /// <summary>
        /// Don't touch this.
        /// </summary>
        public int follower_value;

        public static List<BetaBunny> followers = new List<BetaBunny>();

        public void Follow(Entity e)
        {
            if(followers.Count != 0)
            {
                ai.entity_toFollow = followers[followers.Count - 1];
                followers.Add(this);
                follower_value = followers.Count;
            }
            else
            {
                ai.entity_toFollow = e;
                followers.Add(this);
                follower_value = followers.Count;
            }

            ai.State = CmpAI_Follower.STATE_CmpAI_Follower.Following;
            anim.renderColor = Color.LightGray;
        }


        public void Unfollow()
        {
            ai.State = CmpAI_Follower.STATE_CmpAI_Follower.NoneToFollow;
            anim.renderColor = Color.Gray;

            // Sets isFirst to true if they are the same instance.
            //bool wasFirst = (followers[0].Equals(this));

            followers.Remove(this);
            //if (wasFirst && followers.Count != 0)
            //{
            //    followers[0].Follow(followers[1]);
            //}
            ai.entity_toFollow = null;

            if (followers.Count != 0)
            {
                for (int i = 0; i < followers.Count; ++i)
                {
                    followers[i].follower_value = i;
                }
            }
        }

        public void GetFollowers()
        {

        }

        public BetaBunny(int UUID, ContentManager content) : base(UUID)
        {
            this.AddComponent(new CmpCollider(this, content.Load<Texture2D>("etc/pixel"))
            {
                GetsPushed = true,
                GetsPushedBySameType = false,
                Type = "betabunny"
            });

            this.AddComponent(new CmpStats(this, 10, 3, true));

            var sprites = new Rectangle[][]
            {
                new Rectangle[] { new Rectangle(0, 0, 32, 32), new Rectangle(32, 0, 32, 32) },
                new Rectangle[] { new Rectangle(0, 32, 32, 32), new Rectangle(32, 32, 32, 32) },
                new Rectangle[] { new Rectangle(0, 64, 32, 32), new Rectangle(32, 64, 32, 32) },
                new Rectangle[] { new Rectangle(0, 96, 32, 32), new Rectangle(32, 96, 32, 32) },
            };

            this.anim = new CmpAnim(this, content.Load<Texture2D>("spritesheets/bunny"), sprites);
            anim.renderColor = Color.Gray;

            this.ai = new CmpAI_Follower(this, Vector2.One * 2)
            {
                distance_whenToFollow = 30f
            };

            AddComponent(anim);
            AddComponent(ai);
        }
    }
}
