﻿using System.Collections;
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
    /*public enum InputState
    {
        Idle = (1 << 0), // Used when up
        Up = (1 << 1), // Used the frame it goes up
        Down = (1 << 2), // Used the frame it goes down
        Hold = (1 << 3), // Used when down
        None = (1 << 4)
    }*/

    [System.Serializable]
    public class DigitalInputBase : ICloneable<DigitalInputBase>
    {
        public virtual DigitalInputBase Clone()
        {
            DigitalInputBase newInput = new DigitalInputBase();
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




    [System.Serializable]
    public class DigitalInput : DigitalInputBase
    {
        public override DigitalInputBase Clone()
        {
            DigitalInput newInput = new DigitalInput();
            newInput.InputKey = InputKey;
            return newInput;
        }
        public KeyCode InputKey;

        public override void SetInputKey(KeyCode key) { InputKey = key; }
        public override KeyCode GetInputKey() { return InputKey; }

        public override InputState GetInputState()
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
    public class SimulatedDigitalInput : DigitalInputBase
    {
        public override DigitalInputBase Clone()
        {
            SimulatedDigitalInput newInput = new SimulatedDigitalInput();
            newInput.InputString = InputString;
            newInput.ActivationValue = ActivationValue;
            return newInput;
        }
        public override bool IsSimulated() { return true; }
        [AnalogInput]
        public string InputString;

        public float ActivationValue = .95f;
        public OperatorCondition Operator = OperatorCondition.Above;

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

