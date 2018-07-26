using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Consumable : Item_Mono {
	[Header("Effects when consumed")]
	public float effectOnHunger;
	public float effectOnSleep;
	public float effectOnStability;
	public float effectOnSanity;
	public Consumable(string _name, int _noticeable, bool _permitted, float _hunger, float _sleep, float _stability, float _sanity) : base(_name, _noticeable, _permitted){
		effectOnHunger = _hunger;
		effectOnSleep = _sleep;
		effectOnStability = _stability;
		effectOnSanity = _sanity;
	}

	public void interact(Player p){
		p.eat(effectOnHunger);
		p.sleepForHours(effectOnSleep);
		p.addStability(effectOnStability);
		p.addSanity(effectOnSanity);
		
		//for the effects
		p.cam.GetComponent<CameraScript>().targetDistortion = effectOnSanity;
		p.movementSpeed *= (effectOnSanity / 10); 
		
	}
}
