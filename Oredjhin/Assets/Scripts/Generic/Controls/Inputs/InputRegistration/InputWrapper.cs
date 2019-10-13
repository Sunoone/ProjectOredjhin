using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Freethware.Inputs
{
    public static class InputWrapper
    {
        


        public static void ExecuteKey(KeyCode key)
        {

        }

        public static void GetKeyDown(KeyCode key)
        {
            /* length = UserInputs.Count;
            for (int i = 0; i < length; i++)
            {
                if (UserInputs[i].Key == key)
                    UserInputs[i].Input.SolveKey(key);
            }*/
        }





        /*
        // Input wrapper functionality, not input manager.
        /// Uses Xbox layout
        private static bool DisableInputs = false;

        static string[] PlayerIDs = new string[4] { "P1", "P2", "P3", "P4" };

        #region Controller_Inputs
        // Axis
        public static float MainHorizontal(int playerIndex)
        {
            float r = 0.0f;
            r += Input.GetAxis("J_MainHorizontal" + PlayerIDs[playerIndex]);
            r += Input.GetAxis("K_MainHorizontal" + PlayerIDs[playerIndex]);

            return Mathf.Clamp(r, -1, 1);
        }
        public static float MainVertical(int playerIndex)
        {
            float r = 0.0f;
            r += Input.GetAxis("J_MainVertical" + PlayerIDs[playerIndex]);
            r += Input.GetAxis("K_MainVertical" + PlayerIDs[playerIndex]);

            return Mathf.Clamp(r, -1, 1);
        }
        public static Vector3 MainJoystick(int playerIndex) { return new Vector3(MainHorizontal(playerIndex), 0, MainVertical(playerIndex)); }


        public static float SecondaryHorizontal(int playerIndex)
        {
            float r = 0.0f;
            r += Input.GetAxis("J_SecondaryHorizontal" + PlayerIDs[playerIndex]);
            //r += Input.GetAxis("K_SecondaryHorizontal" + PlayerIDs[playerIndex]);

            return Mathf.Clamp(r, -1, 1);
        }
        public static float SecondaryVertical(int playerIndex)
        {
            float r = 0.0f;
            r += Input.GetAxis("J_SecondaryVertical" + PlayerIDs[playerIndex]);
            //r += Input.GetAxis("K_SecondaryVertical" + PlayerIDs[playerIndex]);

            return Mathf.Clamp(r, -1, 1);
        }

        static bool[] PlayerSecondaryHorizontalReleased = new bool[4] { true, true, true, true };
        static bool[] PlayerSecondaryVerticalReleased = new bool[4] { true, true, true, true };

        public static bool SecondaryLeft(int playerIndex) { return SecondaryAxis(PlayerSecondaryHorizontalReleased, SecondaryHorizontal(playerIndex), -1, playerIndex); }
        public static bool SecondaryRight(int playerIndex) { return SecondaryAxis(PlayerSecondaryHorizontalReleased, SecondaryHorizontal(playerIndex), 1, playerIndex); }
        public static bool SecondaryUp(int playerIndex) { return SecondaryAxis(PlayerSecondaryVerticalReleased, SecondaryVertical(playerIndex), 1, playerIndex); }
        public static bool SecondaryDown(int playerIndex) { return SecondaryAxis(PlayerSecondaryVerticalReleased, SecondaryVertical(playerIndex), -1, playerIndex); }

        private static bool SecondaryAxis(bool[] array, float axis, int required, int playerIndex)
        {
            if (array[playerIndex])
            {
                if (axis == required)
                {
                    array[playerIndex] = false;
                    return true;
                }
            }
            else if (axis == 0)
                array[playerIndex] = true;

            return false;
        }

        public static Vector3 SecondaryJoystick(int playerIndex) { return new Vector3(SecondaryHorizontal(playerIndex), 0, SecondaryVertical(playerIndex)); }

        // Buttons
        public static bool AButton(int playerIndex) { return Input.GetButtonDown("A_Button" + PlayerIDs[playerIndex]); }
        public static bool BButton(int playerIndex) { return Input.GetButtonDown("B_Button" + PlayerIDs[playerIndex]); }
        public static bool XButton(int playerIndex) { return Input.GetButtonDown("X_Button" + PlayerIDs[playerIndex]); }
        public static bool YButton(int playerIndex) { return Input.GetButtonDown("Y_Button" + PlayerIDs[playerIndex]); }
        #endregion*/
    }


}
