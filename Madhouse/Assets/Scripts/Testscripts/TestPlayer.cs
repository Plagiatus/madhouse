using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestPlayer : MonoBehaviour {

    public float movementSpeed;
    public float crouchModifier;
    private float currentMod = 1;
    public float turningSpeed;
    public Animator animator;
    public Transform playerStart;
    private bool crouching = false;
    public float baseSneak = 0.25f;
    private float currentSneakiness = 0.25f;

    public void respawn()
    {
        this.transform.position = new Vector3(playerStart.position.x, this.transform.position.y, playerStart.position.z);
    }
    public void handleInputs()
    {
        if(Input.GetKey(KeyCode.W))
        {
            animator.SetBool("isWalking", true);
            animator.SetFloat("movementSpeed", movementSpeed);
            this.transform.Translate(0, 0, Time.deltaTime * movementSpeed * currentMod);
        }
        if(Input.GetKey(KeyCode.S))
        {
            animator.SetBool("isWalking", true);
            this.transform.Translate(0, 0, -Time.deltaTime * movementSpeed * currentMod);
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
        if(Input.GetKeyDown(KeyCode.C))
        {
            if(crouching == false)
            {
                animator.SetBool("isCrouching", true);
                crouching = true;
                currentMod = crouchModifier;
            }
            else
            {
                animator.SetBool("isCrouching", false);
                crouching = false;
                currentMod = 1;
            }
        }
    }
    public float getSneak()
    {
        return currentSneakiness;
    }
    public void handleSneaky()
    {
        float sneakiness = baseSneak;
        if(animator.GetBool("isWalking"))
        {
            sneakiness -= 0.25f;
        }
        if (animator.GetBool("isCrouching"))
        {
            sneakiness += 0.25f;
        }
        currentSneakiness = sneakiness;
    }
    // Use this for initialization
	void Start () {
        if (playerStart != null)
        {
            this.transform.position = new Vector3(playerStart.position.x, this.transform.position.y, playerStart.position.z);
        }
	}
	
	// Update is called once per frame
	void Update () {
        handleInputs();
        handleSneaky();


    }
}
