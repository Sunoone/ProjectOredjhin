using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

namespace Freethware.Effects
{
    [System.Serializable]
    public class MoveEffect : TweenEffect
    {
        public MoveEffect(Transform target) { Target = target; }
        public MoveEffect(Transform target, Vector3 position, float duration, bool snap)
        {
            Target = target;
            Position = position;
            Duration = duration;
            Snap = snap;
        }

        public Transform Target;

        public Vector3 Position = Vector3.zero;
        public float Duration = 1;
        public bool Snap = false;

        public override Effect Play(Vector3 position, float duration)
        {
            Position = position;
            Duration = duration;
            return Play();
        }


        protected override void InitEffect()
        {
            _tween = Target.DOLocalMove(Position, Duration, Snap);
            _tween.Play();
            base.InitEffect();
        }
        protected override void OverrideEffect() { _tween.ChangeEndValue(Position); }
    }
}
