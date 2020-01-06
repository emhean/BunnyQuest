using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BunnyQuest.ECS
{
    class Entity
    {
        private int UUID;
        public List<Component> components;

        public Entity(int UUID)
        {
            this.UUID = UUID;
            this.components = new List<Component>();
        }
    }
}
