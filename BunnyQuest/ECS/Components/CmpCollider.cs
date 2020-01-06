using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BunnyQuest.ECS.Components
{
    class CmpCollider : Component
    {
        Rectangle rect;

        public CmpCollider(Entity owner) : base(owner)
        {
        }


        public override void Update(float delta)
        {
            base.Update(delta);

            rect.X = (int)this.entity.pos.X;
            rect.Y = (int)this.entity.pos.Y;
            rect.Width = (int)this.entity.size.X;
            rect.Height = (int)this.entity.size.Y;
            
        }
    }

    
}
