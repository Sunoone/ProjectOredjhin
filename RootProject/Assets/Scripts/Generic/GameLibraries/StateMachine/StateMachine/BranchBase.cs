﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace StateMachine
{
    public class BranchBase : ScriptableObject
    {
        public BranchBase(State destinationState)
        {
            DestinationState = destinationState;
        }
        public BranchBase(State destinationState, bool reverseInput, List<InputUnit> acceptableInputs)
        {
            DestinationState = destinationState;
        }

        public State DestinationState = default(State);

        public virtual State TryBranch(object refObject, List<InputUnit> dataSource, int dataIndex, out int outDataIndex)
        {
            outDataIndex = dataIndex;
            return DestinationState;
        }
    }
}