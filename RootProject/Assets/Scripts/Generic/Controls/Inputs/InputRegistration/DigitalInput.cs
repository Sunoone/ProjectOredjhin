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
    public class DigitalInput : ICloneable<DigitalInput>
    {
        public virtual DigitalInput Clone()
        {
            DigitalInput newInput = new DigitalInput();
            newInput.InputKey = InputKey;
            return newInput;
        }
        public virtual bool IsSimulated() { return false; }

        protected InputState _inputState;

        public KeyCode InputKey;

        public virtual void SetInputKey(KeyCode key) { InputKey = key; }
        public virtual KeyCode GetInputKey() { return InputKey; }

        public virtual void SetInputString(string ID) { }
        public virtual string GetInputString() { return ""; }
        public virtual void SetActivationValue(float value) { }
        public virtual float GetActivationValue() { return 0f; }
        public virtual void SetOperatorCondition(OperatorCondition condition) { }
        public virtual OperatorCondition GetOperatorCondition() { return OperatorCondition.None; }

        public virtual InputState GetInputState()
        {
            if (Input.GetKeyDown(InputKey))
            {
                Press();
                _inputState = InputState.Down;
                return _inputState;
            }

            if (Input.GetKeyUp(InputKey))
            {
                Release();
                _inputState = InputState.Up;
                return _inputState;
            }

            bool newState = Input.GetKey(InputKey);
            if (newState)
                _inputState = InputState.Hold;
            else
                _inputState = InputState.Idle;
            return _inputState;
        }

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

    public enum OperatorCondition
    {
        None,
        Match,
        Above,
        Below,
        MatchAndAbove,
        MatchAndBelow,
        DoesNotMatch
    }

    [System.Serializable]
    public class SimulatedDigitalInput : DigitalInput
    {
        public override DigitalInput Clone()
        {
            SimulatedDigitalInput newInput = new SimulatedDigitalInput();
            newInput.InputString = InputString;
            newInput.ActivationValue = ActivationValue;
            return newInput;
        }
        public override bool IsSimulated() { return true; }

        public string InputString;
        public float ActivationValue = .95f;
        public OperatorCondition Operator = OperatorCondition.Above;

        public override void SetInputKey(KeyCode key) {}
        public override KeyCode GetInputKey() { return KeyCode.None; }

        public override void SetInputString(string ID) { InputString = ID; }
        public override string GetInputString() { return InputString; }
        public override void SetActivationValue(float value) { ActivationValue = value; }
        public override float GetActivationValue() { return ActivationValue; }
        public override void SetOperatorCondition(OperatorCondition condition) { Operator = condition; }
        public override OperatorCondition GetOperatorCondition() { return Operator; }

        private InputState lastState;

        public override InputState GetInputState()
        {
            bool condition = false;
            switch (Operator)
            {
                case OperatorCondition.Match:
                    condition = (Input.GetAxis(InputString) == ActivationValue);
                    break;
                case OperatorCondition.Above:
                    condition = (Input.GetAxis(InputString) > ActivationValue);
                    break;
                case OperatorCondition.Below:
                    condition = (Input.GetAxis(InputString) < ActivationValue);
                    break;
                case OperatorCondition.MatchAndAbove:
                    condition = (Input.GetAxis(InputString) >= ActivationValue);
                    break;
                case OperatorCondition.MatchAndBelow:
                    condition = (Input.GetAxis(InputString) <= ActivationValue);
                    break;
                case OperatorCondition.DoesNotMatch:
                    condition = (Input.GetAxis(InputString) != ActivationValue);
                    break;
                default:
                    break;
            }
            //Debug.Log("REACHING");
            if (condition)
            {
                if (_inputState != InputState.Down && _inputState != InputState.Hold)
                {
                    Press();           
                    _inputState = InputState.Down;
                    return _inputState;
                }
                _inputState = InputState.Hold;
                return _inputState;
                // Check if first frame, if so, down.
                // Not first frame, Hold.
            }
            if (_inputState != InputState.Up && _inputState != InputState.Idle)
            {
                Release();            
                _inputState = InputState.Up;
                return _inputState;
            }
            _inputState = InputState.Idle;
            return _inputState;
            // Check if first frame, if so, down.
            // Not first frame, Idle.
        }
    }
}

