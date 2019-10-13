using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

namespace Freethware.Effects
{
    public class SizeEffect : TweenEffect
    {
        public SizeEffect() { }
        public SizeEffect(RectTransform target)
        {
            Target = target;
            Scale = 1;
        }
        public SizeEffect(RectTransform target, Vector3 scale, float duration)
        {
            Target = target;
            Size = scale;
            Duration = duration;
        }

        public SizeEffect(RectTransform target, float scale, float duration)
        {
            Target = target;
            Scale = scale;
            Duration = duration;
        }

        public RectTransform Target;

        private bool useScale = true;
        private Vector3 size = new Vector3(1, 1, 1);
        public Vector3 Size
        {
            get { return size; }
            set
            {
                size = value;
                useScale = false;
            }
        }
        private float scale = 1;
        public float Scale
        {
            get { return scale; }
            set
            {
                scale = value;
                Size = new Vector3(Target.sizeDelta.x * scale, Target.sizeDelta.y * scale);          
                useScale = true;
            }
        }

        public float Duration = 1;
       
        public override Effect Play(Vector3 size, float duration)
        {
            _onComplete = null;
            Size = size;
            Duration = duration;
            return Play();
        }
        public virtual Effect Play(RectTransform target, Vector3 size, float duration)
        {
            _onComplete = null;
            Target = target;
            Size = size;
            Duration = duration;
            return Play();
        }
        public virtual Effect Play(float scale, float duration)
        {
            _onComplete = null;
            Scale = scale;
            Duration = duration;
            return Play();
        }
        public virtual Effect Play(RectTransform target, float scale, float duration)
        {
            _onComplete = null;
            Target = target;
            Scale = scale;
            Duration = duration;
            return Play();
        }

        protected override void InitEffect()
        {
            _tween = Target.DOSizeDelta(Size, Duration);
            _tween.Play();
            base.InitEffect();
        }
        protected override void OverrideEffect()
        {
            _tween.ChangeEndValue(Size);
        }

    }
}
