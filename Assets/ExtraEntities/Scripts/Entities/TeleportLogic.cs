using UnityEngine;
using System.Collections;

public class TeleportLogic : Entity {
	
	public TeleportLogic exit;
	public float coolDown = 2f;
	
	private ArrayList coolDownObjects = new ArrayList();
	
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	void OnTriggerEnter(Collider other) {
		if (exit) {
			int index = coolDownObjects.IndexOf(other);
			if (index == -1) {
				other.transform.position = exit.transform.position;	
				other.transform.localRotation = exit.transform.localRotation;
				exit.AddCoolDownObject(other);
			} else {
				if (Time.time >= (float)coolDownObjects[index + 1]) {
					coolDownObjects.RemoveAt(index);
					coolDownObjects.RemoveAt(index);
				}
			}
		}
    }
	
	
	void OnTriggerStay(Collider other) {
		OnTriggerEnter (other);		
	}
	
	public void AddCoolDownObject(Collider other) {
		coolDownObjects.Add (other);
		coolDownObjects.Add (Time.time + coolDown);		
	}
}
