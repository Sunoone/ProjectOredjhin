using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


public class ChoicePopupEditor : EditorWindow
{
    public static void ShowChoice(string message, Callback yes, Callback no)
    {
        ChoicePopupEditor popup = EditorWindow.GetWindow<ChoicePopupEditor>();
        popup.Init(message, yes, no);
    }

    string message;
    GUIStyle style;

    Callback yes;
    Callback no;

    public void Init(string message, Callback yes, Callback no)
    {
        this.yes = yes;
        this.no = no;

        this.message = message;

        style = new GUIStyle();
        style.normal.textColor = Color.red;
        style.fontStyle = FontStyle.Bold;
        style.alignment = TextAnchor.MiddleLeft;

        DebugUtilities.QuitPlayMode();
    }

    void OnGUI()
    {
        SetPopupSize();
        EditorGUILayout.Space();
        EditorGUILayout.LabelField(message, style);
        EditorGUILayout.Space();
        var centeredStyle = GUI.skin.GetStyle("Button");
        centeredStyle.alignment = TextAnchor.LowerCenter;
        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button("Yes", centeredStyle))
        {
            if (yes != null)
                yes.Invoke();
            this.Close();
        }
        if (GUILayout.Button("No", centeredStyle))
        {
            if (no != null)
                no.Invoke();
            this.Close();
        }
    }

    float width, height;

    private void SetPopupSize()
    {
        Vector2 MessageSize = GUI.skin.label.CalcSize(new GUIContent(message));
        //GUILayoutUtility.GetRect(new GUIContent(message), style).size;
        //GUI.skin.label.CalcSize(new GUIContent(message));
        width = MessageSize.x * .75f;
        height = MessageSize.y * 3;
        minSize = new Vector2(width * 2, height * 2);
        maxSize = minSize;
    }
}
