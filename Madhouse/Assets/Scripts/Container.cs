using System.Collections.Generic;
using UnityEngine;

public class Container : MonoBehaviour, iInteractable{
	protected Dictionary<eSlot, Item> items;


	public Item[] startItems;

	void Start(){
		// items = new Dictionary<eSlot, Item>(){
		// 	{eSlot.CENTER, new Item("Item1",10,false)},
		// 	{eSlot.LEFT, null},
		// 	{eSlot.RIGHT, null}
		// };
		items= new Dictionary<eSlot, Item>();
		for(int i = 0; i < startItems.Length; i++){
			eSlot slot = (eSlot) i + 3;
			if(startItems[i].itemname != ""){
				items.Add(slot, startItems[i]);
			}
			else {
				items.Add(slot, null);
			}
		}
	}

    public void interact()
    {
        throw new System.NotImplementedException();
    }

	public Dictionary<eSlot, Item> getItems(){
		return items;
	}

	public Item takeItem(eSlot slot){
		Item ret = items[slot];
		items[slot] = null;
		return ret;
	}

	public Item putItem(Item item, eSlot slot){
		Item ret = items[slot];
		items[slot] = item;
		return ret;
	}

	public bool swapItems(eSlot slot1, eSlot slot2){
		if(items[slot1] == items[slot2]) return false;
		
		Item tmp = items[slot1];
		items[slot1] = items[slot2];
		items[slot2] = tmp;

		return true;
	}

}
