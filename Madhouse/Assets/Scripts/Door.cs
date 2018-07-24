using UnityEngine;

public class Door : MonoBehaviour, iInteractable
{
    public void interact()
    {
        throw new System.NotImplementedException();
    }

	[Range(0,5)]
	private int accessLevel;
	private bool open = false;
	private bool locked = true;
	private Room[] adjacentRooms;
	private bool isFinalExit;

	public bool setOpen(bool _open){
		open = _open;
		return open;
	} 

	public bool unlockDoor(int level){
		if(level >= accessLevel){
			locked = true;
		}

		return locked;
	}

	public bool isDoorLocked(){
		return locked;
	}

	private void findAccessLevel(){
		foreach (Room r in adjacentRooms){
			if (r.getAccessLevel() > accessLevel){
				accessLevel = r.getAccessLevel();
			}
		}
	}
}
