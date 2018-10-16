using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

	public void Move(float inputAxis)
    {
        // To avoid errors.
        inputAxis = Mathf.Clamp(inputAxis, -1, 1);
    }

}
