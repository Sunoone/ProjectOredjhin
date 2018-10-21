using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Freethware.Inputs
{
    [System.Serializable]
    public class Input_Digital : Input_Digital_Base
    {
        public override Input_Digital_Base Clone()
        {
            Input_Digital newInput = new Input_Digital();
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
}

