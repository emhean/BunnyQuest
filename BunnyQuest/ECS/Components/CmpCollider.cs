using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BunnyQuest.ECS.Components
{
    class CmpCollider : Component
    {
        public CmpCollider(Entity owner) : base(owner)
        {
        }
    }
}
