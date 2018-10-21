using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StateMachine;
[CreateAssetMenu(fileName = "FG_Move", menuName = "FG/Moves/Link", order = 1)]
public class FG_MoveLink : ScriptableObject
{
    //public FG_State
    public FG_State InputStateMachine;

    public SM_StateMachineResult StateMachineResult;
    public FG_Move Move;

    public List<Vector2> CancelWindows;

    public bool ClearInput = true;


    public SM_StateMachineResult TryLink(FG_Fighter refFighter, List<InputUnit> dataSource, int dataIndex, int remainingSteps)
    {
        if (InputStateMachine != null && Move != null)
        {
            bool bCanCancel = false;
            int length = CancelWindows.Count;
            for (int i = 0; i < length; i++)
            {
                if (refFighter.GetTimeInMove() == Mathf.Clamp(refFighter.GetTimeInMove(), CancelWindows[i].x, CancelWindows[i].y))
                {
                    bCanCancel = true;
                    break;
                }
            }

            if (bCanCancel || CancelWindows.Count == 0)
            {
                return InputStateMachine.RunState(refFighter, dataSource, dataIndex, remainingSteps);
            }
        }
        return StateMachineResult;
    }
}