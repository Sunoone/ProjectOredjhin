using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class FreethwareEditor : Editor {

    protected bool AskBeforeDelete = true;
    protected bool MoveUpBool = true;
    protected bool AddButton(string text, Callback cb, float intensity = 1.0f)
    {
        ChangeColor(Color.green / intensity);
        if (GUILayout.Button(text))
        {
            cb.Invoke();
            RestoreColor();
            return true;
        }
        RestoreColor();
        return false;
    }
    protected static GUIContent
        moveDownButtonContent = new GUIContent("\u2193", "Move Down"),
        moveUpButtonContent = new GUIContent("\u2191", "Move Up");
    protected bool RemoveButton(string text, string name, Callback cb, float intensity = 1.0f)
    {
        ChangeColor(Color.red / intensity);
        if (GUILayout.Button(text))
        {
            if (AskBeforeDelete)
            {
                ChoicePopupEditor.ShowChoice("Are you sure you want to delete " + name + " from the Dialogue Tree (This can not be undone)",
                cb, null);
            }
            else
            {
                cb.Invoke();
            }
            RestoreColor();
            return true;
        }
        RestoreColor();
        return false;
    }

    
    protected bool MoveDown<X>(List<X> list, int index, float intensity = 1.0f) where X : class
    {
        if (MoveUpBool)
            return false;

        ChangeColor(Color.blue / intensity);
        if (GUILayout.Button(moveDownButtonContent))
        {
            if (index + 1 >= list.Count)
                return false;

            X storage = list[index + 1];
            list[index + 1] = list[index];
            list[index] = storage;
            RestoreColor();
            return true;
        }
        RestoreColor();
        return false;  
    }
    protected bool MoveUp<X>(List<X> list, int index, float intensity = 1.0f) where X : class
    {
        if (!MoveUpBool)
            return false;

        ChangeColor(Color.cyan / intensity);
        if (GUILayout.Button(moveUpButtonContent))
        {
            if (index == 0)
                return false;

            X storage = list[index - 1];
            list[index - 1] = list[index];
            list[index] = storage;
            RestoreColor();
            return true;
        }
        RestoreColor();
        return false;
    }

    Color defaultColor = Color.gray;
    public void ChangeColor(Color color)
    {
        defaultColor = GUI.backgroundColor;
        GUI.backgroundColor = color;
    }
    public void RestoreColor()
    {
        GUI.backgroundColor = Color.white;
    }

    protected void Horizontal(bool b = true) {
        if (b)
            GUILayout.BeginHorizontal();
        else
            GUILayout.EndHorizontal();
    }

}
