using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Freethware.Inputs;

public class Fighter : MonoBehaviour {

    public InputManager InputManager;
    public ButtonProfile InputProfile;
  
    public void SetPlayerProfile(ButtonProfile customInputs)
    {

    }

    public float MoveSpeed = 2;
    private CharacterController controller;
    private Vector3 moveVector;
    private float verticalVelocity;

    public Collider[] attackHitboxes;

	// Use this for initialization
	void Start () {
        controller = GetComponent<CharacterController>();
        InputProfile = InputManager.Profiles[0];

    }
	
	// Update is called once per frame
	void Update ()
    {

        //if (InputProfile.




        InputProfile.GetInputUp(PlayerButton.LightAttack);
        InputProfile.GetInputUp(PlayerButton.HeavyAttack);
        InputProfile.GetInputUp(PlayerButton.Jump);



        if (InputProfile.GetInputDown(PlayerButton.LightAttack))
            LaunchAttack(attackHitboxes[0]);
        if (InputProfile.GetInputDown(PlayerButton.HeavyAttack)) 
            LaunchAttack(attackHitboxes[1]);



        if (controller.isGrounded)
        {
            verticalVelocity = -1;
            if (InputProfile.GetInputDown(PlayerButton.Jump))
                verticalVelocity = 10;
        }
        else
        {
            verticalVelocity -= 14 * Time.deltaTime;
        }

        moveVector = Vector3.zero;
        moveVector.x = Input.GetAxis("Horizontal") * MoveSpeed;
        moveVector.y = verticalVelocity;

        controller.Move(moveVector * Time.deltaTime);
	}

    private void LaunchAttack(Collider col)
    {
        Collider[] cols = Physics.OverlapBox(col.bounds.center, col.bounds.extents, col.transform.rotation, LayerMask.GetMask("Hitbox"));
        foreach(Collider c in cols)
        {
            if (c.transform.root == transform.root)
                continue;

            Debug.Log(c.name);
        }
    }

}
