using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using UnityEngine.EventSystems;

public class Item : MonoBehaviour {
	public string itemname;
	[Range(0, 100)]
	public int noticeable;
	public bool permitted;

	public Item(string _name, int _noticeable, bool _permitted){
		itemname = _name;
		noticeable = _noticeable;
		permitted = _permitted;
	}
}
