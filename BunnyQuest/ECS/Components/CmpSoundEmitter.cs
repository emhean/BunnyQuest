using Microsoft.Xna.Framework.Audio;
using System.Collections.Generic;

namespace BunnyQuest.ECS.Components
{
    class CmpSoundEmitter : Component
    {
        public Dictionary<string, SoundEffect> soundEffects;

        private List<SoundEffectInstance> instances;

        public CmpSoundEmitter(Entity owner) : base(owner)
        {
            soundEffects = new Dictionary<string, SoundEffect>();
            instances = new List<SoundEffectInstance>();
        }

        public void EmitSound(string sfx_name)
        {
            var inst = soundEffects[sfx_name].CreateInstance();
            inst.Play();
            instances.Add(inst);
        }

        public override void Update(float delta)
        {
            for(int i = 0; i < instances.Count; ++i)
            {
                if (instances[i].State == SoundState.Stopped)
                    instances.RemoveAt(i);
            }
        }
    }
}
