using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StateMachine;

// Eventually need to reverse the loop

[System.Serializable]
[CreateAssetMenu(fileName = "FG_State", menuName = "FG/Component/State", order = 1)]
public class FG_State : SM_State {

    protected override SM_StateMachineResult LoopState(object refObject, List<InputUnit> dataSource, int dataIndex, int remainingSteps)
    {
        FG_Fighter fighter = refObject as FG_Fighter;
        return RunState(refObject, dataSource, dataIndex + fighter.StickCount + fighter.ButtonCount, remainingSteps - 1);
    }
}
