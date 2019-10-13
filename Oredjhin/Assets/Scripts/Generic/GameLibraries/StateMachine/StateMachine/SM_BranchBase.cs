using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace StateMachine
{
    public class SM_BranchBase : ScriptableObject
    {
        public SM_State DestinationState = default(SM_State);
        public SM_BranchBase(SM_State destinationState) { DestinationState = destinationState; }
        public SM_BranchBase(SM_State destinationState, bool reverseInput, List<InputUnit> acceptableInputs) { DestinationState = destinationState; }

        public virtual SM_State TryBranch(object refObject, List<InputUnit> dataSource, int dataIndex, out int outDataIndex)
        {
            outDataIndex = dataIndex;
            return DestinationState;
        }
    }
}