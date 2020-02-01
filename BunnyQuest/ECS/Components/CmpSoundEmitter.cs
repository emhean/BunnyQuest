using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using System.Collections.Generic;

namespace BunnyQuest.ECS.Components
{
    class CmpSoundEmitter : Component
    {
        public Dictionary<string, SoundEffect> soundEffects;

        private List<SoundEffectInstance> instances;
        private ContentManager contentManager;

        public CmpSoundEmitter(Entity owner, ContentManager contentManager) : base(owner)
        {
            this.soundEffects = new Dictionary<string, SoundEffect>();
            this.instances = new List<SoundEffectInstance>();
            this.contentManager = contentManager;
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
