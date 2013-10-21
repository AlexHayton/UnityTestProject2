using UnityEngine;
using System.Collections;

public class EnergyHandler : MonoBehaviour {	
		
	public float maxEnergy = 100;
	public float energy = 100;
	
	public float regenEnergyRate = 10.0f;
	public float regenInterval = 0.2f;
	private float nextRegen = 0.0f;
	
	public bool infiniteEnergy = false;
	
	void Start () {
	}
	
	void Update () {
		if (regenEnergyRate > 0) {
			
			if (energy < maxEnergy) {				
							
				if (nextRegen == 0.0f) {
					nextRegen = Time.time + regenInterval;
				} else if (Time.time > nextRegen) {
					// regen health
					//Debug.Log ("Regen");
					this.energy = Mathf.Min (this.energy + this.regenEnergyRate, maxEnergy);
					nextRegen = Time.time + regenInterval;
				}
				
			}
		}
	}
	
	public float GetEnergy()
	{
		return this.energy;
	}
	
	public float GetMaxEnergy()
	{
		return this.maxEnergy;
	}
	
	public void AddEnergy(float energy) {		
		// not greater than maxHealth
		this.energy = Mathf.Min (this.energy + energy, maxEnergy);		
	}
	
	public bool DeductEnergy(float energy) {	
		
		float newEnergy = Mathf.Max (this.energy - energy, 0);
		
		if (infiniteEnergy)
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
