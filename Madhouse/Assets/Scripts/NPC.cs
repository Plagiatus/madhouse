using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class NPC : MonoBehaviour, iHumanoid {
    public float movementSpeed
    {
        get
        {
            return movementSpeed;
        }

        set
        {
            movementSpeed = value;
        }
    }

    public void moveTo(Vector2 pos)
    {
        throw new System.NotImplementedException();
    }

	[Range(0,5)]
    protected int accessLevel;
	protected Mesh model;
	protected Mesh distortedModel;
	protected eState state;
	[Range(0,5)]
	protected int resistance;
	protected AudioListener al;

	public abstract void AI();
}
