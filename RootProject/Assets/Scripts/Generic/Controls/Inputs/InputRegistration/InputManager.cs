using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Freethware.Inputs
{
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
}
