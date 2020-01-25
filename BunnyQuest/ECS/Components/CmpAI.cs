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
        public CmpAi(Entity owner) : base(owner)
        {
        }

        public Vector2 destination;

        public void set_destination(Vector2 dest)
        {
            destination = dest;
        }

        public override void Update(float delta)
        {
            var dir = Vector2.Normalize(Vector2.Subtract(entity.pos, destination));
        }
    }
}
