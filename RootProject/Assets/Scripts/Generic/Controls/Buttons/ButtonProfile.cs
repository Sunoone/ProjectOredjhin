using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Freethware.Inputs
{
    public enum Button
    {
        Jump = (1 << 0),
        PowerBurn = (1 << 1),
        Block = (1 << 2),
        LightAttack = (1 << 3),
        HeavyAttack = (1 << 4),
        Special = (1 << 5),
        Pause = (1 << 6),
        Help = (1 << 7),
        Taunt = (1 << 8),
        Down = (1 << 9),
        Left = (1 << 10),
        Up = (1 << 11),
        Right = (1 << 12),
    }

    [System.Serializable]
    public class ButtonProfile : ICloneable<ButtonProfile>
    {
        public string ProfileName;

        public List<DigitalButton> DigitalButtons;      
        public List<AnalogDirection> AnalogDirections;

        public bool AllowDuplicates; // Duplicates allows the same input to be used for multiple actions.
        public bool Deletable = true;

        public Input_Digital GetCustomInputForKey(KeyCode key)
        {
            int length = DigitalButtons.Count;
            for (int i = 0; i < length; i++)
            {
                Input_Digital customInput = DigitalButtons[i].GetCustomInputForKey(key);
                if (customInput != null)
                    return customInput;
            }
            return null;
        }

        public void SaveProfile()
        {

        }

        public void LoadProfile(string profileName)
        {

        }

        public void DeleteProfile()
        {

        }


        public void RunInput()
        {
            int length = DigitalButtons.Count;
            for (int i = 0; i < length; i++)
            {
                DigitalButtons[i].RunInputs();
            }
        }

        public DigitalButton GetButton(Controls_ButtonUnit button)
        {
            int length = DigitalButtons.Count;
            for (int i = 0; i < length; i++)
            {
                if (DigitalButtons[i].Button == button)
                    return DigitalButtons[i];
            }
            return null;
        }

        public DigitalButton GetButton(string ID)
        {
            int length = DigitalButtons.Count;
            for (int i = 0; i < length; i++)
            {
                if (DigitalButtons[i].Button.ToString() == ID)
                    return DigitalButtons[i];
            }
            return null;
        }

        public InputState GetButtonState(Controls_ButtonUnit button)
        {
            int length = DigitalButtons.Count;
            for (int i = 0; i < length; i++)
            {
                if (DigitalButtons[i].Button == button)
                    return DigitalButtons[i].GetInputState();
            }
            return InputState.None;
        }

        public InputState GetButtonState(Controls_ButtonUnit button, ref float holdDuration)
        {
            int length = DigitalButtons.Count;
            for (int i = 0; i < length; i++)
            {
                if (DigitalButtons[i].Button == button)
                {
                    InputState buttonState = DigitalButtons[i].GetInputState();
                    if (buttonState == InputState.Up)
                        holdDuration = DigitalButtons[i].GetHoldDuration();
                    return buttonState;
                }           
            }
            return InputState.None;
        }

        public bool GetButtonDown(Controls_ButtonUnit button)
        {
            int length = DigitalButtons.Count;
            for (int i = 0; i < length; i++)
            {
                if (DigitalButtons[i].Button == button)
                    return DigitalButtons[i].GetButtonDown();
            }
            return false;
        }
        public bool GetButtonUp(Controls_ButtonUnit button)
        {
            int length = DigitalButtons.Count;
            for (int i = 0; i < length; i++)
            {
                if (DigitalButtons[i].Button == button)
                    return DigitalButtons[i].GetButtonUp();
            }
            return false;
        }
        public bool GetButton(Controls_ButtonUnit button, ref float duration)
        {
            int length = DigitalButtons.Count;
            for (int i = 0; i < length; i++)
            {
                if (DigitalButtons[i].Button == button)
                {
                    duration = DigitalButtons[i].GetHoldDuration();
                    return DigitalButtons[i].GetButton();
                }
            }
            return false;
        }

        // Needs support for a full analog stick.
        public Vector2 GetAxis()
        {
            Vector2 axis = Vector2.zero;
            int length = AnalogDirections.Count;
            for (int i = 0; i < length; i++)
            {
                if (AnalogDirections[i].Axis == AnalogAxis.Horizontal)
                {
                    axis.x = AnalogDirections[i].GetAxis();
                    continue;
                }
                axis.y = AnalogDirections[i].GetAxis();
            }

            // Currently disabled, but will make it easier to solve all the mixed inputs (upback, etc)
            /*length = DigitalButtons.Count;
            for (int i = 0; i < length; i++)
            {
                switch (DigitalButtons[i].Button)
                {
                    case Button.Down:
                        axis.y -= 1;
                        break;
                    case Button.Left:
                        axis.x -= 1;
                        break;
                    case Button.Up:
                        axis.y += 1;
                        break;
                    case Button.Right:
                        axis.x += 1;
                        break;
                    default:
                        break;
                }
            }*/
            axis.x = Mathf.Clamp(axis.x, -1, 1);
            axis.y = Mathf.Clamp(axis.y, -1, 1);
            return axis;
        }



        public ButtonProfile Clone()
        {
            ButtonProfile newProfile = new ButtonProfile();
            newProfile.ProfileName = "NewProfile";

            newProfile.DigitalButtons = new List<DigitalButton>();
            int length = DigitalButtons.Count;
            for (int i = 0; i < length; i++)
            {
                newProfile.DigitalButtons.Add(DigitalButtons[i].Clone());
            }

            newProfile.AnalogDirections = new List<AnalogDirection>();
            length = AnalogDirections.Count;
            for (int i = 0; i < length; i++)
            {
                newProfile.AnalogDirections.Add(AnalogDirections[i].Clone());
            }

            newProfile.AllowDuplicates = AllowDuplicates;
            return newProfile;
        }
    }
}
