using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Freethware.Math;

public static class TweenUtilities  {

    /*public class Tween
    {
        public void Size2D()
        {

        }
    */



    public static IEnumerator Size2D(this RectTransform rectTransform, TweenType tweenType, Vector2 endValue, float duration)
    {
        int time = 0;
        Vector2 curSize = rectTransform.sizeDelta;
        float differenceX = endValue.x - curSize.x;
        float differenceY = endValue.y - curSize.y;

        while (time < duration)
        {
            Vector2 newSize = Vector2.zero;
            switch (tweenType)
            {
                case TweenType.Linear:
                    newSize.x = TweenEaseScrpt.Linear(time, curSize.x, differenceX, duration);
                    newSize.y = TweenEaseScrpt.Linear(time, curSize.y, differenceY, duration);
                    break;
                case TweenType.EaseInQuad:
                    newSize.x = TweenEaseScrpt.EaseInQuad(time, curSize.x, differenceX, duration);
                    newSize.y = TweenEaseScrpt.EaseInQuad(time, curSize.y, differenceY, duration);
                    break;
                case TweenType.EaseOutQuad:
                    newSize.x = TweenEaseScrpt.EaseInQuad(time, curSize.x, differenceX, duration);
                    newSize.y = TweenEaseScrpt.EaseInQuad(time, curSize.y, differenceY, duration);
                    break;
                case TweenType.EaseInOutQuad:
                    newSize.x = TweenEaseScrpt.EaseInOutQuad(time, curSize.x, differenceX, duration);
                    newSize.y = TweenEaseScrpt.EaseInOutQuad(time, curSize.y, differenceY, duration);
                    break;
                case TweenType.EaseInCubic:
                    newSize.x = TweenEaseScrpt.EaseInCubic(time, curSize.x, differenceX, duration);
                    newSize.y = TweenEaseScrpt.EaseInCubic(time, curSize.y, differenceY, duration);
                    break;
                case TweenType.EaseOutCubic:
                    newSize.x = TweenEaseScrpt.EaseOutCubic(time, curSize.x, differenceX, duration);
                    newSize.y = TweenEaseScrpt.EaseOutCubic(time, curSize.y, differenceY, duration);
                    break;
                case TweenType.EaseInOutCubic:
                    newSize.x = TweenEaseScrpt.EaseInOutCubic(time, curSize.x, differenceX, duration);
                    newSize.y = TweenEaseScrpt.EaseInOutCubic(time, curSize.y, differenceY, duration);
                    break;
                case TweenType.EaseInQuartic:
                    newSize.x = TweenEaseScrpt.EaseInQuartic(time, curSize.x, differenceX, duration);
                    newSize.y = TweenEaseScrpt.EaseInQuartic(time, curSize.y, differenceY, duration);
                    break;
                case TweenType.EaseOutQuartic:
                    newSize.x = TweenEaseScrpt.EaseOutQuartic(time, curSize.x, differenceX, duration);
                    newSize.y = TweenEaseScrpt.EaseOutQuartic(time, curSize.y, differenceY, duration);
                    break;
                case TweenType.EaseInOutQuartic:
                    newSize.x = TweenEaseScrpt.EaseInOutQuartic(time, curSize.x, differenceX, duration);
                    newSize.y = TweenEaseScrpt.EaseInOutQuartic(time, curSize.y, differenceY, duration);
                    break;
                case TweenType.EaseInQuint:
                    newSize.x = TweenEaseScrpt.EaseInQuint(time, curSize.x, differenceX, duration);
                    newSize.y = TweenEaseScrpt.EaseInQuint(time, curSize.y, differenceY, duration);
                    break;
                case TweenType.EaseOutQuint:
                    newSize.x = TweenEaseScrpt.EaseOutQuint(time, curSize.x, differenceX, duration);
                    newSize.y = TweenEaseScrpt.EaseOutQuint(time, curSize.y, differenceY, duration);
                    break;
                case TweenType.EaseInOutQuint:
                    newSize.x = TweenEaseScrpt.EaseInOutQuint(time, curSize.x, differenceX, duration);
                    newSize.y = TweenEaseScrpt.EaseInOutQuint(time, curSize.y, differenceY, duration);
                    break;
                case TweenType.EaseInSine:
                    newSize.x = TweenEaseScrpt.EaseInSine(time, curSize.x, differenceX, duration);
                    newSize.y = TweenEaseScrpt.EaseInSine(time, curSize.y, differenceY, duration);
                    break;
                case TweenType.EaseOutSine:
                    newSize.x = TweenEaseScrpt.EaseOutSine(time, curSize.x, differenceX, duration);
                    newSize.y = TweenEaseScrpt.EaseOutSine(time, curSize.y, differenceY, duration);
                    break;
                case TweenType.EaseInOutSine:
                    newSize.x = TweenEaseScrpt.EaseInOutSine(time, curSize.x, differenceX, duration);
                    newSize.y = TweenEaseScrpt.EaseInOutSine(time, curSize.y, differenceY, duration);
                    break;
                case TweenType.EaseInExpo:
                    newSize.x = TweenEaseScrpt.EaseInExpo(time, curSize.x, differenceX, duration);
                    newSize.y = TweenEaseScrpt.EaseInExpo(time, curSize.y, differenceY, duration);
                    break;
                case TweenType.EaseOutExpo:
                    newSize.x = TweenEaseScrpt.EaseOutExpo(time, curSize.x, differenceX, duration);
                    newSize.y = TweenEaseScrpt.EaseOutExpo(time, curSize.y, differenceY, duration);
                    break;
                case TweenType.EaseInOutExpo:
                    newSize.x = TweenEaseScrpt.EaseInOutExpo(time, curSize.x, differenceX, duration);
                    newSize.y = TweenEaseScrpt.EaseInOutExpo(time, curSize.y, differenceY, duration);
                    break;
                case TweenType.EaseInCirc:
                    newSize.x = TweenEaseScrpt.EaseInCirc(time, curSize.x, differenceX, duration);
                    newSize.y = TweenEaseScrpt.EaseInCirc(time, curSize.y, differenceY, duration);
                    break;
                case TweenType.EaseOutCirc:
                    newSize.x = TweenEaseScrpt.EaseOutCirc(time, curSize.x, differenceX, duration);
                    newSize.y = TweenEaseScrpt.EaseOutCirc(time, curSize.y, differenceY, duration);
                    break;
                case TweenType.EaseInOutCirc:
                    newSize.x = TweenEaseScrpt.EaseInOutCirc(time, curSize.x, differenceX, duration);
                    newSize.y = TweenEaseScrpt.EaseInOutCirc(time, curSize.y, differenceY, duration);
                    break;
                default:
                    break;
            }
            time++;
            yield return new WaitForEndOfFrame();
        }
    }

    public static Vector2 Size2D(int time, TweenType tweenType, Vector2 curValue, Vector2 endValue, float duration)
    {
        float differenceX = endValue.x - curValue.x;
        float differenceY = endValue.y - curValue.y;

        Vector2 newSize = Vector2.zero;
        switch (tweenType)
        {
            case TweenType.Linear:
                newSize.x = TweenEaseScrpt.Linear(time, curValue.x, differenceX, duration);
                newSize.y = TweenEaseScrpt.Linear(time, curValue.y, differenceY, duration);
                break;
            case TweenType.EaseInQuad:
                newSize.x = TweenEaseScrpt.EaseInQuad(time, curValue.x, differenceX, duration);
                newSize.y = TweenEaseScrpt.EaseInQuad(time, curValue.y, differenceY, duration);
                break;
            case TweenType.EaseOutQuad:
                newSize.x = TweenEaseScrpt.EaseInQuad(time, curValue.x, differenceX, duration);
                newSize.y = TweenEaseScrpt.EaseInQuad(time, curValue.y, differenceY, duration);
                break;
            case TweenType.EaseInOutQuad:
                newSize.x = TweenEaseScrpt.EaseInOutQuad(time, curValue.x, differenceX, duration);
                newSize.y = TweenEaseScrpt.EaseInOutQuad(time, curValue.y, differenceY, duration);
                break;
            case TweenType.EaseInCubic:
                newSize.x = TweenEaseScrpt.EaseInCubic(time, curValue.x, differenceX, duration);
                newSize.y = TweenEaseScrpt.EaseInCubic(time, curValue.y, differenceY, duration);
                break;
            case TweenType.EaseOutCubic:
                newSize.x = TweenEaseScrpt.EaseOutCubic(time, curValue.x, differenceX, duration);
                newSize.y = TweenEaseScrpt.EaseOutCubic(time, curValue.y, differenceY, duration);
                break;
            case TweenType.EaseInOutCubic:
                newSize.x = TweenEaseScrpt.EaseInOutCubic(time, curValue.x, differenceX, duration);
                newSize.y = TweenEaseScrpt.EaseInOutCubic(time, curValue.y, differenceY, duration);
                break;
            case TweenType.EaseInQuartic:
                newSize.x = TweenEaseScrpt.EaseInQuartic(time, curValue.x, differenceX, duration);
                newSize.y = TweenEaseScrpt.EaseInQuartic(time, curValue.y, differenceY, duration);
                break;
            case TweenType.EaseOutQuartic:
                newSize.x = TweenEaseScrpt.EaseOutQuartic(time, curValue.x, differenceX, duration);
                newSize.y = TweenEaseScrpt.EaseOutQuartic(time, curValue.y, differenceY, duration);
                break;
            case TweenType.EaseInOutQuartic:
                newSize.x = TweenEaseScrpt.EaseInOutQuartic(time, curValue.x, differenceX, duration);
                newSize.y = TweenEaseScrpt.EaseInOutQuartic(time, curValue.y, differenceY, duration);
                break;
            case TweenType.EaseInQuint:
                newSize.x = TweenEaseScrpt.EaseInQuint(time, curValue.x, differenceX, duration);
                newSize.y = TweenEaseScrpt.EaseInQuint(time, curValue.y, differenceY, duration);
                break;
            case TweenType.EaseOutQuint:
                newSize.x = TweenEaseScrpt.EaseOutQuint(time, curValue.x, differenceX, duration);
                newSize.y = TweenEaseScrpt.EaseOutQuint(time, curValue.y, differenceY, duration);
                break;
            case TweenType.EaseInOutQuint:
                newSize.x = TweenEaseScrpt.EaseInOutQuint(time, curValue.x, differenceX, duration);
                newSize.y = TweenEaseScrpt.EaseInOutQuint(time, curValue.y, differenceY, duration);
                break;
            case TweenType.EaseInSine:
                newSize.x = TweenEaseScrpt.EaseInSine(time, curValue.x, differenceX, duration);
                newSize.y = TweenEaseScrpt.EaseInSine(time, curValue.y, differenceY, duration);
                break;
            case TweenType.EaseOutSine:
                newSize.x = TweenEaseScrpt.EaseOutSine(time, curValue.x, differenceX, duration);
                newSize.y = TweenEaseScrpt.EaseOutSine(time, curValue.y, differenceY, duration);
                break;
            case TweenType.EaseInOutSine:
                newSize.x = TweenEaseScrpt.EaseInOutSine(time, curValue.x, differenceX, duration);
                newSize.y = TweenEaseScrpt.EaseInOutSine(time, curValue.y, differenceY, duration);
                break;
            case TweenType.EaseInExpo:
                newSize.x = TweenEaseScrpt.EaseInExpo(time, curValue.x, differenceX, duration);
                newSize.y = TweenEaseScrpt.EaseInExpo(time, curValue.y, differenceY, duration);
                break;
            case TweenType.EaseOutExpo:
                newSize.x = TweenEaseScrpt.EaseOutExpo(time, curValue.x, differenceX, duration);
                newSize.y = TweenEaseScrpt.EaseOutExpo(time, curValue.y, differenceY, duration);
                break;
            case TweenType.EaseInOutExpo:
                newSize.x = TweenEaseScrpt.EaseInOutExpo(time, curValue.x, differenceX, duration);
                newSize.y = TweenEaseScrpt.EaseInOutExpo(time, curValue.y, differenceY, duration);
                break;
            case TweenType.EaseInCirc:
                newSize.x = TweenEaseScrpt.EaseInCirc(time, curValue.x, differenceX, duration);
                newSize.y = TweenEaseScrpt.EaseInCirc(time, curValue.y, differenceY, duration);
                break;
            case TweenType.EaseOutCirc:
                newSize.x = TweenEaseScrpt.EaseOutCirc(time, curValue.x, differenceX, duration);
                newSize.y = TweenEaseScrpt.EaseOutCirc(time, curValue.y, differenceY, duration);
                break;
            case TweenType.EaseInOutCirc:
                newSize.x = TweenEaseScrpt.EaseInOutCirc(time, curValue.x, differenceX, duration);
                newSize.y = TweenEaseScrpt.EaseInOutCirc(time, curValue.y, differenceY, duration);
                break;
            default:
                break;
        }
        return newSize;
    }

    public static float Tween(float time, TweenType tweenType, float curValue, float endValue, float duration)
    {
        float difference = endValue - curValue;
        float newValue = 0;
        switch (tweenType)
        {
            case TweenType.Linear:
                newValue = TweenEaseScrpt.Linear(time, curValue, difference, duration);
                break;
            case TweenType.EaseInQuad:
                newValue = TweenEaseScrpt.Linear(time, curValue, difference, duration);
                break;
            case TweenType.EaseOutQuad:
                newValue = TweenEaseScrpt.Linear(time, curValue, difference, duration);
                break;
            case TweenType.EaseInOutQuad:
                newValue = TweenEaseScrpt.Linear(time, curValue, difference, duration);
                break;
            case TweenType.EaseInCubic:
                newValue = TweenEaseScrpt.Linear(time, curValue, difference, duration);
                break;
            case TweenType.EaseOutCubic:
                newValue = TweenEaseScrpt.Linear(time, curValue, difference, duration);
                break;
            case TweenType.EaseInOutCubic:
                newValue = TweenEaseScrpt.Linear(time, curValue, difference, duration);
                break;
            case TweenType.EaseInQuartic:
                newValue = TweenEaseScrpt.Linear(time, curValue, difference, duration);
                break;
            case TweenType.EaseOutQuartic:
                newValue = TweenEaseScrpt.Linear(time, curValue, difference, duration);
                break;
            case TweenType.EaseInOutQuartic:
                newValue = TweenEaseScrpt.Linear(time, curValue, difference, duration);
                break;
            case TweenType.EaseInQuint:
                newValue = TweenEaseScrpt.Linear(time, curValue, difference, duration);
                break;
            case TweenType.EaseOutQuint:
                newValue = TweenEaseScrpt.Linear(time, curValue, difference, duration);
                break;
            case TweenType.EaseInOutQuint:
                newValue = TweenEaseScrpt.Linear(time, curValue, difference, duration);
                break;
            case TweenType.EaseInSine:
                newValue = TweenEaseScrpt.Linear(time, curValue, difference, duration);
                break;
            case TweenType.EaseOutSine:
                newValue = TweenEaseScrpt.Linear(time, curValue, difference, duration);
                break;
            case TweenType.EaseInOutSine:
                newValue = TweenEaseScrpt.Linear(time, curValue, difference, duration);
                break;
            case TweenType.EaseInExpo:
                newValue = TweenEaseScrpt.Linear(time, curValue, difference, duration);
                break;
            case TweenType.EaseOutExpo:
                newValue = TweenEaseScrpt.Linear(time, curValue, difference, duration);
                break;
            case TweenType.EaseInOutExpo:
                newValue = TweenEaseScrpt.Linear(time, curValue, difference, duration);
                break;
            case TweenType.EaseInCirc:
                newValue = TweenEaseScrpt.Linear(time, curValue, difference, duration);
                break;
            case TweenType.EaseOutCirc:
                newValue = TweenEaseScrpt.Linear(time, curValue, difference, duration);
                break;
            case TweenType.EaseInOutCirc:
                newValue = TweenEaseScrpt.Linear(time, curValue, difference, duration);
                break;
            default:
                break;
        }
        return newValue;
    }

    public static IEnumerator Transparency(this Color color, TweenType p_tweenType, float p_endValue, float p_duration, Callback onComplete = null)
    {
        int t_time = 0;
        float t_difference = p_endValue - color.a;

        while (t_time <= p_duration)
        {
            float newValue = TweenEaseScrpt.Linear(t_time, color.a, t_difference, p_duration);
            color.a = newValue;
            t_time++;
            yield return new WaitForEndOfFrame();
        }

        if (onComplete != null)
            onComplete.Invoke();
    }

    /*public static IEnumerator Tween(this IEnumerator animation, Delegate onComplete)
    {

    }*/
}
