using UnityEngine;
using System.Collections;

public class HitHandler : MonoBehaviour {

	public bool allowAddForce = false;

	void Start () {
	
	}

	// Handles hits so you can do whatever you want, rotate, just ignore etc
	public virtual void OnHit(GameObject doer, Vector3 hitPos, Vector3 force, ForceMode mode) {
		if (allowAddForce) {
			if (rigidbody) {
				rigidbody.AddForceAtPosition (force, hitPos, mode);
			}
		}
	}

}
