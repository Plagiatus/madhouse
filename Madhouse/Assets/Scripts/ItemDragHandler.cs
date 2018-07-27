using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemDragHandler : MonoBehaviour {

	bool isDragging = false;
	GameObject target;
	Vector3 offsetValue;
	Vector3 originalTargetPosition;
	Vector3 positionOfScreen;
	
	public Player player;
	Container container;

	Dictionary<eSlot, Item> items;

	eSlot originSlot;

	float timer = 0;

	void OnEnable () {
		container = null;
		transform.Find("Container").gameObject.SetActive(false);
		transform.Find("Player").gameObject.SetActive(true);
		Dictionary<eSlot, Item> playerItems = player.getItems();
		items = new Dictionary<eSlot, Item>(){
			{eSlot.HAND, playerItems[eSlot.HAND]},
			{eSlot.LEFTPOCKET, playerItems[eSlot.LEFTPOCKET]},
			{eSlot.RIGHTPOCKET, playerItems[eSlot.RIGHTPOCKET]}
		};

		checkForContainer();

		createItems();
	}

	void OnDisable(){
		for(int i = 0; i < transform.childCount; i++){
			GameObject go = transform.GetChild(i).gameObject;
			if(go.tag == "Item"){
				Destroy(go);
			}
		}
		transform.Find("Container").gameObject.SetActive(false);
		transform.Find("Player").gameObject.SetActive(false);
	}

	void checkForContainer(){
		GameObject closestCollider = null;
		Collider[] closeColliders = Physics.OverlapSphere(player.transform.position, Config.interactionDistance);
		foreach(Collider c in closeColliders){
			if(c.gameObject.GetComponent<Container>() != null){
				if(closestCollider != null){
					if((player.transform.position - c.gameObject.transform.position).sqrMagnitude < (player.transform.position - closestCollider.transform.position).sqrMagnitude){
						closestCollider = c.gameObject;
					}
				} else {
					closestCollider = c.gameObject;
				}
			}
		}

		if(closestCollider != null){
			addContainer(closestCollider.GetComponent<Container>());
			player.transform.LookAt(new Vector3(closestCollider.transform.position.x, player.transform.position.y, closestCollider.transform.position.z ));
		}
	}

	void createItems(){
		for(int i = 0; i < transform.childCount; i++){
			GameObject go = transform.GetChild(i).gameObject;
			if(go.tag == "Item"){
				Destroy(go);
			}
		}
		foreach(KeyValuePair<eSlot,Item> pair in items){
			if(pair.Value != null){
				GameObject newItem = (GameObject) Instantiate(Resources.Load("Prefab/" + pair.Value.itemname));
				newItem.transform.position = GameObject.Find(Config.enumToNameString(pair.Key)).transform.position + Vector3.up * newItem.gameObject.GetComponent<Renderer>().bounds.size.y * 0.5f;
				newItem.GetComponent<Item_Mono>().itemname = pair.Value.itemname;
				newItem.transform.parent = this.transform;
				newItem.transform.rotation = player.transform.rotation;
				newItem.layer = 10;
				newItem.tag = "Item";
			}
		}
	}
	
	public void addContainer(Container co){
		container = co;
		activateContainer();
	}

	void activateContainer(){
		transform.Find("Container").gameObject.SetActive(true);
		Dictionary<eSlot, Item> containerItems = container.getItems();
		for(int i=3; i < 6; i++){
			if(containerItems.ContainsKey((eSlot) i)){
				items.Add((eSlot) i, containerItems[(eSlot) i]);
				transform.Find("Container").Find(Config.enumToNameString((eSlot) i)).gameObject.SetActive(true);
			} else {
				transform.Find("Container").Find(Config.enumToNameString((eSlot) i)).gameObject.SetActive(false);
			}
		}
	}

	void Update () {
		timer += Time.deltaTime;
		//START DRAG
		if(Input.GetMouseButtonDown(0)){
			RaycastHit hitInfo;
			target = GetClickedObject(out hitInfo);
			if(target != null && target.gameObject.tag == "Item"){
				if(GetClickedSlot(out hitInfo) != null) originSlot = Config.gameObjectToEnum(GetClickedSlot(out hitInfo));
				if(timer > 0.2){
					// Debug.Log("origin: " + originSlot);
					isDragging = true;
					originalTargetPosition = target.transform.position;
					positionOfScreen = Camera.main.WorldToScreenPoint(target.transform.position);
					offsetValue = target.transform.position - Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, positionOfScreen.z));
				} else {
					//Doubleclick
					if(originSlot == eSlot.HAND){
						useItem(target);
						player.takeItem(originSlot);
					}
				}
			} else {
				target = null;
			}
			timer = 0;
		}

		//DROP
		if(Input.GetMouseButtonUp(0)){
			isDragging = false;
			if(target==null) return;
			RaycastHit hit;
			GameObject slotGO = GetClickedSlot(out hit);
			if(slotGO != null){
				// Debug.Log("target: " + slotGO.name);
				//if item was pulled onto a slot
				eSlot currentSlot = Config.gameObjectToEnum(slotGO);
				//if the new slot is currently occupied
				if(Config.isPlayerSlot(currentSlot)){
					if(player.getItems()[currentSlot] != null){
						foreach(GameObject go in GameObject.FindGameObjectsWithTag("Item")){
							if(go.GetComponent<Item_Mono>() != null){
								if(go.GetComponent<Item_Mono>().itemname == player.getItems()[currentSlot].itemname){
									go.transform.position = GameObject.Find(Config.enumToNameString(originSlot)).transform.position + Vector3.up * target.gameObject.GetComponent<Renderer>().bounds.size.y * 0.5f;
									break;
								}
							}
						}
					}
				} else {
					if(container.getItems()[currentSlot] != null){
						foreach(GameObject go in GameObject.FindGameObjectsWithTag("Item")){
							if(go.GetComponent<Item_Mono>() != null){
								if(go.GetComponent<Item_Mono>().itemname == container.getItems()[currentSlot].itemname){
									go.transform.position = GameObject.Find(Config.enumToNameString(originSlot)).transform.position + Vector3.up * target.gameObject.GetComponent<Renderer>().bounds.size.y * 0.5f;
									break;
								}
							}
						}
					}
				}

				// Debug.Log("from: " + originSlot + " to: " + currentSlot);

				//if both items are inside the player inventory
				if(Config.isPlayerSlot(currentSlot) && Config.isPlayerSlot(originSlot)){
					player.swapItems(originSlot,currentSlot);
				}
				//if both items are inside the container inventory
				if(!Config.isPlayerSlot(currentSlot) && !Config.isPlayerSlot(originSlot)){
					container.swapItems(originSlot,currentSlot);
				}
				//if pulled from Object to Player
				if(Config.isPlayerSlot(currentSlot) && !Config.isPlayerSlot(originSlot)){
					Item tmp = player.takeItem(currentSlot);
					player.putItem(container.takeItem(originSlot),currentSlot);
					container.putItem(tmp, originSlot);
				}
				//if pulled from Player To Object
				if(!Config.isPlayerSlot(currentSlot) && Config.isPlayerSlot(originSlot)){
					Item tmp = container.takeItem(currentSlot);
					container.putItem(player.takeItem(originSlot),currentSlot);
					player.putItem(tmp, originSlot);
				}

				target.transform.position = hit.collider.transform.position + Vector3.up * target.gameObject.GetComponent<Renderer>().bounds.size.y * 0.5f;
				// PrintInventory();

			} else {
				target.transform.position = originalTargetPosition;
			}
		}

		if(isDragging){
			Vector3 currentScreenSpace = new Vector3(Input.mousePosition.x, Input.mousePosition.y, positionOfScreen.z);
			Vector3 targetPosition = Camera.main.ScreenToWorldPoint(currentScreenSpace) + offsetValue;
			target.transform.position = targetPosition;
		}
		
	}

    private void useItem(GameObject target)
    {
       if(target.GetComponent<Consumable>() != null){
		   Consumable cons = target.GetComponent<Consumable>();
		   cons.interact(player);
		   player.playerAnimator.SetTrigger("consume");
		   Destroy(target);

	   }
    }

    GameObject GetClickedSlot(out RaycastHit hit){
		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		int layerMask = 1 << 9;
		GameObject t = null;
		// Debug.DrawRay(ray.origin, ray.direction, Color.blue, 1);
		if (Physics.Raycast(ray, out hit, Mathf.Infinity, layerMask)){
			t = hit.collider.gameObject;
		}
		return t;
	}

	GameObject GetClickedObject(out RaycastHit hit){
		GameObject t = null;
		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		int layerMask = 1 << 10;
		// Debug.DrawRay(ray.origin, ray.direction, Color.red, 1);
		if(Physics.Raycast(ray, out hit, Mathf.Infinity, layerMask)){
			t = hit.collider.gameObject;
		}

		return t;
	}

	void PrintInventory(){
		string output = "";

		foreach(KeyValuePair<eSlot, Item> pair in player.getItems()){
			if(pair.Value != null)
			output += pair.Key + ": " + pair.Value.itemname + " - ";
		}

		Debug.Log(output);
	}
}
