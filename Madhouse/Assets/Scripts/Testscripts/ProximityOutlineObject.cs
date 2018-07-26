using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProximityOutlineObject : MonoBehaviour {

	float cooldown = 1;
	float timer = 0;
	float intensity = 0;

	Outline outline;

	void Start () {
		outline = GetComponent<Outline>();
	}

	void Update () {
		Debug.Log(intensity);
		outline.OutlineWidth = intensity;
		// outline.OutlineColor = new Color(1, 1, 1, intensity / 100);
	}

	public void showOutline(float _intensity){
		intensity = 10 - _intensity;
	}
}
