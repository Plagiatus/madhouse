using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Throwable : Item {
	private int state;

	public Throwable(string _name, int _noticeable, bool _permitted, int _state) : base(_name, _noticeable, _permitted){
		state = _state;
	}

	public delegate void thrown();
	public delegate void onCollisionEnter(Collision c);//TODO: Do whatever the thing does when it's colliding with something.
}
