using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Freethware.Math
{
    public enum TweenType
    {
        Linear,

        EaseInQuad,
        EaseOutQuad,
        EaseInOutQuad,

        EaseInCubic,
        EaseOutCubic,
        EaseInOutCubic,

        EaseInQuartic,
        EaseOutQuartic,
        EaseInOutQuartic,

        EaseInQuint,
        EaseOutQuint,
        EaseInOutQuint,

        EaseInSine,
        EaseOutSine,
        EaseInOutSine,

        EaseInExpo,
        EaseOutExpo,
        EaseInOutExpo,

        EaseInCirc,
        EaseOutCirc,
        EaseInOutCirc
    }
    public static class TweenEaseScrpt
    {
        public static float GetNewValue(TweenType p_tweenType, float p_time, float p_startValue, float p_differenceValue, float p_duration)
        {
            switch (p_tweenType)
            {
                case TweenType.Linear:
                    return Linear(p_time, p_startValue, p_differenceValue, p_duration);
                case TweenType.EaseInQuad:
                    return TweenEaseScrpt.EaseInQuad(p_time, p_startValue, p_differenceValue, p_duration);
                case TweenType.EaseOutQuad:
                    return TweenEaseScrpt.EaseOutQuad(p_time, p_startValue, p_differenceValue, p_duration);
                case TweenType.EaseInOutQuad:
                    return TweenEaseScrpt.EaseInOutQuad(p_time, p_startValue, p_differenceValue, p_duration);
                case TweenType.EaseInCubic:
                    return TweenEaseScrpt.EaseInCubic(p_time, p_startValue, p_differenceValue, p_duration);
                case TweenType.EaseOutCubic:
                    return TweenEaseScrpt.EaseOutCubic(p_time, p_startValue, p_differenceValue, p_duration);
                case TweenType.EaseInOutCubic:
                    return TweenEaseScrpt.EaseInOutCubic(p_time, p_startValue, p_differenceValue, p_duration);
                case TweenType.EaseInQuartic:
                    return TweenEaseScrpt.EaseInQuartic(p_time, p_startValue, p_differenceValue, p_duration);
                case TweenType.EaseOutQuartic:
                    return TweenEaseScrpt.EaseOutQuartic(p_time, p_startValue, p_differenceValue, p_duration);
                case TweenType.EaseInOutQuartic:
                    return TweenEaseScrpt.EaseInOutQuartic(p_time, p_startValue, p_differenceValue, p_duration);
                case TweenType.EaseInQuint:
                    return TweenEaseScrpt.EaseInQuint(p_time, p_startValue, p_differenceValue, p_duration);
                case TweenType.EaseOutQuint:
                    return TweenEaseScrpt.EaseOutQuint(p_time, p_startValue, p_differenceValue, p_duration);
                case TweenType.EaseInOutQuint:
                    return TweenEaseScrpt.EaseInOutQuint(p_time, p_startValue, p_differenceValue, p_duration);
                case TweenType.EaseInSine:
                    return TweenEaseScrpt.EaseInSine(p_time, p_startValue, p_differenceValue, p_duration);
                case TweenType.EaseOutSine:
                    return TweenEaseScrpt.EaseOutSine(p_time, p_startValue, p_differenceValue, p_duration);
                case TweenType.EaseInOutSine:
                    return TweenEaseScrpt.EaseInOutSine(p_time, p_startValue, p_differenceValue, p_duration);
                case TweenType.EaseInExpo:
                    return TweenEaseScrpt.EaseInExpo(p_time, p_startValue, p_differenceValue, p_duration);
                case TweenType.EaseOutExpo:
                    return TweenEaseScrpt.EaseOutExpo(p_time, p_startValue, p_differenceValue, p_duration);
                case TweenType.EaseInOutExpo:
                    return TweenEaseScrpt.EaseInOutExpo(p_time, p_startValue, p_differenceValue, p_duration);
                case TweenType.EaseInCirc:
                    return TweenEaseScrpt.EaseInCirc(p_time, p_startValue, p_differenceValue, p_duration);
                case TweenType.EaseOutCirc:
                    return TweenEaseScrpt.EaseOutCirc(p_time, p_startValue, p_differenceValue, p_duration);
                case TweenType.EaseInOutCirc:
                    return TweenEaseScrpt.EaseInOutCirc(p_time, p_startValue, p_differenceValue, p_duration);
                default:
                    return TweenEaseScrpt.Linear(p_time, p_startValue, p_differenceValue, p_duration);
            }
        }



    public static float Linear(float t, float b, float c, float d)
        {
            return ((c * t) / d) + b;
        }

        public static float EaseInQuad(float t, float b, float c, float d)
        {
            t /= d;
            return c * t * t + b;
        }
        public static float EaseOutQuad(float t, float b, float c, float d)
        {
            t /= d;
            return -c * t * (t - 2) + b;
        }
        public static float EaseInOutQuad(float t, float b, float c, float d)
        {
            t /= d / 2;
            if (t < 1) return c / 2 * t * t + b;
            t--;
            return -c / 2 * (t * (t - 2) - 1) + b;
        }

        public static float EaseInCubic(float t, float b, float c, float d)
        {
            t /= d;
            return c * t * t * t + b;
        }
        public static float EaseOutCubic(float t, float b, float c, float d)
        {
            t /= d;
            t--;
            return c * (t * t * t + 1) + b;
        }
        public static float EaseInOutCubic(float t, float b, float c, float d)
        {
            t /= d / 2;
            if (t < 1) return c / 2 * t * t * t + b;
            t -= 2;
            return c / 2 * (t * t * t + 2) + b;
        }

        public static float EaseInQuartic(float t, float b, float c, float d)
        {
            t /= d;
            return c * t * t * t * t + b;
        }
        public static float EaseOutQuartic(float t, float b, float c, float d)
        {
            t /= d;
            t--;
            return -c * (t * t * t * t - 1) + b;
        }
        public static float EaseInOutQuartic(float t, float b, float c, float d)
        {
            t /= d / 2;
            if (t < 1) return c / 2 * t * t * t * t + b;
            t -= 2;
            return -c / 2 * (t * t * t * t - 2) + b;
        }

        public static float EaseInQuint(float t, float b, float c, float d)
        {
            t /= d;
            return c * t * t * t * t * t + b;
        }
        public static float EaseOutQuint(float t, float b, float c, float d)
        {
            t /= d;
            t--;
            return c * (t * t * t * t * t + 1) + b;
        }
        public static float EaseInOutQuint(float t, float b, float c, float d)
        {
            t /= d / 2;
            if (t < 1) return c / 2 * t * t * t * t * t + b;
            t -= 2;
            return c / 2 * (t * t * t * t * t + 2) + b;
        }

        public static float EaseInSine(float t, float b, float c, float d)
        {
            return -c * Mathf.Cos(t / d * (Mathf.PI / 2)) + c + b;
        }
        public static float EaseOutSine(float t, float b, float c, float d)
        {
            return c * Mathf.Sin(t / d * (Mathf.PI / 2)) + b;
        }
        public static float EaseInOutSine(float t, float b, float c, float d)
        {
            return -c / 2 * (Mathf.Cos(Mathf.PI * t / d) - 1) + b;
        }

        public static float EaseInExpo(float t, float b, float c, float d)
        {
            return c * Mathf.Pow(2, 10 * (t / d - 1)) + b;
        }
        public static float EaseOutExpo(float t, float b, float c, float d)
        {
            return c * (-Mathf.Pow(2, -10 * t / d) + 1) + b;
        }
        public static float EaseInOutExpo(float t, float b, float c, float d)
        {
            t /= d / 2;
            if (t < 1) return c / 2 * Mathf.Pow(2, 10 * (t - 1)) + b;
            t--;
            return c / 2 * (-Mathf.Pow(2, -10 * t) + 2) + b;
        }

        public static float EaseInCirc(float t, float b, float c, float d)
        {
            t /= d;
            return -c * (Mathf.Sqrt(1 - t * t) - 1) + b;
        }
        public static float EaseOutCirc(float t, float b, float c, float d)
        {
            t /= d;
            t--;
            return c * Mathf.Sqrt(1 - t * t) + b;
        }
        public static float EaseInOutCirc(float t, float b, float c, float d)
        {
            t /= d / 2;
            if (t < 1) return -c / 2 * (Mathf.Sqrt(1 - t * t) - 1) + b;
            t -= 2;
            return c / 2 * (Mathf.Sqrt(1 - t * t) + 1) + b;
        }
    }
}
