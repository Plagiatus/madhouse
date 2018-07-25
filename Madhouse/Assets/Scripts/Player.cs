using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour, iHumanoid
{
	#region iHumanoid
    public float movementSpeed;

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
	private Rigidbody rb;
	private Vector3 defaultCameraPositon;
	#endregion

	#region UnityMethods
	
	void Start(){
		movementSpeed = 4f;
		rb = GetComponent<Rigidbody>();
		defaultCameraPositon = cam.transform.position;
	}

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

		public Item takeItem(eSlot slot){
			Item ret = items[slot];
			items[slot] = null;
			return ret;
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
			//move forward
			if (-Input.acceleration.z < 1 && Input.acceleration.z > -1)
        	{
				move(-Input.acceleration.z * Time.deltaTime * movementSpeed * Config.sensitivity);
			}
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
			//TODO: rewrite to use rigidbody
			transform.Translate(0, 0, distance);
		}

		private void turn(float speed){
			transform.Rotate(0, speed, 0);
		}

		private void moveCameraBehindPlayer(){
			RaycastHit hit;
			Vector3 hitPoint;
			
			Ray ray = new Ray(this.transform.position + Vector3.up * 0.5f, transform.forward * -1);
			Debug.DrawRay(ray.origin, ray.direction, Color.green, 1);
			// Debug.Log(Physics.Raycast(ray, out hit, 2.5f));
			if (Physics.Raycast(ray, out hit, 2.5f)){
				hitPoint = hit.point;
				// Debug.Log("hit");
				// Debug.Log(hitPoint);
				// worldposition auf lokalposition setzen
				cam.transform.localPosition = this.transform.InverseTransformPoint(hitPoint);
				Debug.Log(cam.transform.localPosition);
			}
			// camera auf standard setzen
			else { 
				cam.transform.localPosition = defaultCameraPositon; 
				// Debug.Log("no hit"); 
			}
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
