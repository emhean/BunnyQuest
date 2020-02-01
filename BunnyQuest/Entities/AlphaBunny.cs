using BunnyQuest.ECS;
using BunnyQuest.ECS.Components;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace BunnyQuest.Entities
{
    class AlphaBunny : Entity
    {
        public Vector2 speed = new Vector2(2.5f, 2.5f);
        public Vector2 direction = Vector2.Zero;

        public AlphaBunny(int UUID, ContentManager content) : base(UUID)
        {
            this.AddComponent(new CmpCollider(this, content.Load<Texture2D>("etc/pixel"))
            {
                GetsPushed = true,
                GetsPushedBySameType = true,
                Type = "alphabunny",
                TypeFilter = new string[] { "betabunny" }
            });

            this.AddComponent(new CmpStats(this, 10, 0, true));

            var sprites = new Rectangle[][]
            {
                new Rectangle[] { new Rectangle(0, 0, 32, 32), new Rectangle(32, 0, 32, 32) },
                new Rectangle[] { new Rectangle(0, 32, 32, 32), new Rectangle(32, 32, 32, 32) },
                new Rectangle[] { new Rectangle(0, 64, 32, 32), new Rectangle(32, 64, 32, 32) },
                new Rectangle[] { new Rectangle(0, 96, 32, 32), new Rectangle(32, 96, 32, 32) },
            };

            var anim = new CmpAnim(this, content.Load<Texture2D>("spritesheets/bunny"), sprites)
            {
                renderColor = Color.Pink
            };
            AddComponent(anim);
        }




        public List<BetaBunny> followers = new List<BetaBunny>();


        public void AddFollower(BetaBunny betaBunny)
        {
            if(followers.Count == 0)
                betaBunny.ai.entity_toFollow = this;
            else
            {
                betaBunny.ai.entity_toFollow = followers[followers.Count - 1];
            }

            betaBunny.SetState(CmpAI_Follower.STATE_CmpAI_Follower.Following);

            followers.Add(betaBunny);
        }

        public void RemoveFollower(BetaBunny betaBunny)
        {
            if (followers.Count > 1)
            {
                int index = followers.IndexOf(betaBunny);

                var foo = new List<BetaBunny>();

                for (int i = index; i < followers.Count; i++)
                {
                    followers[i].SetState(CmpAI_Follower.STATE_CmpAI_Follower.NoneToFollow);
                    followers[i].ai.entity_toFollow = null;
                    foo.Add(followers[i]);
                }

                foreach (var bb in foo)
                    followers.Remove(bb);
            }
            else
            {
                betaBunny.ai.entity_toFollow = null;
                betaBunny.SetState(CmpAI_Follower.STATE_CmpAI_Follower.NoneToFollow);

                followers.Remove(betaBunny);
            }


        }

        public void RemoveAllFollowers()
        {
            for(int i = 0; i < followers.Count; ++i)
            {
                followers[i].ai.entity_toFollow = null;
                followers[i].SetState(CmpAI_Follower.STATE_CmpAI_Follower.NoneToFollow);
            }

            followers.Clear();
        }
    }
}
