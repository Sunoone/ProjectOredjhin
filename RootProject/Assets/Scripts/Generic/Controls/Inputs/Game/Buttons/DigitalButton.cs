using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Freethware.Inputs
{
    [System.Serializable]
    public class DigitalButton : ICloneable<DigitalButton>
    {
        public DigitalButton Clone()
        {
            DigitalButton newButton = new DigitalButton();
            newButton.PlayerButton = PlayerButton;
            newButton.InputString = InputString;

            newButton.inputKeys = new List<DigitalInput>();
            int length = inputKeys.Count;
            for (int i = 0; i < length; i++)
            {
                newButton.inputKeys.Add(inputKeys[i].Clone());
            }

            newButton.singleInput = singleInput;
            newButton.keyCount = inputKeys.Count;
            return newButton;
        }

        public Button PlayerButton;   // Identification for the button. Change the enum list for every project
        public string InputString;          // String used for controllers to gain access to Unity's InputManager.asset

        public List<DigitalInput> inputKeys;       // Allows for as many keys to be assigned to this input as you want
        public bool singleInput = true;     // When a digitalinput is already pressed and being held, it cannot return true when another digital input is being pressed or released

        public int keyCount = 0;

        public void Init()
        {
            keyCount = inputKeys.Count;
            // Needs a chec to see whether an input has already been used...? No, that is solved with the actual UI implementation. 
        }

        bool OldPressed = false;
        bool Pressed = false;
        public virtual bool GetInputDown()
        {
            for (int i = 0; i < keyCount; i++)
            {
                if (GetInputDown(inputKeys[i].InputKey))
                {
                    Debug.Log(PlayerButton.ToString() + " got pressed");
                    return true;
                }

            }
            return false;
        }
        public virtual bool GetInputUp()
        {
            for (int i = 0; i < keyCount; i++)
            {
                if (GetInputUp(inputKeys[i].InputKey))
                {
                    Debug.Log(PlayerButton.ToString() + " got released");
                    return true;
                }
            }
            return false;
        }
        public virtual bool GetInput()
        {
            for (int i = 0; i < keyCount; i++)
            {
                if (Input.GetKey(inputKeys[i].InputKey))
                    return true;
            }
            return false;
        }


        public virtual bool GetInputDown(KeyCode key) { return GetInput(key, Input.GetKeyDown(key), true); }
        public virtual bool GetInputUp(KeyCode key) { return GetInput(key, Input.GetKeyUp(key), false); }

        protected virtual bool GetInput(KeyCode key, bool condition, bool newPressedState)
        {
            if (condition)
            {
                bool otherConditions = false;
                KeyCode tempKey;
                DigitalInput tempInput = null;
                if (singleInput)
                {
                    for (int i = 0; i < keyCount; i++)
                    {
                        tempKey = inputKeys[i].InputKey;

                        if (key == tempKey)
                        {
                            tempInput = inputKeys[i];  //Found same key and it is being pressed.
                            continue;
                        }
                        if (Input.GetKey(tempKey))
                        {
                            otherConditions = true;                         // Another key is already being pressed.
                        }
                    }

                    if (tempInput == null)
                        return false;
                }

                if (!otherConditions)
                {
                    Pressed = newPressedState;
                    if (!singleInput || Pressed != OldPressed)
                    {
                        OldPressed = Pressed;
                        return true;
                    }
                }
                tempInput.Switch(condition);
            }
            return false;
        }

        public virtual DigitalInput GetCustomInputForKey(KeyCode key)
        {
            int length = inputKeys.Count;
            for (int i = 0; i < length; i++)
            {
                if (inputKeys[i].InputKey == key)
                    return inputKeys[i];
            }
            return null;
        }
    }
}
