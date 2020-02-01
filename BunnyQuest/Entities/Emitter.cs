using BunnyQuest.ECS;
using BunnyQuest.ECS.Components;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BunnyQuest.Entities
{
    class Emitter : Entity
    {
        public Emitter(int UUID) : base(UUID)
        {
             this.AddComponent(new CmpGraphicsEmitter() )
        }

        public Vector2 position;
        public bool is_emitting;

        public void Emit()
        {
            is_emitting = true;
        }
        public void stopEmitting()
        {
            is_emitting = false;
        }

        
        



    }
}