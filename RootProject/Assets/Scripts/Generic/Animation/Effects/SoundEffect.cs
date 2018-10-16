using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Freethware.Effects
{
    /// <summary>
    ///  The plugin sound manager is nice, but lacks some functionality. Will have to take a look at it later.
    /// </summary>
    public class SoundEffect : Effect
    {
        public SoundEffect(AudioSource target)
        {
            Target = target;
            Target.clip = Clip;
        }
        public SoundEffect(AudioSource target, AudioClip clip)
        {
            Target = target;
            Target.clip = clip;
        }

        public AudioSource Target;
        public AudioClip Clip;

        protected override void InitEffect() { Target.Play(); }
        protected override void EndEffect() { }
        protected override void PauseEffect() { Target.Pause(); }
        protected override void UnPauseEffect() { Target.UnPause(); }
        protected override void StopEffect() { }

        /// <summary>
        /// Apparently this gets fucked when alttabbing.
        /// </summary>
        public virtual void Update()
        {
            if (!Target.isPlaying && Phase == Phase.Active)
            {
                End();
            }
        }
    }
}
