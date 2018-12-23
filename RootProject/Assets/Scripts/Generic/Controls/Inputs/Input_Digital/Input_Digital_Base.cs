using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Freethware.Inputs
{
    public enum InputState
    {
        Idle, // Used when up
        Up, // Used the frame it goes up
        Down, // Used the frame it goes down
        Hold, // Used when down
        None
    }

    [System.Serializable]
    public class Input_Digital_Base : ICloneable<Input_Digital_Base>
    {
        public virtual Input_Digital_Base Clone()
        {
            Input_Digital_Base newInput = new Input_Digital_Base();
            return newInput;
        }
        public virtual bool IsSimulated() { return false; }
        protected InputState _inputState;

        public virtual void SetInputKey(KeyCode key) { }
        public virtual KeyCode GetInputKey() { return KeyCode.None; }
        public virtual void SetInputString(string ID) { }
        public virtual string GetInputString() { return ""; }
        public virtual void SetActivationValue(float value) { }
        public virtual float GetActivationValue() { return 0f; }
        public virtual void SetOperatorCondition(OperatorCondition condition) { }
        public virtual OperatorCondition GetOperatorCondition() { return OperatorCondition.None; }

        public virtual InputState GetInputState() { return _inputState; }

        public virtual void Press()
        {
            lastPressTime = Time.time;
        }
        public virtual void Release()
        {
            //Debug.Log("elapsed time: " + (Time.time - lastPressTime));
        }
        protected float lastPressTime;
    }
}
