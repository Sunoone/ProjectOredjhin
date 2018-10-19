using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StateMachine;
using Freethware.Inputs;

[CreateAssetMenu(fileName = "FG_Branch", menuName = "FG/Branch", order = 1)]
public class FG_InputBranch : BranchBase {

    public FG_InputBranch(State destinationState) : base(destinationState)
    {
        DestinationState = destinationState;
    }
    public FG_InputBranch(State destinationState, bool reverseInput, List<InputUnit> acceptableInputs) : base(destinationState, reverseInput, acceptableInputs)
    {
        DestinationState = destinationState;
    }

    public override State TryBranch(object refObject, List<InputUnit> dataSource, int dataIndex, out int outDataIndex)
    {
        outDataIndex = dataIndex;
        return null;
        //return base.TryBranch(refObject, dataSource, dataIndex, out outDataIndex);
    }

    [EnumFlags] public Button RequiredButtons;
    [EnumFlags] public Button ForbiddenButtons;
    [EnumFlags] public InputDirection AcceptableDirections;
}
