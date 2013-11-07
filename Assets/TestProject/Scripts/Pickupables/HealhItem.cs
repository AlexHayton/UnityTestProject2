using UnityEngine;
using System.Collections;

public class HealhItem : Pickupable {
		
	public int addedHealth = 10;


	public override bool OnPickUp (GameObject player)
	{
		HealthHandler healthHandler = (HealthHandler)player.GetComponent<HealthHandler>();
		if (healthHandler) {
			if (healthHandler.health < healthHandler.maxHealth) {
				healthHandler.AddHealth(addedHealth);
				return true;
			}
		}
		
		return false;
	}
}
