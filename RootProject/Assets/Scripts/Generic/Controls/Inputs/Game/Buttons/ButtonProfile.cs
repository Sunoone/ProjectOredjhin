using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Freethware.Inputs
{
    public enum Button
    {
        AxisHorizontal,
        AxisVertical,
        Jump,
        PowerBurn,
        Block,
        LightAttack,
        HeavyAttack,
        Special,
        Pause,
        Help,
        Taunt
        /*AxisLeft_Left,
        AxisLeft_Right,
        AxisLeft_Up,
        AxisLeft_Down,
        AxisLeft_Horizontal,
        AxisLeft_Vertical,

        AxisRight_Left,
        AxisRight_Right,
        AxisRight_Up,
        AxisRight_Down,
        AxisRight_Horizontal,
        AxisRight_Vertical,

        DPad_Left,
        DPad_Right,
        DPad_Up,
        DPad_Down,

        Button1, // Square/X
        Button2, // Triangle/Y
        Button3, // X/B
        Button4, // Circle/A

        LeftShoulder,
        LeftTrigger,

        RightShould,
        RightTrigger*/
    }

    [System.Serializable]
    public class ButtonProfile : ICloneable<ButtonProfile>
    {
        public string ProfileName;

        public List<DigitalButton> DigitalInputs;
        public List<AnalogButton> AnalogInputs;

        public bool AllowDuplicates; // Duplicates allows the same input to be used for multiple actions.
        public bool Deletable = true;


        public DigitalInput GetCustomInputForKey(KeyCode key)
        {
            int length = DigitalInputs.Count;
            for (int i = 0; i < length; i++)
            {
                DigitalInput customInput = DigitalInputs[i].GetCustomInputForKey(key);
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

        public bool GetInputDown(Button button)
        {
            int length = DigitalInputs.Count;
            for (int i = 0; i < length; i++)
            {
                if (DigitalInputs[i].PlayerButton == button)
                    return DigitalInputs[i].GetInputDown();
            }
            return false;
        }
        public bool GetInputUp(Button button)
        {
            int length = DigitalInputs.Count;
            for (int i = 0; i < length; i++)
            {
                if (DigitalInputs[i].PlayerButton == button)
                    return DigitalInputs[i].GetInputUp();
            }
            return false;
        }
        public bool GetInput(Button button)
        {
            int length = DigitalInputs.Count;
            for (int i = 0; i < length; i++)
            {
                if (DigitalInputs[i].PlayerButton == button)
                    return DigitalInputs[i].GetInput();
            }
            return false;
        }

        public ButtonProfile Clone()
        {
            ButtonProfile newProfile = new ButtonProfile();
            newProfile.ProfileName = "NewProfile";

            newProfile.DigitalInputs = new List<DigitalButton>();
            int length = DigitalInputs.Count;
            for (int i = 0; i < length; i++)
            {
                newProfile.DigitalInputs.Add(DigitalInputs[i].Clone());
            }

            newProfile.AnalogInputs = new List<AnalogButton>();
            length = AnalogInputs.Count;
            for (int i = 0; i < length; i++)
            {
                newProfile.AnalogInputs.Add(AnalogInputs[i].Clone());
            }

            newProfile.AllowDuplicates = AllowDuplicates;
            return newProfile;
        }
    }
}
