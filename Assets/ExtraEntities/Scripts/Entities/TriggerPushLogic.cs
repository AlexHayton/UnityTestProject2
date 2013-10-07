using UnityEngine;
using System.Collections;

public class TriggerPushLogic : MonoBehaviour {

	void OnTriggerStay(Collider other) {
		CharacterController test = other.GetComponent<CharacterController>();
		if (test) {
			test.velocity.Set(0,1000, 0);
		} else {
			other.rigidbody.AddForce(Vector3.up * 100);
		}
	}
}
