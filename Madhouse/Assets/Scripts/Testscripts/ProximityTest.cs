using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProximityTest : MonoBehaviour {
	private Rigidbody rb;

	void Start(){
		rb = this.GetComponent<Rigidbody>();
	}

	void Update () {
		// if(Input.GetKey(KeyCode.W)){
		// 	rb.AddForce(Vector3.forward);
		// }		
	
		// if(Input.GetKey(KeyCode.S)){
		// 	rb.AddForce(Vector3.back);
		// }
		// if(Input.GetKey(KeyCode.A)){
		// 	rb.AddForce(Vector3.left);
		// }
		// if(Input.GetKey(KeyCode.D)){
		// 	rb.AddForce(Vector3.right);
		// }

		Collider[] closeColliders = Physics.OverlapSphere(transform.position, Config.highlightDistance);
		foreach(Collider c in closeColliders){
			if(c is BoxCollider && c.gameObject.GetComponent<ProximityOutlineObject>() != null) c.gameObject.GetComponent<ProximityOutlineObject>().showOutline((this.transform.position - c.gameObject.transform.position).magnitude);
		}
	}
}
