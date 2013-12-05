using UnityEngine;
using System.Collections;
using TestProject;

public class DoorTrigger : MonoBehaviour {
	
	DoorScript myDoorScript;
	
	void Start() {
		myDoorScript = (DoorScript)transform.parent.GetComponent<DoorScript>();
	}
		
	
	void OnTriggerEnter(Collider other) {
		myDoorScript.OnEnter(other);
    }
	
	void OnTriggerStay(Collider other) {
		myDoorScript.OnEnter(other);
    }
}
