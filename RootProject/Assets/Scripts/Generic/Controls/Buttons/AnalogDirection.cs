using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Freethware.Inputs
{
   public enum AnalogAxis
    {
        Horizontal,
        Vertical,
    }
    [System.Serializable]
    public class AnalogDirection : ICloneable<AnalogDirection>
    {
        public AnalogDirection Clone()
        {
            AnalogDirection newAnalog = new AnalogDirection();
            //newAnalog.InputString = InputString;
            newAnalog.Axis = Axis;

            newAnalog.InputAxis = new List<Input_Analog>();
            int length = InputAxis.Count;
            for (int i = 0; i < length; i++)
            {
                newAnalog.InputAxis.Add((Input_Analog)InputAxis[i].Clone());
            }

            newAnalog.InputKeys = new List<Input_Analog_Simulated>();
            length = InputKeys.Count;
            for (int i = 0; i < length; i++)
            {
                newAnalog.InputKeys.Add((Input_Analog_Simulated)InputKeys[i].Clone());
            }

            return newAnalog;
        }
        public AnalogAxis Axis;   // Identification for the button. Change the enum list for every project

        public List<Input_Analog> InputAxis;       // Allows for as many keys to be assigned to this input as you want
        public List<Input_Analog_Simulated> InputKeys;

        public float GetAxis()
        {
            float value = 0f;

            int length = InputKeys.Count;
            for (int i = 0; i < length; i++)
            {
                value += InputKeys[i].GetAxis();
            }

            length = InputAxis.Count;
            for (int i = 0; i < length; i++)
            {
                value += InputAxis[i].GetAxis();
            }
            return value;
        }
    }
}
