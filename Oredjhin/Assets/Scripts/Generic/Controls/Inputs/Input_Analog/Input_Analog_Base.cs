using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Freethware.Math;

namespace Freethware.Inputs
{
    public enum Direction
    {
        DownBack = 1,
        Down = 2,
        DownForward = 4,
        Back = 8,
        Neutral = 16,
        Forward = 32,
        UpBack = 64,
        Up = 128,
        UpForward = 256,      
    }
    /*public enum InputDirection
{
    DownBack = (1 << 0),
    Down = (1 << 1),
    DownForward = (1 << 2),
    Back = (1 << 3),
    Neutral = (1 << 4),
    Forward = (1 << 5),
    UpBack = (1 << 6),
    Up = (1 << 7),
    UpForward = (1 << 8),
    None = (1 << 9),
}*/

    public class Input_Analog_Base : ICloneable<Input_Analog_Base>
    {
        public virtual bool IsSimulated() { return false; }

        protected float _value = 0f;
        public virtual float GetAxis() { return _value; }

        public virtual void SetInputString(string ID) { }
        public virtual string GetInputString() { return ""; }

        public virtual void SetPositive(bool positive) { }
        public virtual bool GetPositive() { return false; }
        public virtual void SetKeyCode(KeyCode key) { }
        public virtual KeyCode GetKeyCode() { return KeyCode.None; }
        public virtual void SetTweenType(TweenType tweenType) { }
        public virtual TweenType GetTweenType() { return TweenType.Linear; }
        public virtual void SetAccelerationTime(float accelerationTime) { }
        public virtual float GetAccelerationTime() { return 0f; }

        public virtual Input_Analog_Base Clone()
        {
            Input_Analog_Base newInput = new Input_Analog_Base();
            return newInput;
        }
    }
}