using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StateMachine;
using Freethware.Inputs;

[CreateAssetMenu(fileName = "FG_Move", menuName = "FG/Moves/Move", order = 1)]
public class FG_Move : ScriptableObject {

    public string MoveName;
    public Animation PlaceHolder;

    public virtual FG_MoveLinkToFollow TryLinks(FG_Fighter refFighter, List<InputUnit> dataSource, int dataIndex = 0, int remainingSteps = -1)//, int dataIndex, int remainingSteps)
    {
        FG_MoveLinkToFollow ResultLink = new FG_MoveLinkToFollow();

        int length = LinkedMoves.Count;
        for (int i = 0; i < length; i++)
        {
            ResultLink.SMR = LinkedMoves[i].TryLink(refFighter, dataSource, dataIndex, remainingSteps);
            if (ResultLink.SMR.CompletionType == EStateMachineCompletionType.Accepted)
            {
                ResultLink.Link = LinkedMoves[i];
                return ResultLink;
            }
        }
        return null;
    }

    public bool ClearInputOnEntry;
    public bool ClearInputOnExit;

    public List<FG_MoveLink> LinkedMoves;
}



public class FG_MoveLinkToFollow
{
    public SM_StateMachineResult SMR;
    public FG_MoveLink Link;
}

