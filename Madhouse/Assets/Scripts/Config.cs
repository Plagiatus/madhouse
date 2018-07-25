using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Config {

	//Sensitivity should affect the speed of which certain events are triggered. higher numbers = faster trigger
	[Range(0,10)]
	public static float sensitivity = 2; 


	//Dictionary of what needs to be the mental state before an action can be done. (stability, sanity)
	public static Dictionary<eAction, Vector2> actionRequirements = new Dictionary<eAction, Vector2>() {
		{eAction.ATTACK, new Vector2(0,20)},
		{eAction.IDLE, new Vector2(0,-20)},
		{eAction.IDLE_SNEAK, new Vector2(0,-20)},
		{eAction.INTERACT, new Vector2(0,-20)},
		{eAction.INVENTORY, new Vector2(0,-20)},
		{eAction.KILL, new Vector2(0,50)},
		{eAction.SNEAK, new Vector2(20, 0)},
		{eAction.USE, new Vector2(0, -20)},
		{eAction.WALK, new Vector2(0, -20)},
	};
	

	//Dictionary of what needs to be the mental state before an action can be done. (stability, sanity)
	public static Dictionary<eAction, Vector2> actionConsequences = new Dictionary<eAction, Vector2>() {
		{eAction.ATTACK, new Vector2(-30,20)},
		{eAction.IDLE, new Vector2(0,0)},
		{eAction.IDLE_SNEAK, new Vector2(0,0)},
		{eAction.INTERACT, new Vector2(0,0)},
		{eAction.INVENTORY, new Vector2(0,0)},
		{eAction.KILL, new Vector2(-50,50)},
		{eAction.SNEAK, new Vector2(0, 0)},
		{eAction.USE, new Vector2(0, 0)},
		{eAction.WALK, new Vector2(0, 0)},
	};
	
}
