using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controller_Inputs_Player : MonoBehaviour {

    private void Start()
    {
        ControllerCheck();
        
    }
    public bool playstationController, xboxController, keyboard;

    public string[] currentControllers;
    public float controllerCheckTimer = 2;
    public float controllerCheckTimerOG = 2;
    public void ControllerCheck()
    {
        int numberOfControllers = 0;
        for (int i = 0; i < Input.GetJoystickNames().Length; i++)
        {
            Debug.Log(Input.GetJoystickNames()[i]);
            //Input.
        }
    }
}
