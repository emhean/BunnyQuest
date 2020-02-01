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
            this.AddComponent(new CmpParticleEmitter(this));
            this.AddComponent(new CmpSoundEmitter(this));
        }

        public bool isEmitting;

        public void Emit()
        {
            isEmitting = true;
        }
        public void stopEmitting()
        {
            isEmitting = false;
        }

        
        



    }
}