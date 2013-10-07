using UnityEngine;
using System.Collections;

public class DamageHandler : MonoBehaviour {
	
	public int damage = 10;
	public float fireRate = 0.5f;
	public int range = 2;
	
	private float nextFire = 0.0f;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetButton ("Fire1") & Time.time > nextFire) {
			nextFire = Time.time + fireRate;
			Debug.Log ("Fire");
			
			// look if theres an enemy in front of us
			// todo: change this to "impact" of weapon etc
			RaycastHit hit;
			Vector3 fwd = transform.TransformDirection (Vector3.forward);
			if (Physics.Raycast (transform.position, fwd, out hit, range)) {
				Debug.Log ("Enemy in: " + hit.distance);
				HealthHandler handler = hit.collider.GetComponent<HealthHandler>();
				if (handler) {
					DoDamage (hit.collider);
					Debug.Log ("Damage done, Health left:" + handler.health);
				}
				
			}
				
		}
	}
	
	public void DoDamage(Component enemy) {	
		if (HealthUtility.DeductHealth(enemy, damage)) {
			Destroy (enemy.gameObject);
			XPHandler xpComponent = this.gameObject.GetComponent<XPHandler>();
			if (xpComponent != null)
			{
				xpComponent.AddXp(10);
			}
			Debug.Log ("Destroyed2");
		}
		
	}

}
