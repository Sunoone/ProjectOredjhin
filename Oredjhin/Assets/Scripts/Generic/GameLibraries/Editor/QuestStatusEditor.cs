using UnityEditor;
using UnityEngine;
using System;
using StateMachine;
using StateMachine.Quests;
using System.Collections.Generic;

[CanEditMultipleObjects]
[CustomEditor(typeof(QuestStatus), true)]
public class QuestStatusEditor : Editor {
    QuestStatus questStatus;

    

    

    static bool showTileEditor = false;
    float boxSize = 65f;
    public void OnEnable()
    {
        questStatus = (QuestStatus)target;
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        SolveInputUnits();
        SolveQuestActivities();
        SolveQuestList("Active Quests", questStatus.QuestList);
        //SolveQuestList("PriorityQuests", questStatus.PriorityQuests, true);
        //SolveQuestList("PriorityList", questStatus.PriorityList, true);


        serializedObject.ApplyModifiedProperties();
        if (GUI.changed)
            EditorUtility.SetDirty(target);
    }

    private void SolveInputUnits()
    {
        questStatus.Keypad1 = (InputUnit)EditorGUILayout.ObjectField("Keypad 1: ", questStatus.Keypad1, typeof(InputUnit), true);
        questStatus.Keypad2 = (InputUnit)EditorGUILayout.ObjectField("Keypad 2: ", questStatus.Keypad2, typeof(InputUnit), true);
        questStatus.Keypad3 = (InputUnit)EditorGUILayout.ObjectField("Keypad 3: ", questStatus.Keypad3, typeof(InputUnit), true);
        questStatus.Keypad4 = (InputUnit)EditorGUILayout.ObjectField("Keypad 4: ", questStatus.Keypad4, typeof(InputUnit), true);
        questStatus.Keypad5 = (InputUnit)EditorGUILayout.ObjectField("Keypad 5: ", questStatus.Keypad5, typeof(InputUnit), true);
        questStatus.Keypad6 = (InputUnit)EditorGUILayout.ObjectField("Keypad 6: ", questStatus.Keypad6, typeof(InputUnit), true);
        GUILayout.Label("");
    }

    private void SolveQuestActivities()
    {
        EditorGUILayout.LabelField("Quest activities");
        EditorGUILayout.BeginVertical("Box");
        GUI.enabled = false;
        int length = questStatus.QuestActivities.Count;
        for (int i = 0; i < length; i++)
        {
            EditorGUILayout.ObjectField("Quest activity " + i + ": ", questStatus.QuestActivities[i], typeof(InputUnit), true);
        }
        EditorGUILayout.EndVertical();
        GUI.enabled = true;
    }

    private void SolveQuestLog(QuestInProgress QIP, bool interactable = true)
    {
        
        if (QIP.Quest != null)
        {
            EditorGUILayout.LabelField(QIP.Quest.QuestName, EditorStyles.boldLabel);

            GUI.enabled = interactable;
            QIP.QuestProgress = (EQuestCompletion)EditorGUILayout.EnumPopup(QIP.QuestProgress);
            GUI.enabled = false;
            EditorGUILayout.LabelField("Quest log: ", EditorStyles.boldLabel);
            //if (QIP.QuestActivities == null)
            //    QIP.QuestActivities = new List<InputUnit>();
            if (QIP.QuestActivities != null)
            {
                int lengthInputs = QIP.QuestActivities.Count;
                for (int e = 0; e < lengthInputs; e++)
                {
                    InputUnit Log = QIP.QuestActivities[e];
                    EditorGUILayout.LabelField(Log.Description, EditorStyles.boldLabel);
                }
            }
            GUI.enabled = true;
        }
    }

    private void SolveQuestList(string activeQuests, List<QuestInProgress> questList, bool displayOnly = false)
    {
        EditorGUILayout.LabelField(activeQuests, EditorStyles.boldLabel);
        int lengthQuests = questList.Count;
        for (int i = 0; i < lengthQuests; i++)
        {
            EditorGUILayout.BeginVertical("Box");
            QuestInProgress QIP = questList[i];
            /*if (QIP.Info != null)
            {
                if (QIP.Info.QuestState == null)
                    if (QIP.Quest != null)
                        if (QIP.Quest.QuestStateMachine != null)
                            QIP.Info.QuestState = QIP.Quest.QuestStateMachine;

                if (QIP.Info.QuestState != null)
                    GUILayout.Label(QIP.Info.QuestState.ToString());
            }*/

            if (!displayOnly)
                QIP.Quest = (Quest)EditorGUILayout.ObjectField((i + 1).ToString(), QIP.Quest, typeof(Quest), true);
            SolveQuestLog(QIP, true);

            if (!displayOnly)
            {
                GUILayout.BeginHorizontal();
                if (Application.isPlaying)
                {
                    if (GUILayout.Button(resetButtonContent))
                    {
                        QIP.Reset();
                    }
                    GUILayout.Label(QIP.priority.ToString());
                }
                if (GUILayout.Button(deleteButtonContent))
                {
                    questStatus.QuestList.RemoveAt(i);
                }
                GUILayout.EndHorizontal();
            }              
            EditorGUILayout.EndVertical();
        }

        if (!displayOnly)
        {
            if (GUILayout.Button(addButtonContent))
            {
                questStatus.QuestList.Add(new QuestInProgress());
            }
        }
    }

    private static GUIContent
    moveButtonContent = new GUIContent("\u21b4", "Move down"),
    addButtonContent = new GUIContent("Add Quest", "Add a quest"),
    deleteButtonContent = new GUIContent("-", "Delete"),
    resetButtonContent = new GUIContent("Reset", "Reset");

}
