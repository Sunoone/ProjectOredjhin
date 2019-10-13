using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Freethware.Inputs
{
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
    public class Input_Digital_Simulated : Input_Digital_Base
    {
        public override Input_Digital_Base Clone()
        {
            Input_Digital_Simulated newInput = new Input_Digital_Simulated();
            newInput.InputString = InputString;
            newInput.ActivationValue = ActivationValue;
            return newInput;
        }
        public override bool IsSimulated() { return true; }
        [Input_Analog]
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