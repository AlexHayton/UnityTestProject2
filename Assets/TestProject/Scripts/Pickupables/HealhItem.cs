using UnityEngine;
using System.Collections;

public class HealhItem : Pickupable {
		
	public int addedHealth = 10;

	public override bool CanBePickedUpBy(GameObject player)
	{
		bool canBePickedUp = false;
		
		HealthHandler healthHandler = (HealthHandler)player.GetComponent<HealthHandler>();
		if (healthHandler) {
			if (healthHandler.health < healthHandler.maxHealth) {
				canBePickedUp = true;
			}
		}
		
		return canBePickedUp;
	}
		
	public override bool OnPickUp (GameObject player)
	{
		bool success = true;
		
		HealthHandler healthHandler = (HealthHandler)player.GetComponent<HealthHandler>();
		if (healthHandler) {
			if (healthHandler.health < healthHandler.maxHealth) {
				healthHandler.AddHealth(addedHealth);
				success = true;
			}
		}
		
		return success;
	}
		
}
