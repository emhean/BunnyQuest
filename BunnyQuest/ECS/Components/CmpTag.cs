using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BunnyQuest.ECS.Components
{
    /// <summary>
    /// A component used for tagging entities.
    /// </summary>
    class CmpTag : Component
    {
        public CmpTag(Entity owner, string tag) : base(owner)
        {
            Tag = tag;

            IsUpdated = false;
            IsRendered = false;
        }

        public string Tag { get; private set; }
    }
}
