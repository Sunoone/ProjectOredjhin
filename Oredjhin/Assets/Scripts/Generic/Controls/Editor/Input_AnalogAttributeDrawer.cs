using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
[CustomPropertyDrawer(typeof(Input_AnalogAttribute))]
public class Input_AnalogAttributeDrawer : PropertyDrawer
{
    public enum AnalogStick
    {
        Left,
        Right,
        Dpad
    }
    public enum ControllerAxis
    {
        Horizontal,
        Vertical,  
        Trigger,
    }
    public enum DeviceType
    {
        Controller,
        Keyboard,
        Mouse
    }

    public override void OnGUI(Rect _position, SerializedProperty _property, GUIContent _label)
    {
        string[] components = _property.stringValue.Split('_');

        if (components.Length < 4)
        {
            components = new string[4] { ((AnalogStick)0).ToString(), ((ControllerAxis)0).ToString(), ((DeviceType)0).ToString(), "_0" };
            OnGUI(_position, _property, _label);
            return;
        }

        GUILayout.BeginVertical("Box");
        string analogStick = components[0];
        AnalogStick eAnalogStick = AnalogStick.Dpad;
        var namesCount = System.Enum.GetNames(typeof(AnalogStick)).Length;
        for (int i = 0; i < namesCount; i++)
        {
            eAnalogStick = (AnalogStick)i;
            if (eAnalogStick.ToString() == analogStick)
                break;
            //if (analogStick == )
        }
        GUILayout.Label(_property.stringValue);
        eAnalogStick = (AnalogStick)EditorGUILayout.EnumPopup("AnalogStick", eAnalogStick);

        string controllerAxis = components[1];
        ControllerAxis eControllerAxis = ControllerAxis.Horizontal;
        namesCount = System.Enum.GetNames(typeof(ControllerAxis)).Length;
        for (int i = 0; i < namesCount; i++)
        {
            eControllerAxis = (ControllerAxis)i;
            if (eControllerAxis.ToString() == controllerAxis)
                break;
            //if (analogStick == )
        }
        eControllerAxis = (ControllerAxis)EditorGUILayout.EnumPopup("Axis", eControllerAxis);

        string deviceType = components[2];
        DeviceType eDeviceType = DeviceType.Controller;
        namesCount = System.Enum.GetNames(typeof(DeviceType)).Length;
        for (int i = 0; i < namesCount; i++)
        {
            eDeviceType = (DeviceType)i;
            if (eDeviceType.ToString() == deviceType)
                break;
            //if (analogStick == )
        }
        eDeviceType = (DeviceType)EditorGUILayout.EnumPopup("Device", eDeviceType);

        int index = 0;
        int.TryParse(components[3], out index);
        index = EditorGUILayout.IntSlider(index, 0, 4);

        string newStringValue = eAnalogStick.ToString() + "_" + eControllerAxis.ToString() + "_" + eDeviceType.ToString() + "_" + index.ToString();
        if (newStringValue != _property.stringValue)
        {
            _property.stringValue = newStringValue;
            Debug.Log("Changed to: " + _property.stringValue);
        }

        GUILayout.EndVertical();
    }


    /*private T RecoverEnum<T>(string id) where T : System.Enum
    {
        if (!typeof(T).IsEnum)
        {
            throw new System.ArgumentException("T must be an enumerated type");
        }

        T enumType = default(T);
        var namesCount = System.Enum.GetNames(typeof(T)).Length;
        for (int i = 0; i < namesCount; i++)
        {
            enumType = (T)i;
            if (enumType.ToString() == id)
                return enumType;
        }
        return default(T);
    }*/
}
