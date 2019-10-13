using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour{

    public float MovementSpeed;

    public bool selected = false;

    public virtual void PlayerMovement()
    {
        if(selected == true)
        {
            if (Input.GetKey(KeyCode.A))
            {
                transform.Translate(Vector3.left * MovementSpeed * Time.deltaTime);
            }

            if (Input.GetKey(KeyCode.W))
            {
                transform.Translate(Vector3.forward * MovementSpeed * Time.deltaTime);
            }

            if (Input.GetKey(KeyCode.D))
            {
                transform.Translate(Vector3.right * MovementSpeed * Time.deltaTime);
            }

            if (Input.GetKey(KeyCode.S))
            {
                transform.Translate(Vector3.back * MovementSpeed * Time.deltaTime);
            }
        }
    }
        

    void Update()
    {
        PlayerMovement();
    }

    public void GotSelected()
    {
        selected = true;
    }

}
