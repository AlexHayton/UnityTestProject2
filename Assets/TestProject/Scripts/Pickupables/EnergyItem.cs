using UnityEngine;
using System.Collections;

public class EnergyItem : Pickupable {
		
	public int addedEnergy = 10;

	public override bool CanBePickedUpBy(GameObject player)
	{
		bool canBePickedUp = false;
		
		EnergyHandler energyHandler = (EnergyHandler)player.GetComponent<EnergyHandler>();
		if (energyHandler) {
			if (energyHandler.GetEnergy() < energyHandler.GetMaxEnergy()) {
				canBePickedUp = true;
			}
		}
		
		return canBePickedUp;
	}
		
	public override bool OnPickUp (GameObject player)
	{
		bool success = true;
		
		EnergyHandler energyHandler = (EnergyHandler)player.GetComponent<EnergyHandler>();
		if (energyHandler) {
			if (energyHandler.GetEnergy() < energyHandler.GetMaxEnergy()) {
				energyHandler.AddEnergy(addedEnergy);
				success = true;
			}
		}
		
		return success;
	}
		
}
