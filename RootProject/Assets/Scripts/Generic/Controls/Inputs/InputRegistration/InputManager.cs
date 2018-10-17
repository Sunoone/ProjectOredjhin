using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Freethware.Inputs
{

    public enum PlayerButton
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


    public class UserInputs
    {
        public PlayerButton Button;
        public DigitalButton ActionInput;
        public DigitalInput Input;
        public AnalogButton AnalogInput;

        public Callback InputCB;
    }

    public class InputManager : MonoBehaviour
    {
        public List<ButtonProfile> Profiles;
        bool showMenu = true;

        // Profile name. And load list with profile names. Also, be able to delete it.

        [EasyButtons.Button("CreateDefaultProfile", EasyButtons.ButtonMode.AlwaysEnabled)]
        public ButtonProfile CreateDefaultProfile()
        {
            return CreateDefaultButtonProfile(0);
        }


        public ButtonProfile CreateDefaultButtonProfile(int index)
        {
            ButtonProfile newProfile = DefaultProfiles[index].Clone();
            Profiles.Add(newProfile);
            return newProfile;
        }


        private void Reset2Defaults(ButtonProfile profile, int index)
        {
            profile = DefaultProfiles[index].Clone();
        }

        public void SaveInputs()
        {

        }

        public void LoadInputs()
        {

        }

        public ButtonProfile[] DefaultProfiles;
    }

    [System.Serializable]
    public class ButtonProfile : ICloneable<ButtonProfile>
    {
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


        public string ProfileName;

        public Entity User;
        

        public List<DigitalButton> DigitalInputs;
        public List<AnalogButton> AnalogInputs;
        public bool AllowDuplicates;
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

        public bool GetInputDown(PlayerButton button)
        {
            int length = DigitalInputs.Count;
            for (int i = 0; i < length; i++)
            {
                if (DigitalInputs[i].PlayerButton == button)
                    return DigitalInputs[i].GetInputDown();
            }
            return false;
        }
        public bool GetInputUp(PlayerButton button)
        {
            int length = DigitalInputs.Count;
            for (int i = 0; i < length; i++)
            {
                if (DigitalInputs[i].PlayerButton == button)
                    return DigitalInputs[i].GetInputUp();
            }
            return false;
        }
        public bool GetInput(PlayerButton button)
        {
            int length = DigitalInputs.Count;
            for (int i = 0; i < length; i++)
            {
                if (DigitalInputs[i].PlayerButton == button)
                    return DigitalInputs[i].GetInput();
            }
            return false;
        }
    }

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

        public PlayerButton PlayerButton;   // Identification for the button. Change the enum list for every project
        public string InputString;          // String used for controllers to gain access to Unity's InputManager.asset
        public List<DigitalInput> inputKeys;       // Allows for as many keys to be assigned to this input as you want
        public bool singleInput = true;     // When a digitalinput is already pressed and being held, it cannot return true when another digital input is being pressed or released

        public int keyCount = 0;

        public void Init()
        {
            keyCount = inputKeys.Count;
        }

        bool OldPressed = false;
        bool Pressed = false;
        public bool GetInputDown()
        {
            for (int i = 0; i < keyCount; i++)
            {
                if (GetInputDown(inputKeys[i].Key))
                {
                    Debug.Log(PlayerButton.ToString() + " got pressed");
                    return true;
                }
                    
            }
            return false;
        }
        public bool GetInputUp()
        {
            for (int i = 0; i < keyCount; i++)
            {
                if (GetInputUp(inputKeys[i].Key))
                {
                    Debug.Log(PlayerButton.ToString() + " got released");
                    return true;
                }
            }
            return false;
        }
        public bool GetInput()
        {
            for (int i = 0; i < keyCount; i++)
            {
                if (Input.GetKey(inputKeys[i].Key))
                    return true;
            }
            return false;
        }


        public bool GetInputDown(KeyCode key) { return GetInput(key, Input.GetKeyDown(key), true); }
        public bool GetInputUp(KeyCode key) { return GetInput(key, Input.GetKeyUp(key), false); }

        private bool GetInput(KeyCode key, bool condition, bool newPressedState)
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
                        tempKey = inputKeys[i].Key;

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

        public DigitalInput GetCustomInputForKey(KeyCode key)
        {
            int length = inputKeys.Count;
            for (int i = 0; i < length; i++)
            {
                if (inputKeys[i].Key == key)
                    return inputKeys[i];
            }
            return null;
        }
        
    }



    [System.Serializable]
    public class DigitalInput : ICloneable<DigitalInput>
    {
        public DigitalInput Clone()
        {
            DigitalInput newInput = new DigitalInput();
            newInput.Key = Key;
            newInput.NegativeEdge = NegativeEdge;
            newInput.simulated = simulated;
            return newInput;
        }

        protected bool Pressed = false;
        public KeyCode Key;
        private bool simulated = false;

        public void Switch(bool condition)
        {
            Pressed = condition;
            if (Pressed)
            {
                Press();
                return;
            }
            Release();
        }

        public void Press()
        {
            lastPressTime = Time.time;
        }

        public void Release()
        {
            //Debug.Log("elapsed time: " + (Time.time - lastPressTime));
        }

        private float NegativeEdge = 1f;
        private float lastPressTime;
    }
    [System.Serializable]
    public class SimulatedDigitalInput
    {

    }

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
        public PlayerButton PlayerButton;
        // Needs to take into account player count for the controller. (For Default)

    }

    [System.Serializable]
    public class SimulatedAnalogButton : AnalogButton
    {

        /*public float analogFeel_up = 0;         // analog feel values
        public float analogFeel_down = 0;       // use these values if you want to simulate a virtual analog axis
        public float analogFeel_left = 0;
        public float analogFeel_right = 0;
        public float analogFeel_jump = 0;

        public float analogFeel_gravity = 0.2f; // how fast do we slow down after we release the button 
        public float analogFeel_sensitivity = 0.8f; // how fast do we speed up when we start pressing a button
        */
        //public UnityEvent KeyChanged;
    }

}
