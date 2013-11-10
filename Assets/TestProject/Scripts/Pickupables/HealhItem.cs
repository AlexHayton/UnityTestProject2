using UnityEngine;
using System.Collections;

public class HealhItem : Pickupable {
		
	public int addedHealth = 10;
	
	HealthHandler _healthHandler;

	public override bool CanBePickedUpBy(GameObject player)
	{
		bool canBePickedUp = false;
		
		_healthHandler =(HealthHandler)player.GetComponent<HealthHandler>();
		if (_healthHandler) {
			if (_healthHandler.health < _healthHandler.maxHealth) {
				canBePickedUp = true;
			}
		}
		
		return canBePickedUp;
	}
		
	public override bool OnPickUp (GameObject player)
	{
		bool success = true;

		if (_healthHandler) {
			_healthHandler.AddHealth(addedHealth);
			success = true;
		}
		
		return success;
	}
		
}
