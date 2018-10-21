using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Freethware.Math;

namespace Freethware.Inputs
{
    [System.Serializable]
    public class Input_Analog_Simulated : Input_Analog_Base
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

        public override void SetPositive(bool positive) { Positive = positive; }
        public override bool GetPositive() { return Positive; }
        public override void SetKeyCode(KeyCode key) { InputKey = key; }
        public override KeyCode GetKeyCode() { return InputKey; }
        public override void SetTweenType(TweenType tweenType) { TweenType = tweenType; }
        public override TweenType GetTweenType() { return TweenType; }
        public override void SetAccelerationTime(float accelerationTime) { AccelerationTime = accelerationTime; }
        public override float GetAccelerationTime() { return AccelerationTime; }


        public override Input_Analog_Base Clone()
        {
            Input_Analog_Simulated newInput = new Input_Analog_Simulated();
            newInput.Positive = Positive;
            newInput.InputKey = InputKey;
            newInput.TweenType = TweenType;
            newInput.AccelerationTime = AccelerationTime;
            return newInput;
        }
    }
}
