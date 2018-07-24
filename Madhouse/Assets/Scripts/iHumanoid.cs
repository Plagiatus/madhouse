using UnityEngine;

public interface iHumanoid{
	float movementSpeed {get; set;}
	void moveTo(Vector2 pos);
}