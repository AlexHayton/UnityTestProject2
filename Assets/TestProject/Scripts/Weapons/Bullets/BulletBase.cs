using UnityEngine;
using System.Collections;

[RequireComponent (typeof (CapsuleCollider))]
[RequireComponent (typeof (Rigidbody))]
public class BulletBase : MonoBehaviour {

	public GameObject destroyPrefab;
	public int damageOnHit = 10;
	public float speed = 20f;
	public float forceOnImpact = 20.0f;	
	
	private float dist = 50f;
	private GameObject owner;
	private Transform tr;
	private bool valuesSet = false;
	private string ignoreTag;
	
	void OnBecameInvisible (){
		Destroy (this.gameObject);	
	}
		
	// set the start values for the bullet
	public virtual void SetStartValues(GameObject owner, Vector3 forward) {
		this.owner = owner;
		this.ignoreTag = owner.tag;	
		this.rigidbody.velocity = forward * speed;
	}	
		
	//void  OnTriggerEnter (Collider collision) {
	void OnTriggerEnter(Collider enterObj) {
		if (enterObj.tag != ignoreTag & enterObj.tag != "Bullet") {
			
			// add force to the object
			if (enterObj.rigidbody) {
				enterObj.rigidbody.AddForceAtPosition (transform.forward, enterObj.transform.position, ForceMode.Impulse);			
			}
			
			// TODO
			// aply damage
			HealthHandler health = (HealthHandler)enterObj.GetComponentInChildren<HealthHandler>();			
			if (health) {
				health.DeductHealth(owner, damageOnHit);				
			}	
		}
	}
	
}
