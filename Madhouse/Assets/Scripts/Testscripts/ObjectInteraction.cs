using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectInteraction : MonoBehaviour {


	void Update () {
		if (Input.GetMouseButtonDown(0)){
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			RaycastHit hit;
			if(Physics.Raycast(ray, out hit)){
				if((this.transform.position - hit.transform.position).magnitude < 10){
					if(hit.transform.gameObject.GetComponent<Door>() != null){
						Debug.Log("Door");
					} else if (hit.transform.gameObject.GetComponent<Container>() != null){
						Debug.Log("Container");
					}
				}
			}
		}
	}
}
