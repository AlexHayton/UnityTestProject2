using UnityEngine;
using System.Collections;

public class TriggerHurtLogic : Entity {
	
	public int damage = 10;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}	
		
	void OnTriggerEnter(Collider other) {
		if (isEnabled) {
			// TODO dynamcly add a health logic here
		}
    }
	
}
