using System.Collections.Generic;
using UnityEngine;

public class Container : MonoBehaviour, iInteractable{
	protected Dictionary<eSlot, Item> items;

    public void interact()
    {
        throw new System.NotImplementedException();
    }

	public Dictionary<eSlot, Item> getItems(){
		return items;
	}

	public Item takeItem(eSlot slot){
		Item ret = items[slot];
		items.Remove(slot);
		return ret;
	}

	public Item putItem(Item item, eSlot slot){
		Item ret = items[slot];
		items[slot] = item;
		return ret;
	}

}
