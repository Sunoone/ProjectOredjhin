using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

namespace Freethware.Effects
{
    public class ColorEffect : TweenEffect
    {
        public Image Target;
        public Color Color;
        public float Duration = 1;

        public ColorEffect() { }
        public ColorEffect(Image target)
        {
            Target = target;
        }
        public ColorEffect(Image target, Color color, float duration)
        {
            Target = target;
            Color = color;
            Duration = duration;
        }

        public override Effect Play(Vector3 color, float duration)
        {
            _onComplete = null;
            color.x = Mathf.Clamp01(color.x);
            color.y = Mathf.Clamp01(color.y);
            color.z = Mathf.Clamp01(color.z);
            Color = new Color(color.x, color.y, color.z, 1);
            Duration = duration;
            return Play();
        }
        public virtual Effect Play(Image target, Color color, float duration)
        {
            _onComplete = null;
            Target = target;
            Color = color;
            Duration = duration;
            return Play();
        }
        protected override void InitEffect()
        {
            _tween = Target.DOColor(Color, Duration);
            _tween.Play();
            base.InitEffect();
        }
        protected override void OverrideEffect()
        {
            _tween.ChangeEndValue(Color);
        }
    }
}
