using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectInteraction : MonoBehaviour {
	public GameObject player;

	void Update () {
		if (Input.GetMouseButtonDown(0)){
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			RaycastHit hit;
            if (Physics.Raycast(ray, out hit)){
            	GameObject target = hit.transform.gameObject;
				// Debug.Log(player.transform.position + " .. " + hit.transform.position);
				if((player.transform.position - hit.transform.position).magnitude < Config.interactionDistance){
					// Debug.Log("tap close enough on " + target.name);
					if(hit.transform.gameObject.GetComponent<Door>() != null){
						Debug.Log("Door");
					// } else if (target.GetComponent<Container>() != null){
					// 	Debug.Log("Container");
					// 	ItemDragHandler idh = Camera.main.GetComponent<ItemDragHandler>();
					// 	idh.enabled = true;
					// 	idh.addContainer(hit.transform.gameObject.GetComponent<Container>());
					}
				}
			}
		}
	}
}