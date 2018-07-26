﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectInteraction : MonoBehaviour {


	void Update () {
		if (Input.GetMouseButtonDown(0)){
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			RaycastHit hit;
            GameObject target = hit.transform.gameObject;

            if (Physics.Raycast(ray, out hit)){
				if((this.transform.position - hit.transform.position).magnitude < Config.interactionDistance){
					if(hit.transform.gameObject.GetComponent<Door>() != null){
						Debug.Log("Door");
					} else if (hit.transform.gameObject.GetComponent<Container>() != null){
						ItemDragHandler idh = Camera.main.GetComponent<ItemDragHandler>();
						idh.enabled = true;
						idh.addContainer(hit.transform.gameObject.GetComponent<Container>());
					}
				}
			}
		}
	}
}
