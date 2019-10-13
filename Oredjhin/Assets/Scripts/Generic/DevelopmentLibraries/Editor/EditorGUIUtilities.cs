using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public static class EditorGUIUtilities {

    public static int SetFlags(string propertyName, int flag, string[] options)
    {
        Controls_InputUnit[] buttons = EditorGUIFileUtilities.FindAssetsWithExtension<Controls_InputUnit>(".asset");
        int length = buttons.Length;
        options = new string[length];
        for (int i = 0; i < length; i++)
        {
            options[i] = buttons[i].name.ToString();
        }

        flag = EditorGUILayout.MaskField(propertyName, flag, options);
        List<string> selectedOptions = new List<string>();
        for (int i = 0; i < options.Length; i++)
        {
            if ((flag & (1 << i)) == (1 << i)) selectedOptions.Add(options[i]);
        }
        EditorGUILayout.LabelField("Value: " + flag);
        return flag;
    }
}
