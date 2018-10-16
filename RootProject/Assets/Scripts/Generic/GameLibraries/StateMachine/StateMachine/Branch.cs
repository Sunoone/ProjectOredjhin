using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace StateMachine
{
    [System.Serializable]
    //[CreateAssetMenu(fileName = "Branch", menuName = "Quests/Branch", order = 1)]
    public class Branch// : ScriptableObject
    {
        public Branch(State destinationState)
        {
            DestinationState = destinationState;
        }
        public Branch(State destinationState, bool reverseInput, List<InputUnit> acceptableInputs)
        {
            DestinationState = destinationState;
            ReverseInput = reverseInput;
            AcceptableInputs = acceptableInputs;
        }
        // Returns Destination State on success, NULL on failure. For subclacces, OutDataIndex might be something other than 1, if a branch is made to consume multiple inputs.

        // RefObject should probably a generic type.
        public virtual State TryBranch(object refObject, List<InputUnit> dataSource, int dataIndex, out int outDataIndex)
        {
            outDataIndex = dataIndex + 1;
            if (dataSource.IsValidIndex(dataIndex) && AcceptableInputs.Contains(dataSource[dataIndex]))
            {
                return ReverseInput ? null : DestinationState;
            }
            return ReverseInput ? DestinationState : null;
        }

        public State DestinationState = default(State);
        public bool ReverseInput = false;   // If true, the meaning of AcceptableInputs is reversed.
        [SerializeField]
        public List<InputUnit> AcceptableInputs = new List<InputUnit>(); // Acceptable inputs. The current input unit must be on this list.
    }
}

