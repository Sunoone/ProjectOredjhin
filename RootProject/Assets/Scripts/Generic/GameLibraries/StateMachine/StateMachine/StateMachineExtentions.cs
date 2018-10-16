using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StateMachine;

public static class StateMachineExtentions  {
    public static int ContainsState(this List<Branch> list, State state)
    {
        int length = list.Count;
        for (int i = 0; i < length; i++)
        {
            if (list[i].DestinationState == state)
            {
                return i;
            }
        }
        return -1;
    }

    /// <summary>
    /// Returns all the States and associated UnitInputs as their own branches.
    /// </summary>
    /// <returns></returns>
    public static List<Branch> GetTrackerInfo(this State state, List<Branch> currentList = null)
    {
        // Avoids NullReferenceException
        if (currentList == null)
            currentList = new List<Branch>();

        List<Branch> branchList = currentList;

        // Puts itself as a branch. We have to work backwards in order to associate the right inputs with the right states.
        if (currentList.ContainsState(state) == -1)
            branchList.Add(new Branch(state));

        if (state.InstancedBranches == null)
            state.InstancedBranches = new List<Branch>();

        int length = state.InstancedBranches.Count;
        for (int i = 0; i < length; i++)
        {
            Branch branch = state.InstancedBranches[i];
            int index = currentList.ContainsState(state.InstancedBranches[i].DestinationState);
            if (index != -1)
            {
                if (branch.ReverseInput != currentList[index].ReverseInput)
                {
                    Debug.LogError("Cannot combine white and blacklists with each other.");
                    Debug.LogError("Change either " + branch + " or " + currentList[index]);
                    return null;
                }
                int length2 = state.InstancedBranches[i].AcceptableInputs.Count;
                for (int e = 0; e < length2; e++)
                {
                    var newInput = state.InstancedBranches[i].AcceptableInputs[e];
                    var currentInputs = currentList[index].AcceptableInputs;
                    if (!currentInputs.Contains(newInput))
                        currentInputs.Add(newInput);
                }
            }
            else
            {
                Branch newBranch = new Branch(state.InstancedBranches[i].DestinationState);
                newBranch.AcceptableInputs.AddRange(state.InstancedBranches[i].AcceptableInputs);
                newBranch.ReverseInput = state.InstancedBranches[i].ReverseInput;
                branchList.Add(newBranch);

                branchList = state.InstancedBranches[i].DestinationState.GetTrackerInfo(branchList);
            }
        }
        return branchList;
    }
}
