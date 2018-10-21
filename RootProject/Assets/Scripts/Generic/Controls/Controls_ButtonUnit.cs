using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StateMachine;

namespace Freethware.Inputs
{
    [System.Serializable]
    [CreateAssetMenu(fileName = "ButtonUnit", menuName = "Controls/ButtonUnit", order = 1)]
    public class Controls_ButtonUnit : Controls_InputUnit
    {
        [Controls_ButtonUnit]
        public int ButtonIndex;
        public InputState InputState;

        public override float GetFloatValue()
        {
            return ButtonIndex;
        }
    }
}

