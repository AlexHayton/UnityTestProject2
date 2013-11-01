using UnityEngine;
using System.Collections;

public class HealhItem : Pickupable {
		
	public int addedHealth = 20;


	public override void OnPickUp (GameObject player)
	{
		HealthHandler health = (HealthHandler)player.GetComponent<HealthHandler>();
		if (health) {
			health.AddHealth(addedHealth);
		}
	}
}
