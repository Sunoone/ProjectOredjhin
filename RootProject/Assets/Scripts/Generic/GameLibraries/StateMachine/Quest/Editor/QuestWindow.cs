using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

using StateMachine.Quests;

public class QuestWindow : EditorWindow {

    string myString = "Hello World";
    bool groupEnabled;
    bool myBool = true;
    float myFloat = 1.23f;

    // Add menu named "My Window" to the Window menu
    [MenuItem("Quest/Quest editor")]
    public static void Init()
    {
        // Get existing open window or if none, make a new one:
        QuestWindow window = (QuestWindow)EditorWindow.GetWindow(typeof(QuestWindow));
        window.Show();
    }
    public static void Init(Quest quest)
    {
        // Get existing open window or if none, make a new one:
        QuestWindow window = (QuestWindow)EditorWindow.GetWindow(typeof(QuestWindow));
        window.Quest = quest;
        window.Show();
    }

    public Quest Quest;

    void OnGUI()
    {
        Quest = (Quest)EditorGUILayout.ObjectField("Quest:", Quest, typeof(Quest), true);
        if (Quest == null)
            return;

        DrawStateEditor(Quest); 

        if (Quest.QuestStateMachine != null)
            DrawStateEditor(Quest.QuestStateMachine);


    }

    void DrawStateEditor(Object state)
    {
        GUILayout.BeginVertical("Box");
        Editor editor = Editor.CreateEditor(state);
        editor.OnInspectorGUI();
        GUILayout.EndVertical();
    }

}
