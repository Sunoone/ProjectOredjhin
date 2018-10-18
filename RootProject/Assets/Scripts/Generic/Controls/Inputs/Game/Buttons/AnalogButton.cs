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
            newAnalog.InputString = InputString;
            newAnalog.PlayerButton = PlayerButton;
            return newAnalog;
        }

        public float value = 0f;
        public string InputString;
        public Button PlayerButton;
        

    }
}
