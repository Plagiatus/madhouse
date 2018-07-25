using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour {
	[Range(0,5)]
	private int accessLevel;

	public int getAccessLevel(){
		return accessLevel;
	}
}
