using UnityEngine;
using System.Collections;

[RequireComponent (typeof (Collider))]

public class HealthHandler : MonoBehaviour {	
		
	public float maxHealth = 100;
	public float health = 100;
	public bool dead = false;
	
	public int regenHealth = 0;
	public float regenSpeed = 1f;
	private float nextRegen = 0.0f;
	
	public bool invincible = false;	
	public GameObject destroyPrefab;
	
	void Start () {
	}
	
	void Update () {
		if (regenHealth > 0) {
			
			if (health < maxHealth) {				
							
				if (nextRegen == 0.0f) {
					nextRegen = Time.time + regenSpeed;
				} else if (Time.time > nextRegen) {
					// regen health
					Debug.Log ("Regen");
					this.health = Mathf.Min (this.health + regenHealth, maxHealth);
					nextRegen = Time.time + regenSpeed;
				}
				
			}
		}
	}
	
	public void AddHealth(int health) {		
		// not greater than maxHealth
		this.health = Mathf.Min (this.health + health, maxHealth);		
	}
	
	public bool DeductHealth(int damage) {	
		if (!invincible) {
			// no negative health
			this.health = Mathf.Max (this.health - damage, 0);
		}
		
		this.dead = this.health <= 0;
		
		if (dead & gameObject.tag != "Player") {
			if (destroyPrefab) {
				GameObject test = (GameObject)Instantiate(destroyPrefab, transform.position, transform.rotation);
				Destroy (test, 0.5f);				
			}
			Destroy(gameObject);
		}
			
		// returns true if died
		return this.dead;
	}
	
	public bool GetIsAlive()
	{
		return !this.dead;
	}

}
