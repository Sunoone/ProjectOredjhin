using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Freethware.Inputs
{
    /// <summary>
    /// For future improvements, include an integrated update method somewhere in order to simply retrieve the current states of the button. 
    /// Because of a lack of that feature, it currently runs everything parralel, and requires both the detection for up and down to be used.
    /// </summary>


    [System.Serializable]
    public class DigitalButton : ICloneable<DigitalButton>
    {
        public DigitalButton Clone()
        {
            DigitalButton newButton = new DigitalButton();
            newButton.PlayerButton = PlayerButton;

            newButton.inputKeys = new List<DigitalInput>();
            int length = inputKeys.Count;
            for (int i = 0; i < length; i++)
            {
                newButton.inputKeys.Add(inputKeys[i].Clone());
            }

            newButton.inputStrings = new List<SimulatedDigitalInput>();
            length = inputStrings.Count;
            for (int i = 0; i < length; i++)
            {
                newButton.inputStrings.Add((SimulatedDigitalInput)inputStrings[i].Clone());
            }


            newButton.singleInput = singleInput;
            //newButton.keyCount = inputKeys.Count;
            return newButton;
        }

        public Button PlayerButton;   // Identification for the button. Change the enum list for every project
        public List<DigitalInput> inputKeys;       // Allows for as many keys to be assigned to this input as you want
        public List<SimulatedDigitalInput> inputStrings;
        public bool singleInput = true;     // When a digitalinput is already pressed and being held, it cannot return true when another digital input is being pressed or released

        protected InputState _inputState;

        public virtual bool GetButtonDown() { return _inputState == InputState.Down; }
        public virtual bool GetButtonUp() { return _inputState == InputState.Up; }
        public virtual bool GetButton() { return (_inputState == InputState.Hold || _inputState == InputState.Down); }
        public virtual InputState GetInputState() { return _inputState; }

        public virtual void RunInputs()
        {
            switch (_inputState)
            {
                case InputState.Hold:
                case InputState.Idle:
                    int length = inputKeys.Count;
                    for (int i = 0; i < length; i++)
                    {
                        if (SolveInput(inputKeys[i]))
                            return;
                    }


                    length = inputStrings.Count;
                    for (int i = 0; i < length; i++)
                    {
                        if (SolveInput(inputStrings[i]))
                            return;
                    }
                    break;
                case InputState.Up:
                    _inputState = InputState.Idle;
                    break;
                case InputState.Down:
                    _inputState = InputState.Hold;
                    break;
                default:
                    break;
            }          
        }
        protected virtual bool SolveInput(DigitalInput input)
        {
            InputState newState = input.GetInputState();
            // Now it depends on the current state and this state. So. Switch
            switch (_inputState)
            {
                case InputState.Idle:
                    if (newState == InputState.Down)
                    {
                        _inputState = InputState.Down;
                        return true;
                    }
                    break;
                case InputState.Up:
                    break;
                case InputState.Down:
                    break;
                case InputState.Hold:
                    bool otherConditions = false;
                    if (newState == InputState.Up)
                    {
                        // As long as no other input objects.
                        if (singleInput)
                        {
                            int length = inputKeys.Count;
                            for (int i = 0; i < length; i++)
                            {
                                if (inputKeys[i] == input)
                                    continue;

                                if (inputKeys[i].GetInputState() == InputState.Hold)
                                {
                                    otherConditions = true;                         // Another key is already being pressed.
                                }
                            }

                            length = inputStrings.Count;
                            for (int i = 0; i < length; i++)
                            {
                                if (inputStrings[i] == input)
                                    continue;

                                if (inputStrings[i].GetInputState() == InputState.Hold)
                                {
                                    otherConditions = true;                         // Another key is already being pressed.
                                }
                            }
                        }
                        if (!otherConditions)
                        {
                            _inputState = InputState.Up;
                            return true;
                        }
                    }
                    break;
                default:
                    break;
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
