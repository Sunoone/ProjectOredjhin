using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

namespace Freethware.Effects
{
    public enum Phase
    {
        Idle,
        Active,
        Done,
        Pause,
    }

    public static class EffectExtensions
    {
        public static MoveEffect DoMoveEffect(this Transform target, Vector3 position, float duration, bool snap = false)
        {
            MoveEffect ME = new MoveEffect(target, position, duration, snap);
            ME.Play();
            return ME;
        }
        public static MoveEffect CreateMoveEffect(this Transform target, Vector3 position, float duration, bool snap = false)
        {
            MoveEffect ME = new MoveEffect(target, position, duration, snap);
            return ME;
        }
        public static MoveEffect CreateMoveEffect(this Transform target)
        {
            MoveEffect ME = new MoveEffect(target);
            return ME;
        }
        public static ScaleEffect DoScaleEffect(this Transform target, Vector3 scale, float duration, bool snap = false)
        {
            ScaleEffect SE = new ScaleEffect(target, scale, duration);
            SE.Play();
            return SE;
        }
        public static ScaleEffect DoScaleEffect(this Transform target, float scale, float duration, bool snap = false)
        {
            ScaleEffect SE = new ScaleEffect(target, scale, duration);
            SE.Play();
            return SE;
        }
        public static ScaleEffect CreateScaleEffect(this Transform target, Vector3 scale, float duration, bool snap = false)
        {
            ScaleEffect SE = new ScaleEffect(target, scale, duration);
            return SE;
        }
        public static ScaleEffect CreateScaleEffect(this Transform target, float scale, float duration, bool snap = false)
        {
            ScaleEffect SE = new ScaleEffect(target, scale, duration);
            return SE;
        }
        public static ScaleEffect CreateScaleEffect(this Transform target)
        {
            ScaleEffect SE = new ScaleEffect(target);
            return SE;
        }
    }


    [System.Serializable]
    public abstract class Effect
    {
        public bool EndOnStop = false;
        protected Phase Phase = Phase.Idle;
        protected Callback _onComplete;

        public Effect OnComplete(Callback cb)
        {
            _onComplete = cb;
            return this;
        }

        protected virtual void Init()
        {
            Phase = Phase.Active;
            InitEffect();
        }
        protected virtual void End()
        {
            _onComplete.Call();
            _onComplete = null;

            Phase = Phase.Done;
            EndEffect();
        }      
        public virtual Effect Play()
        {
            if (Phase == Phase.Pause)
            {
                UnPauseEffect();
                return this;
            }

            Stop();
            Init();
            return this;
        }     
        public virtual void Pause()
        {
            /*if (Phase == Phase.Done)
            {
                int length = EffectsOnComplete.Count;
                for (int i = 0; i < length; i++)
                    EffectsOnComplete[i].Pause();
                return;
            }*/

            Phase = Phase.Pause;
            PauseEffect();
        }      
        public virtual void Stop()
        {
            if (EndOnStop && Phase == Phase.Active)
                End();

            _onComplete = null;
            Phase = Phase.Idle;
            StopEffect();
        }

        protected virtual void InitEffect() { }
        protected virtual void OverrideEffect() { }
        protected virtual void EndEffect() { }
        protected virtual void PauseEffect() { }
        protected virtual void UnPauseEffect() { }
        protected virtual void StopEffect() { }
    }

    public abstract class TweenEffect : Effect
    {
        protected Tweener _tween;

        public virtual Effect Play(Vector3 endValue, float duration) { return Play(); }
        public override void Stop()
        {
            if (EndOnStop && Phase == Phase.Active)
                End();

            Phase = Phase.Idle;
            StopEffect();
        }
        protected override void InitEffect() {
            _tween.OnComplete(() => End());
            _tween.Play();
        }
        protected override void EndEffect() { _tween = null; }
        protected override void PauseEffect() { _tween.Pause(); }
        protected override void UnPauseEffect() { _tween.Play(); }
        protected override void StopEffect() {
            if (_tween != null)
            {
                _tween.Kill();
                _tween = null;
            }
        }
    }
}
