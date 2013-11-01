using UnityEngine;
using System.Collections;

[RequireComponent (typeof (CapsuleCollider))]
public class Pickupable : MonoBehaviour {

	public GameObject pickUpEffectPrefab;
	
	
	void  OnTriggerEnter (Collider collision) {
		if (collision.tag == "Player") {
			OnPickUp(collision.gameObject);
			
			if (pickUpEffectPrefab) {
				GameObject test = (GameObject)Instantiate(pickUpEffectPrefab, transform.position, transform.rotation);
				Destroy (test, 0.5f);	
			}
			
			Destroy (this.gameObject);
		}
	}
	
	
	void OnBecameInvisible (){
		Destroy (this.gameObject);
	}
	
	
	public virtual void OnPickUp(GameObject player) {			
	}
}
