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
				newItem.layer = 10;
				newItem.tag = "Item";
			}
		}
	}
	
	public void addContainer(Container co){
		container = co;
		activateContainer();
		createItems();
	}

	void activateContainer(){
		transform.Find("Container").gameObject.SetActive(true);
		Dictionary<eSlot, Item> containerItems = container.getItems();
		
		items.Add(eSlot.LEFT, containerItems[eSlot.LEFT]);
		items.Add(eSlot.CENTER, containerItems[eSlot.CENTER]);
		items.Add(eSlot.RIGHT, containerItems[eSlot.RIGHT]);
	}

	void Update () {

		//START DRAG
		if(Input.GetMouseButtonDown(0)){
			RaycastHit hitInfo;
			target = GetClickedObject(out hitInfo);
			if(target != null && target.gameObject.tag == "Item"){
				if(GetClickedSlot(out hitInfo) != null) originSlot = Config.gameObjectToEnum(GetClickedSlot(out hitInfo));
				// Debug.Log("origin: " + originSlot);
				isDragging = true;
				originalTargetPosition = target.transform.position;
				positionOfScreen = Camera.main.WorldToScreenPoint(target.transform.position);
				offsetValue = target.transform.position - Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, positionOfScreen.z));
			} else {
				target = null;
			}
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
