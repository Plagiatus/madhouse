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
	private Vector3 temp1;

	void Start () {
		offset = this.transform.position - player.transform.position;
		temp1 = this.transform.localPosition; 
	}

	void Update() {
		Mathf.Lerp(sanity, targetSanity, Time.deltaTime);
		distortImage();
	}

	private void moveCameraBehindPLayer(){
			RaycastHit hit;
			Vector3 hitPoint;
			
			Ray ray = new Ray(player.transform.position + Vector3.up * 0.5f, player.transform.forward * -1);
			Debug.DrawRay(ray.origin, ray.direction, Color.green, 1);
			// Debug.Log(Physics.Raycast(ray, out hit, 2.5f));
			if (Physics.Raycast(ray, out hit, 2.5f)){
				hitPoint = hit.point;
				// Debug.Log("hit");
				// Debug.Log(hitPoint);
				// worldposition auf lokalposition setzen
				player.transform.localPosition = this.transform.InverseTransformPoint(hitPoint);
				Debug.Log(player.transform.localPosition);
			}
			// camera auf standard setzen
			else { 
				player.transform.localPosition = temp1; 
				// Debug.Log("no hit"); 
			}
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
