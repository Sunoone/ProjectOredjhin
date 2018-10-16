using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class ErrorPopupEditor : EditorWindow
{
    public static void ShowError(string message)
    {
        ErrorPopupEditor popup = EditorWindow.GetWindow<ErrorPopupEditor>();
        popup.Init(message);
    }



    string message;
    GUIStyle style;
    public void Init(string message)
    {
        this.message = message;

        style = new GUIStyle();
        style.normal.textColor = Color.red;
        style.fontStyle = FontStyle.Bold;
        style.alignment = TextAnchor.UpperCenter;

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
        if (GUILayout.Button("Ok", centeredStyle)) this.Close();
    }

    float width, height;

    private void SetPopupSize()
    {
        Vector2 ErrorMessageSize = GUI.skin.label.CalcSize(new GUIContent(message));
        //GUILayoutUtility.GetRect(new GUIContent(message), style).size;
        //GUI.skin.label.CalcSize(new GUIContent(message));
        width = ErrorMessageSize.x * 1.3f;
        height = ErrorMessageSize.y * 3;
        minSize = new Vector2(width * 2, height * 2);
        maxSize = minSize;
    }
}
