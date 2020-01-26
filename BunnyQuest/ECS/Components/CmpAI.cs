using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace BunnyQuest.ECS.Components
{
    class CmpAi : Component
    {
        public CmpAi(Entity owner, Vector2 init_velocity) : base(owner)
        {
            velocity = init_velocity;
        }

        public Vector2 destination;
        public Vector2 velocity;

        public void update_velocity(Vector2 spd)
            {
                velocity = spd;
            }

        public void set_destination(Vector2 dest)
        {
            destination = dest;
        }

        public override void Update(float delta)
        {
            var dir = Vector2.Normalize(Vector2.Subtract(destination, entity.pos));


            entity.pos += dir * velocity;
            //entity.pos = Vector2.Add(entity.pos,  dir*velocity);
        }
    }
}
