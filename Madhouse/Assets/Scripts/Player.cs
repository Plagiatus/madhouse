﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour, iHumanoid
{
	[Header("Attributes")]
	#region iHumanoid
    public float movementSpeed;


    public void moveTo(Vector2 pos)
    {
        throw new System.NotImplementedException();
    }
	#endregion

	#region Attributes
	public Item[] startItems;
	[Header("Stuff needed during Runtime")]
	public GameObject cam;
	public AudioSource normal;
	public AudioClip normalM;
	public AudioSource dep;
	public AudioClip depM;
	public AudioSource rage;
	public AudioClip rageM;
	public AudioSource invent;
	public AudioClip inventM;
	public AudioSource eating;
	public AudioClip eatingS;
	public AudioSource heartbeat;
	public AudioClip heartbeatS;
	private Dictionary<eSlot, Item> items;
	[Range(-20,80)]
	private float sanity = 0;
	[Range(0,100)]
	private float stability = 100;
	[Range(0,100)]
	private float hunger = 50;
	[Range(0,100)]
	private float sleep = 50;
	private eAction action;
	private AudioSource micinput;
	private Rigidbody rb;
	private Vector3 defaultCameraPositon;
	private bool inInventory = false;
	private int count = 0;


	// private Animator camAnim;
	public Animator camAnim;
	public Animator playerAnimator;
	#endregion

	#region UnityMethods
	
	void Start(){
		// camStandard = GetComponent<Animator>();
		movementSpeed = 4f;
		rb = GetComponent<Rigidbody>();
		defaultCameraPositon = cam.transform.position;
		// items = new Dictionary<eSlot, Item>() {
		// 	{eSlot.HAND, new Item("Item2",10,false)},
		// 	{eSlot.LEFTPOCKET, new Item("Item1", 80, true)},
		// 	{eSlot.RIGHTPOCKET, null}
		// };
		items = new Dictionary<eSlot, Item>();
		for(int i = 0; i < 3; i++){
			eSlot slot = (eSlot) i;
			if(i < startItems.Length){
				if(startItems[i].itemname != ""){
					items.Add(slot, startItems[i]);
				} else {
					items.Add(slot, null);
				}
			}
			else {
				items.Add(slot, null);
			}
		}

		normal.clip = normalM;
		dep.clip = depM;
		invent.clip = inventM;
		heartbeat.clip = heartbeatS;
		eating.clip = eatingS;
		rage.clip = rageM;
	}

	void Update(){
		inputManager();
		updateMentalState();
		updateAppearance();

		Debug.Log("hunger: " + hunger + " sleep: " + sleep + " stability: " + stability + " sanity: " + sanity);
	}

	#endregion

	#region ClassMethods
		#region publicMethods
		public Dictionary<eSlot, Item> getItems(){
			return items;
		} 

		public bool swapItems(eSlot slot1, eSlot slot2){
			if(items[slot1] == items[slot2]) return false;
			
			Item tmp = items[slot1];
			items[slot1] = items[slot2];
			items[slot2] = tmp;

			return true;
		}

		public bool putItem(Item item, eSlot slot){
			if(item == null) return false;
			items[slot] = item;
			return true;
		}

		public Item takeItem(eSlot slot){
			Item ret = items[slot];
			items[slot] = null;
			return ret;
		}	

		public void eat(float nutrition){
			hunger = Mathf.Clamp(hunger + nutrition, 0, 100);
			eating.Play();
		}

		public void sleepForHours(float hours){
			sleep = Mathf.Clamp(sleep + hours * 10, 0, 100);
		}

		public void addStability(float stability){
			this.stability = Mathf.Clamp(this.stability + stability, 0, 100);
		}

		public void addSanity(float sanity){
			//change severity of effect according to current stability. 50% - 150%
			this.sanity = Mathf.Clamp(this.sanity + (sanity * (((100 - this.stability)/100) + 0.5f)), -20, 80);
		}

		public void slowTime(bool slow){
			if(slow){
				//TODO: slow time
			} else {
				//TODO: resume time
			}
		}

		#endregion
		
		#region privateMethods
		private void inputManager(){
			// Debug.Log(count);
			//move forward
			// Debug.Log(count);
			if (((-Input.acceleration.z < 0.9 && -Input.acceleration.z > 0.1) || Input.acceleration.z > 0.1) && count >= Config.actionChangeThreshold * -1)
        	{
				// camAnim.SetBool("Inventory", false);	
				count = Mathf.Clamp(--count, -1* Config.actionChangeThreshold, Config.actionChangeThreshold);
				if (count <= Config.actionChangeThreshold *-1){
					move(-Input.acceleration.z);
				}
			}

			if(Input.acceleration.z < 0.1 && Input.acceleration.z > -0.1){
				playerAnimator.SetBool("isWalking",false);
				count = 0;
				inInventory = false;
				playerAnimator.SetBool("inInventory", false);
				cam.GetComponent<CameraScript>().transitionToState(false);
			}

			if (-Input.acceleration.z > 0.9 && count <= Config.actionChangeThreshold)
        	{
				// camAnim.SetBool("Inventory", false);
				count = Mathf.Clamp(++count, -1* Config.actionChangeThreshold, Config.actionChangeThreshold);
				if (count >= Config.actionChangeThreshold && !inInventory){	
					goToInventory();
				}
			}
			if(inInventory) return;
			//turn
			turn(Input.acceleration.x * Config.sensitivity * movementSpeed);

			//interact with object
			if(Input.GetMouseButtonDown(0)){
				Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
				RaycastHit hit;
				if(Physics.Raycast(ray, out hit)){
					if((this.transform.position - hit.transform.position).magnitude < Config.interactionDistance){
						InteractWithObject(hit.transform.gameObject);
					}
				}
			}
		}

		private void move(float distance){
			inInventory = false;
			playerAnimator.SetBool("inInventory", false);
			cam.GetComponent<CameraScript>().transitionToState(false);

			//TODO: rewrite to use rigidbody
			// if(distance > 0.1 || distance < -0.1){
				playerAnimator.SetBool("isWalking",true);
				transform.Translate(0, 0, distance * Time.deltaTime * movementSpeed * Config.sensitivity);
				// rb.AddRelativeForce(0, 0, distance * Time.deltaTime * movementSpeed * Config.sensitivity);
				
		 	// } else {
			// 	playerAnimator.SetBool("isWalking",false);
			// } 
			// if (sanity > 0 && sanity < 60) {
			// 	normal.Play();
			// }
			// else if (sanity < 0){
			// 	dep.Play();
			// 	heartbeat.PlayDelayed(5);
			// }
			// else {
			// 	rage.Play();
			// 	heartbeat.Play();
			// }
		}

		private void turn(float speed){
			transform.Rotate(0, speed, 0);
		}

		private void goToInventory(){
			inInventory = true;
			playerAnimator.SetBool("inInventory", true);
			cam.GetComponent<CameraScript>().transitionToState(true);
			invent.Play();
		}

		private void InteractWithObject(GameObject go){
			if(go.GetComponent<Door>() != null){
				Door door = go.GetComponent<Door>();
				//TODO: implement Door interaction
			} else if (go.GetComponent<Container>() != null){
				Container container = go.GetComponent<Container>();
				//TODO: implement Container interaction
			}
		}

		private void updateMentalState(){
			//TODO: update the hunger, sleep, stability and sanity every frame
		}

		private void updateAppearance(){
			//TODO: update the Appearance depending on the current action as well as the different mental states
		}

		#endregion

	#endregion
}
