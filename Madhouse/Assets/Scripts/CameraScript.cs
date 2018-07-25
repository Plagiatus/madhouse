using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour {

	public GameObject player;
	private Vector3 offset;
	private bool inInventory = false;
	private bool inTransition = false;
	[Range(-20,80)]
	private float sanity;
	private float targetSanity;

	void Start () {
		offset = this.transform.position - player.transform.position; 
	}

	void Update() {
		Mathf.Lerp(sanity, targetSanity, Time.deltaTime);
		distortImage();
	}

	public void transitionToState(bool toInventory){
		if(toInventory && !inInventory && !inTransition){
			inTransition = true;
			//TODO: move to Inventory view, maybe a Coroutine or Animation.
			inInventory = true;
		} else if (!toInventory && inInventory && !inTransition) {
			inTransition = true;
			//TODO: move back to normal view, maybe via Coroutine or Animation
			inInventory = false;
		}
	}

	private void distortImage(){
		//TODO: implement video changes here, depending on the sanity level
	}

	public void moveSanityTo(float newSanity){
		targetSanity = Mathf.Clamp(newSanity, -20, 80);
	}

}
