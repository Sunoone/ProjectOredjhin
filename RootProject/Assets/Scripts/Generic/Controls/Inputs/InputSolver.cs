using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputSolver : MonoBehaviour {
    // What is this was a static script and every move has an enum execution option?
    // Also, multiple executions can be added.


    List<string> keyCodeStrings;
    List<KeyCode> keyCodes;
    // List<KeyIdentification> 

    /*
     * Within this script there needs to be a pattern recognation of some sorts. Whenever a combination of keys has been pressed and recognized,
     * the conditions have been met to execute the move.
     * 
     * Difficulties lie within:
     *  - Recognizing the pattern accurately and quickly
     *  - Allowing for some leniency in input. A mistake in between needs to be sorted out and categorized.
     */



	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
