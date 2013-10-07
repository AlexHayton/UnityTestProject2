using UnityEngine;
using System.Collections;

public class PickupableLogic : Entity {
	
	public GameObject particle;
	public AudioSource soundEffect;
	
	void OnTriggerEnter(Collider other) {
		if (isEnabled) {
			// pick it up
			// play effect
			GameObject test = (GameObject)Instantiate(particle, transform.position, transform.rotation);
			Destroy (test, 2);
			Destroy (this.gameObject);
		}	
	}

}
