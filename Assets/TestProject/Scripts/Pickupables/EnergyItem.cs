using UnityEngine;
using System.Collections;

public class EnergyItem : Pickupable {
		
	public int addedEnergy = 10;
	
	private EnergyHandler _energyHandler;

	public override bool CanBePickedUpBy(GameObject player)
	{
		bool canBePickedUp = false;
		
		_energyHandler = (EnergyHandler)player.GetComponent<EnergyHandler>();
		if (_energyHandler) {
			if (_energyHandler.energy < _energyHandler.maxEnergy) {
				canBePickedUp = true;
			}
		}
		
		return canBePickedUp;
	}
		
	public override bool OnPickUp (GameObject player)
	{
		bool success = true;

		if (_energyHandler) {
			_energyHandler.AddEnergy(this.addedEnergy);
			success = true;
		}
		
		return success;
	}
		
}
