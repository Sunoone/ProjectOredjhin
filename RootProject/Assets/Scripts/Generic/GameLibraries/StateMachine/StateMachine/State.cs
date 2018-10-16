using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StateMachine;

namespace StateMachine
{
    public enum EStateMachineCompletionType
    {
        NotAccepted, // Implicit failure - this state is not marked as Accept.      
        Accepted, // Success - this state is an Accept state.      
        Rejected, // Explicit failure - this state is specifically marked as failure/not-accept.      
        OutOfSteps, // Our simulation ran out of steps while the machine was still running.
    }

    [System.Serializable]
    public struct StateMachineResult
    {
        public EStateMachineCompletionType CompletionType;
        public State FinalState;
        public int DataIndex;
    }

    [System.Serializable]
    [CreateAssetMenu(fileName = "QuestState", menuName = "Quests/Component/State", order = 1)]
    public class State : ScriptableObject
    {
        /* Attempt to run the state with input from the given DataSource object.
         * Will start reading input at DataIndex.
         * Will decrement RemainingSteps and automatically fail after it hits 0. */

        public virtual StateMachineResult RunState(object refObject, List<InputUnit> dataSource, int dataIndex = 0, int remainingSteps = -1)
        {
            bool bMustEndNow = (TerminateImmediately || !dataSource.IsValidIndex(dataIndex));

            // If we're still running, see where our branches lead.
            if (remainingSteps != 0 && !bMustEndNow)
            {
                State DestinationState;
                int destinationDataIndex = dataIndex;

                int length = InstancedBranches.Count;
                for (int i = 0; i < length; i++)
                {
                    if (InstancedBranches[i] != null)
                    {
                        DestinationState = InstancedBranches[i].TryBranch(refObject, dataSource, dataIndex, out destinationDataIndex);
                        if (DestinationState != null)
                        {
                            return DestinationState.RunState(refObject, dataSource, destinationDataIndex, remainingSteps - 1);
                        }
                    }
                }
                length = SharedBranches.Count;
                for (int i = 0; i < length; i++)
                {
                    if (SharedBranches[i] != null)
                    {
                        DestinationState = SharedBranches[i].TryBranch(refObject, dataSource, dataIndex, out destinationDataIndex);
                        if (DestinationState != null)
                        {
                            return DestinationState.RunState(refObject, dataSource, destinationDataIndex, remainingSteps - 1);
                        }
                    }
                }
                // We didn't find any acceptable branch, so we must end on this state unless we're told to loop.
                if (LoopByDefault)
                {
                    return LoopState(refObject, dataSource, dataIndex, remainingSteps);
                }
                bMustEndNow = true;
            }
            StateMachineResult SMR;
            SMR.FinalState = this;
            SMR.DataIndex = dataIndex;
            SMR.CompletionType = bMustEndNow ? CompletionType : EStateMachineCompletionType.OutOfSteps;
            return SMR;
        }
        protected virtual StateMachineResult LoopState(object refObject, List<InputUnit> dataSource, int dataIndex, int remainingSteps)
        {
            return RunState(refObject, dataSource, dataIndex + 1, remainingSteps - 1);
        }

        public EStateMachineCompletionType CompletionType; // IF input runs out on this state (or TerminateImmediately is true), this is how the result will be interpreted.   
        public bool TerminateImmediately = false; // A state machine run that enters this state will terminate immediately, regardless of whether or not there is more input data.
        public bool LoopByDefault = true; // If this is set, this state will loop on itself whenever an unhandled input unit is detected.
        public List<Branch> InstancedBranches; // Branches to other states. These are in priority order, so first successful branch will be taken.
        public List<Branch> SharedBranches; // Branches to other states. These are in priority order, so first successful branch will be taken.
    }
}