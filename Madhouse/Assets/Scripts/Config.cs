using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Config {

	//Sensitivity should affect the speed of which certain events are triggered. higher numbers = faster trigger
	[Range(0.1f,2)]
	public static float sensitivity = 1; 

	public static float highlightDistance = 10;
	public static float interactionDistance = 2;


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
	
	public static bool isPlayerSlot(eSlot slot){
		if((int) slot < 3) return true;
		return false;
	}

	public static eSlot gameObjectToEnum(GameObject g){
		if (g.name == "Hand") return eSlot.HAND;
		if (g.name == "Center") return eSlot.CENTER;
		if (g.name == "Left") return eSlot.LEFT;
		if (g.name == "Right") return eSlot.RIGHT;
		if (g.name == "Leftpocket") return eSlot.LEFTPOCKET;
		if (g.name == "Rightpocket") return eSlot.RIGHTPOCKET;
		return eSlot.HAND;
	}

	public static string enumToNameString(eSlot slot){
		switch(slot){
			case eSlot.HAND:
			return "Hand";
			case eSlot.CENTER:
			return "Center";
			case eSlot.LEFT:
			return "Left";
			case eSlot.RIGHT:
			return "Right";
			case eSlot.LEFTPOCKET:
			return "Leftpocket";
			case eSlot.RIGHTPOCKET:
			return "Rightpocket";
			default:
			return null;
		}
	}
}
