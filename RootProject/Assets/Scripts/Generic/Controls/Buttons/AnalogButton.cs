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

            newAnalog.inputStrings = new List<AnalogInput>();
            int length = inputStrings.Count;
            for (int i = 0; i < length; i++)
            {
                newAnalog.inputStrings.Add(inputStrings[i].Clone());
            }

            newAnalog.inputKeys = new List<SimulatedAnalogInput>();
            length = inputKeys.Count;
            for (int i = 0; i < length; i++)
            {
                newAnalog.inputKeys.Add((SimulatedAnalogInput)inputKeys[i].Clone());
            }

            return newAnalog;
        }
        public Button PlayerButton;   // Identification for the button. Change the enum list for every project

        public List<AnalogInput> inputStrings;       // Allows for as many keys to be assigned to this input as you want
        public List<SimulatedAnalogInput> inputKeys;

        public float GetAxis()
        {
            float value = 0f;

            int length = inputKeys.Count;
            for (int i = 0; i < length; i++)
            {
                value += inputKeys[i].GetAxis();
            }

            length = inputStrings.Count;
            for (int i = 0; i < length; i++)
            {
                value += inputStrings[i].GetAxis();
            }
            return value;
        }
    }
}
