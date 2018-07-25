using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Key : Item {

	[Range(0,5)]
	private int accessLevel;

	public Key(string _name, int _noticeable, bool _permitted, int _accessLevel) : base(_name, _noticeable, _permitted){
		accessLevel = _accessLevel;
	}

	public int getAccessLevel(){
		return accessLevel;
	}
}
