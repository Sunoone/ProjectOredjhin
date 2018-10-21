using UnityEditor;
using UnityEngine;
using System;
using StateMachine;
using System.Collections.Generic;

[InitializeOnLoad]
public class Startup
{
    static Startup()
    {
        Debug.Log("Up and running");
        UtilitiesEditor.GetAllInstances<InputUnit>();
        UtilitiesEditor.GetAllInstances<SM_State>();
    }
}


[CanEditMultipleObjects]
[CustomEditor(typeof(SM_State), true)]
public class StateEditor : Editor
{

    

    //SerializedObject State;

    SerializedProperty completionType;
    SerializedProperty teminateImmediately;
    SerializedProperty loopByDefault;

    //SerializedProperty InstancedBranches;

    SM_State state = null;

    static bool showTileEditor = false;
    float boxSize = 65f;
    public void OnEnable()
    {
        if (!init) ;
            Init();
    }

    static bool init = false;
    private void Init()
    {
        state = (SM_State)target;

        completionType = serializedObject.FindProperty("CompletionType");
        teminateImmediately = serializedObject.FindProperty("TerminateImmediately");
        loopByDefault = serializedObject.FindProperty("LoopByDefault");
        init = true;
    }

    bool CustomInspector = false;

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        if (!init)
        Init();

        if (CustomInspector)
        {
            ShowDefaultFields();
            ShowBranches();
        }
        else
            DrawDefaultInspector();

        serializedObject.ApplyModifiedProperties();
        if (GUI.changed)
            EditorUtility.SetDirty(target);
    }

    private void ShowDefaultFields()
    {
        EditorGUILayout.PropertyField(completionType, new GUIContent("CompletionType"), GUILayout.Height(20));
        EditorGUILayout.PropertyField(teminateImmediately, new GUIContent("teminateImmediately"), GUILayout.Height(20));
        EditorGUILayout.PropertyField(loopByDefault, new GUIContent("loopByDefault"), GUILayout.Height(20));
    }
    
    private void ShowBranches() {
        if (state.InstancedBranches == null)
            state.InstancedBranches = new List<SM_BranchBase>(0);


        int length = state.InstancedBranches.Count;
        for (int i = 0; i < length; i++)
        {
            int newLength = state.InstancedBranches.Count;
            if (length != newLength)
                break;

            GUILayout.BeginVertical("Box");
            SM_BranchBase branchBase = state.InstancedBranches[i];
            branchBase.DestinationState = (SM_State)EditorGUILayout.ObjectField("Destination state", branchBase.DestinationState, typeof(SM_State), true);

            if (branchBase.DestinationState != null)
            {
                Branch branch = (Branch)branchBase;
                if (branch != null)
                {
                    branch.ReverseInput = EditorGUILayout.Toggle("Reverse Input ", branch.ReverseInput);

                    GUILayout.BeginVertical("Box");
                    int acceptedLength = branch.AcceptableInputs.Count;
                    for (int e = 0; e < acceptedLength; e++)
                    {
                        GUILayout.BeginHorizontal();
                        branch.AcceptableInputs[e] = (InputUnit)EditorGUILayout.ObjectField("Input Unit", branch.AcceptableInputs[e], typeof(InputUnit), true);
                        if (GUILayout.Button(deleteButtonContent))
                            branch.AcceptableInputs.RemoveAt(e);
                        GUILayout.EndHorizontal();
                    }

                    GUILayout.BeginHorizontal();
                    GUILayout.Label("Add accepted input");
                    if (GUILayout.Button(duplicateButtonContent))
                        branch.AcceptableInputs.Add(default(InputUnit));


                    GUILayout.EndHorizontal();
                    GUILayout.EndVertical();
                }
            }
            if (GUILayout.Button(deleteButtonContent))
                state.InstancedBranches.RemoveAt(i);
            GUILayout.EndVertical();
        }
        SolveBranchButtons();
    }


    private void SolveObjectField<X>(ref X field, string text) where X : UnityEngine.Object
    {
        
        field = (X)EditorGUILayout.ObjectField(text, field, typeof(X), true);
    }

    private void SolveBranchButtons()
    {
        GUILayout.BeginHorizontal();
        GUILayout.Label("Add Branch");

        if (GUILayout.Button(duplicateButtonContent))
            state.InstancedBranches.Add(default(Branch));// = state.InstancedBranches.DuplicateLastArrayItem();

        //if (GUILayout.Button(deleteButtonContent))
        //    state.InstancedBranches = state.InstancedBranches.RemoveLastArrayItem();
 
        GUILayout.EndHorizontal();
    }
    
    

    private static GUIContent
    moveButtonContent = new GUIContent("\u21b4", "Move down"),
    duplicateButtonContent = new GUIContent("+", "Duplicate"),
	deleteButtonContent = new GUIContent("-", "Delete");
}
