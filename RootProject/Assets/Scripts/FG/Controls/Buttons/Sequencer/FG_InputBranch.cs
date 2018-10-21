using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StateMachine;
using Freethware.Inputs;

[CreateAssetMenu(fileName = "FG_Branch", menuName = "FG/Branch", order = 1)]
public class FG_InputBranch : SM_BranchBase {

    public FG_InputBranch(SM_State destinationState) : base(destinationState)
    {
        DestinationState = destinationState;
    }
    public FG_InputBranch(SM_State destinationState, bool reverseInput, List<InputUnit> acceptableInputs) : base(destinationState, reverseInput, acceptableInputs)
    {
        DestinationState = destinationState;
    }

    public override SM_State TryBranch(object refObject, List<InputUnit> dataSource, int dataIndex, out int outDataIndex)
    {
        outDataIndex = dataIndex;

        if (RequiredButtons == (ForbiddenButtons & RequiredButtons))
        {
            Debug.LogError("Error: Required button is also forbidden.");
            return null;
        }

        if (dataIndex + FG_Fighter.ButtonCount >= dataSource.Count)
        {
            Debug.LogError("Error: Not enough data.");
            return null;
        }

        Controls_DirectionUnit directionUnit = dataSource[dataIndex] as Controls_DirectionUnit;
        if (directionUnit != null )
        {
            int input = (int)directionUnit.Direction;
            int accepted = (int)AcceptableDirections;
            if (accepted != -1 && input != (accepted & input))
            {
                return null;
            }
        }
        else
        {
            Debug.LogError("Error: No directional input at index " + dataIndex);
            return null;
        }

        outDataIndex++;

        bool bRequiredButtonPressed = false;
        for (int i = 0; i < FG_Fighter.ButtonCount; i++, outDataIndex++)
        {
            Controls_ButtonUnit buttonUnit = dataSource[outDataIndex] as Controls_ButtonUnit;
            if (buttonUnit == null)
                continue;

            Debug.Log("Button: " + (1 << i) + ", count: " + FG_Fighter.ButtonCount);
            if (RequiredButtons == (RequiredButtons & (1<<i)))
            { 
                if (buttonUnit.InputState == InputState.Down)
                {
                    bRequiredButtonPressed = true;
                    continue;
                }
                else if (buttonUnit.InputState == InputState.Hold)
                {
                    continue;
                }
                return null;
            }
            else if(i == (ForbiddenButtons & (1<<i)))
            {
                if (buttonUnit.InputState != InputState.Down)
                {
                    continue;
                }
                return null;
            }
            else
            {
                Debug.LogError("Error: Expected " + FG_Fighter.ButtonCount + " button inputs, only found " + i);
                return null;
            }
        }
        if (RequiredButtons != 0 && !bRequiredButtonPressed)
            return null;

        return DestinationState;
    }

    [Controls_ButtonUnitFlag] public int RequiredButtons;
    [Controls_ButtonUnitFlag] public int ForbiddenButtons;
    [EnumFlags] public Direction AcceptableDirections;
}
