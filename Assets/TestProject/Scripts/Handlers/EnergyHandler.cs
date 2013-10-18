using UnityEngine;
using System.Collections;

public class EnergyHandler : MonoBehaviour {	
		
	public float maxEnergy = 100;
	public float energy = 100;
	
	public float regenEnergyRate = 10.0f;
	public float regenSpeed = 0.2f;
	private float nextRegen = 0.0f;
	
	public bool infiniteEnergy = false;
	
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
					this.energy = Mathf.Min (this.energy + this.regenEnergyRate, maxEnergy);
					nextRegen = Time.time + regenSpeed;
				}
				
			}
		}
	}
	
	public void AddEnergy(int energy) {		
		// not greater than maxHealth
		this.energy = Mathf.Min (this.energy + energy, maxEnergy);		
	}
	
	public bool DeductEnergy(int energy) {	
		
		int newEnergy = Mathf.Max (this.energy - energy, 0);
		
		if (invincible)
		{
			newEnergy = this.energy;
		}
		else
		{
			// no negative health
			this.energy = newEnergy;
		}
		
		return this.energy > 0;
	}

}
