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



        public void SetState(CmpAI_Follower.STATE_CmpAI_Follower state)
        {
            ai.State = state;

            if (state == CmpAI_Follower.STATE_CmpAI_Follower.Following)
            {
                anim.renderColor = Color.LightGray;
            }
            else if (state == CmpAI_Follower.STATE_CmpAI_Follower.NoneToFollow)
            {
                anim.renderColor = Color.Gray;
            }
            //else if (state == CmpAI_Follower.STATE_CmpAI_Follower.Separated)
            //{
            //    anim.renderColor = Color.Gray;
            //}
        }
    }
}
