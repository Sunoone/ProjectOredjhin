using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Freethware.Inputs
{
   
    [System.Serializable]
    public class AnalogButton : ICloneable<AnalogButton>
    {
        public AnalogButton Clone()
        {
            AnalogButton newAnalog = new AnalogButton();
            //newAnalog.InputString = InputString;
            newAnalog.PlayerButton = PlayerButton;

            newAnalog.InputStrings = new List<AnalogInput>();
            int length = InputStrings.Count;
            for (int i = 0; i < length; i++)
            {
                newAnalog.InputStrings.Add(InputStrings[i].Clone());
            }

            newAnalog.InputKeys = new List<SimulatedAnalogInput>();
            length = InputKeys.Count;
            for (int i = 0; i < length; i++)
            {
                newAnalog.InputKeys.Add((SimulatedAnalogInput)InputKeys[i].Clone());
            }

            return newAnalog;
        }
        public Button PlayerButton;   // Identification for the button. Change the enum list for every project

        public List<AnalogInput> InputStrings;       // Allows for as many keys to be assigned to this input as you want
        public List<SimulatedAnalogInput> InputKeys;

        public float GetAxis()
        {
            float value = 0f;

            int length = InputKeys.Count;
            for (int i = 0; i < length; i++)
            {
                value += InputKeys[i].GetAxis();
            }

            length = InputStrings.Count;
            for (int i = 0; i < length; i++)
            {
                value += InputStrings[i].GetAxis();
            }
            return value;
        }
    }
}
