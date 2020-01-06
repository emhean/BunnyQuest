using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BunnyQuest.ECS
{
    class Entity
    {
        public readonly int UUID;
        public List<Component> components;
        public Vector2 pos;
        public Vector2 size;



        public Entity(int UUID)
        {
            this.UUID = UUID;
            this.components = new List<Component>();
        }

    }
}
