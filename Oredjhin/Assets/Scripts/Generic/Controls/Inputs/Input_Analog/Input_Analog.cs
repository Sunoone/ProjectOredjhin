using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Freethware.Math;

namespace Freethware.Inputs
{
    [System.Serializable]
    public class Input_Analog : Input_Analog_Base
    {
        [Input_Analog]
        public string InputString;
        
        public override float GetAxis()
        {
            _value = Mathf.Clamp(Input.GetAxis(InputString), -1, 1);
            //Debug.Log(_value);
            return _value;
        }

        public override void SetInputString(string ID) { InputString = ID; }
        public override string GetInputString() { return InputString; }

        public override Input_Analog_Base Clone()
        {
            Input_Analog newInput = new Input_Analog();
            newInput.InputString = InputString;
            return newInput;
        }
    }
}
