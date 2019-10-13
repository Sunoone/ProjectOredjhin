using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Freethware.Inputs;

[CustomPropertyDrawer(typeof(Controls_ButtonUnitFlagAttribute))]
public class Controls_ButtonUnitFlagAttributeDrawer : PropertyDrawer {

    Controls_ButtonUnit[] buttons;
    public override void OnGUI(Rect _position, SerializedProperty _property, GUIContent _label)
    {
        string[] options = new string[0];
        if (buttons == null)
        {
            buttons = EditorGUIFileUtilities.FindAssetsWithExtension<Controls_ButtonUnit>(".asset");
            int length = buttons.Length;
            options = new string[length];
            for (int i = 0; i < length; i++)
            {
                options[i] = buttons[i].name.ToString();
                buttons[i].ButtonIndex = i;
            }
        }
        _property.intValue = EditorGUIUtilities.SetFlags(_property.name, _property.intValue, options);
    }
}

[CustomPropertyDrawer(typeof(Controls_ButtonUnitAttribute))]
public class Controls_ButtonUnitAttributeDrawer : PropertyDrawer
{
    public override void OnGUI(Rect _position, SerializedProperty _property, GUIContent _label)
    {
        ForceIndex(_property.name, _property.intValue);
        GUI.enabled = false;
        EditorGUILayout.IntField(_property.name, _property.intValue);
        GUI.enabled = true;
    }

    Controls_ButtonUnit[] buttons;
    private void ForceIndex(string propertyName, int flag)
    {
        if (buttons == null)
        {
            buttons = EditorGUIFileUtilities.FindAssetsWithExtension<Controls_ButtonUnit>(".asset");
            int length = buttons.Length;
            for (int i = 0; i < length; i++)
            {
                buttons[i].ButtonIndex = i;
            }
        }
    }
}
