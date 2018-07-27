using UnityEngine;

public class Door : MonoBehaviour, iInteractable
{
    public void interact()
    {
        if(!isDoorLocked())
            setOpen(!open);
    }

	[Range(0,5)]
	private int accessLevel;
	public bool open = false;
	private Room[] adjacentRooms;
	private bool isFinalExit;
    private Animator animator;
    private float openDuration = 0;
    private float maxOpenDuration = 5f;

    public bool openByDefault = false;
    public bool locked = true;

    public void Start()
    {
        animator = this.GetComponent<Animator>();
        if(openByDefault == true)
        {
            open = true;
            animator.SetTrigger("InstantOpen");
        }
    }
    public void Update()
    {
        // if(!openByDefault)
        // {
        //     if (openDuration >= 0)
        //     {
        //         openDuration = openDuration - Time.deltaTime;
        //     }
        //     else
        //     {
        //         openDuration = 0;
        //         animator.SetBool("Open", false);
        //     }
        // }
 
    }

    public bool setOpen(bool _open){
        if (_open != open)
        {
            openDuration = maxOpenDuration;
        }
        
        open = _open;
        animator.SetBool("Open", open);
        GetComponent<BoxCollider>().isTrigger = open;
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
