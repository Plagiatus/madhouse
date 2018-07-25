using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour, iHumanoid
{
	#region iHumanoid
    public float movementSpeed{get{return movementSpeed;} set{movementSpeed = value;}}
	public Animator camAnim;

    public void moveTo(Vector2 pos)
    {
        throw new System.NotImplementedException();
    }
	#endregion

	#region Attributes
	public GameObject cam;
	private Dictionary<eSlot, Item> items;
	[Range(-20,80)]
	private float sanity;
	[Range(0,100)]
	private float stability;
	[Range(0,100)]
	private float hunger;
	[Range(0,100)]
	private float sleep;
	private eAction action;
	private AudioSource micinput;
	#endregion

	void Start(){
		camAnim = GetComponent<Animator>();
	}

	#region UnityMethods
	void Update(){
		inputManager();
		updateMentalState();
		updateAppearance();
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

		public void eat(float nutrition){
			hunger = Mathf.Clamp(hunger + nutrition, 0, 100);
		}

		public void sleepForHours(float hours){
			sleep = Mathf.Clamp(sleep + hours * 10, 0, 100);
		}

		public void addStability(float stability){
			this.stability = Mathf.Clamp(this.stability + stability, 0, 100);
		}

		public void addSanity(float sanity){
			this.sanity = Mathf.Clamp(this.sanity + sanity, -20, 80);
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
			//TODO: create logic to move & interact
		}

		private void move(float distance){
			float speed = 4.0f;
			float temp1 = -Input.acceleration.z * Time.deltaTime * speed;

			if (-Input.acceleration.z < 0.8 && Input.acceleration.z > -0.7)        	{
				transform.Translate(0, 0, temp1);
			}
		}

		private void turn(float speed){
			transform.Rotate(0, speed * Input.acceleration.x, 0);
		}

		private void goToInventory(){
			if (-Input.acceleration.z > 0.8) {
				camAnim.Play("CamAnim");
			}
		}

		private void InteractWithObject(GameObject go){
			//TODO: implement the interaction with different objects after the interaction has been detected by the input Manager
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
