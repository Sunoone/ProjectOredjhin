using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StateMachine;


[System.Serializable]
[CreateAssetMenu(fileName = "FG_State", menuName = "FG/Component/State", order = 1)]
public class FG_State : SM_State {

    // Hardcoded number: Sticks required in game




    protected override SM_StateMachineResult LoopState(object refObject, List<InputUnit> dataSource, int dataIndex, int remainingSteps)
    {       
        return RunState(refObject, dataSource, dataIndex + 1 + FG_Fighter.StickCount, remainingSteps - 1);
    }
}
