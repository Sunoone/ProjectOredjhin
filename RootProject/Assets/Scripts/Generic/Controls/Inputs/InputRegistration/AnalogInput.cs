using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Freethware.Math;

namespace Freethware.Inputs
{
    public enum InputDirection
    {
        DownBack,
        Down,
        DownForward,
        Back,
        Neutral,
        Forward,
        UpBack,
        Up,
        UpForward,
        None,
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

  

    /// <summary>
    /// Replace the input string with an analog enum that simply is in sync with the InputManager.asset
    /// </summary>
    [System.Serializable]
    public class AnalogInput : ICloneable<AnalogInput>
    {

        [AnalogInput]
        public string InputString;
        public virtual bool IsSimulated() { return false; }

        protected float _value;  
        public virtual float GetAxis()
        {
            _value = Mathf.Clamp(Input.GetAxis(InputString), -1, 1);
            Debug.Log(_value);
            return _value;
        }

        public virtual void SetInputString(string ID) { InputString = ID; }
        public virtual string GetInputString() { return InputString; }

        public virtual void SetPositive(bool positive) { }
        public virtual bool GetPositive() { return false; }
        public virtual void SetKeyCode(KeyCode key) { }
        public virtual KeyCode GetKeyCode() { return KeyCode.None; }
        public virtual void SetTweenType(TweenType tweenType) { }
        public virtual TweenType GetTweenType() { return TweenType.Linear; }
        public virtual void SetAccelerationTime(float accelerationTime) { }
        public virtual float GetAccelerationTime() { return 0f; }


        public virtual AnalogInput Clone()
        {
            AnalogInput newInput = new AnalogInput();
            newInput.InputString = InputString;
            return newInput;
        }
    }

    [System.Serializable]
    public class SimulatedAnalogInput : AnalogInput
    {
        public bool Positive;
        public KeyCode InputKey;
        public TweenType TweenType;
        public float AccelerationTime = 1f;
        //public float DecelerationTime = .4f;

        private bool pressed;   
        private float time;

        private float decreaseSpeed;
        public override float GetAxis()
        {
            if (pressed != Input.GetKey(InputKey))
            {
                time = 0;
                pressed = Input.GetKey(InputKey);
                decreaseSpeed = _value;
            }

            if (pressed && _value < 1)
            {
                _value = Mathf.Clamp01(TweenEaseScrpt.GetNewValue(TweenType, time, _value, 1 - _value, AccelerationTime));
            }
            else if (!pressed && _value > 0)
            {
                _value = Mathf.Clamp01(TweenEaseScrpt.GetNewValue(TweenType, time, _value, -_value, AccelerationTime * decreaseSpeed));
            }

            time += Time.deltaTime;
            return (Positive) ? _value : -_value;
        }

        public override void SetInputString(string ID) { }
        public override string GetInputString() { return ""; }

        public override void SetPositive(bool positive) { Positive = positive; }
        public override bool GetPositive() { return Positive; }
        public override void SetKeyCode(KeyCode key) { InputKey = key; }
        public override KeyCode GetKeyCode() { return InputKey; }
        public override void SetTweenType(TweenType tweenType) { TweenType = tweenType; }
        public override TweenType GetTweenType() { return TweenType; }
        public override void SetAccelerationTime(float accelerationTime) { AccelerationTime = accelerationTime; }
        public override float GetAccelerationTime() { return AccelerationTime; }


        public override AnalogInput Clone()
        {
            SimulatedAnalogInput newInput = new SimulatedAnalogInput();
            newInput.Positive = Positive;
            newInput.InputKey = InputKey;
            newInput.TweenType = TweenType;
            newInput.AccelerationTime = AccelerationTime;
            return newInput;
        }
    }
}
