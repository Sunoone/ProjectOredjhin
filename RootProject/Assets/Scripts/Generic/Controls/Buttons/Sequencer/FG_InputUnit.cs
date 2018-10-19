using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StateMachine;
using Freethware.Inputs;

[System.Serializable]
[CreateAssetMenu(fileName = "FG_InputUnit", menuName = "FG/Component/InputUnit", order = 1)]
public class FG_InputUnit : InputUnit {

    public InputDirection InputDirection;
    public InputState InputState;
}
