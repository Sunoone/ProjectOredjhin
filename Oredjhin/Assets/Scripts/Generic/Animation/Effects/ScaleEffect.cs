using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

namespace Freethware.Effects
{
    public class ScaleEffect : TweenEffect
    {
        public ScaleEffect() { }
        public ScaleEffect(Transform target)
        {
            Target = target;
            FloatScale = 1;
        }
        public ScaleEffect(Transform target, Vector3 scale, float duration)
        {
            Target = target;
            VectorScale = scale;
            Duration = duration;
        }

        public ScaleEffect(Transform target, float scale, float duration)
        {
            Target = target;
            FloatScale = scale;
            Duration = duration;
        }

        public Transform Target;

        private bool isFloatScale = true;
        private Vector3 vectorScale = new Vector3(1, 1, 1);
        public Vector3 VectorScale
        {
            get { return vectorScale; }
            set
            {
                vectorScale = value;
                isFloatScale = false;
            }
        }
        private float floatScale = 1;
        public float FloatScale
        {
            get { return floatScale; }
            set
            {
                floatScale = value;
                isFloatScale = true;
            }
        }

        public float Duration = 1;
       

        public override Effect Play(Vector3 scale, float duration)
        {
            _onComplete = null;
            VectorScale = scale;
            Duration = duration;
            return Play();
        }
        public virtual Effect Play(Transform target, Vector3 scale, float duration)
        {
            _onComplete = null;
            Target = target;
            VectorScale = scale;
            Duration = duration;
            return Play();
        }
        public virtual Effect Play(float scale, float duration)
        {
            _onComplete = null;
            FloatScale = scale;
            Duration = duration;
            return Play();
        }
        public virtual Effect Play(Transform target, float scale, float duration)
        {
            _onComplete = null;
            Target = target;
            FloatScale = scale;
            Duration = duration;
            return Play();
        }

        protected override void InitEffect()
        {
            if (isFloatScale)
                _tween = Target.DOScale(FloatScale, Duration);
            else
                _tween = Target.DOScale(VectorScale, Duration);
            _tween.Play();
            base.InitEffect();
        }
        protected override void OverrideEffect()
        {
            if (isFloatScale)
                _tween.ChangeEndValue(FloatScale);
            else
                _tween.ChangeEndValue(VectorScale);
        }

    }
}
