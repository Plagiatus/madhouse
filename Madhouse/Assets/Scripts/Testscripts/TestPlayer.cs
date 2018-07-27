using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestPlayer : MonoBehaviour {

    public float movementSpeed;
    public float turningSpeed;
    public Animator animator;

    public void handleInputs()
    {
        if(Input.GetKey(KeyCode.W))
        {
            animator.SetBool("isWalking", true);
            animator.SetFloat("movementSpeed", movementSpeed);
            this.transform.Translate(0, 0, Time.deltaTime * movementSpeed);
        }
        if(Input.GetKey(KeyCode.S))
        {
            animator.SetBool("isWalking", true);
            this.transform.Translate(0, 0, -Time.deltaTime * movementSpeed);
        }
        if(Input.GetKey(KeyCode.A))
        {
            transform.Rotate(-Vector3.up * turningSpeed * Time.deltaTime);
        }
        if(Input.GetKey(KeyCode.D))
        {
            transform.Rotate(Vector3.up * turningSpeed * Time.deltaTime);
        }
        if(!Input.GetKey(KeyCode.S) && !Input.GetKey(KeyCode.W))
        {
            animator.SetBool("isWalking", false);
        }
    }

	// Use this for initialization
	void Start () {
        
	}
	
	// Update is called once per frame
	void Update () {
        handleInputs();
        

    }
}
