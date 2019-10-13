using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Reflection;

public static class DebugUtilities {

    public static void QuitPlayMode()
    {
        Type t = null;
        foreach (Assembly a in AppDomain.CurrentDomain.GetAssemblies())
        {
            t = a.GetType("UnityEditor.EditorApplication");
            if (t != null)
            {
                t.GetProperty("isPlaying").SetValue(null, false, null);
                break;
            }
        }
    }
}

public static class DebugExtensions
{
    public static void RuntimeOnly(this MonoBehaviour obj)
    {
        Debug.LogError("Running a runtime only script. Remove component " + obj.ToString() + "from " + obj.gameObject + " before building.");
    }
}
