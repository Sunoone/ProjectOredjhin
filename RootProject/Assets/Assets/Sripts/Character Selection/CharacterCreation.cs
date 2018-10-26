using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterCreation : MonoBehaviour {


    private List<GameObject> models;  // List containing all character models. 
    private int selectionIndex = 0;     // Default index of the model;

    // Use this for initialization
    void Start () {
         models = new List<GameObject>();
        // Adds all models into the list.
        foreach (Transform t in transform)
        {
            models.Add(t.gameObject);
            t.gameObject.SetActive(false);
        }
        models[selectionIndex].SetActive(true);     //This will set the first index of the list to active, so it appears on the screen.		
	}

    public void SelectNextCharacter(int index)
    {
        if (index == selectionIndex)
            return;
        if (index < 0 || index >= models.Count)
            return;

        models[selectionIndex].SetActive(false);
        selectionIndex = index;
        models[selectionIndex].SetActive(true);
    }
 }
