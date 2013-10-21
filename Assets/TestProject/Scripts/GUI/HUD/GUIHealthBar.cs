using UnityEngine;
using System.Collections;
using System;
	
public abstract class GUIHealthBar : GUIHorizontalBar {

	private GameObject player;

	public override void Start()
	{
		base.Start();
		player = PlayerUtility.GetPlayer ();
	}
	
	public override float GetPercentageFull()
	{
		HealthHandler handler = player.GetComponent<HealthHandler>();
		float maxHealth = handler.GetMaxHealth();
		float health = handler.GetHealth();
		
		if (health > 0) {
			 return maxHealth / health;
		} else {
			 return 0;
		}
	}	
	
	public override string GetText (){
		HealthHandler handler = player.GetComponent<HealthHandler>();
		float maxHealth = handler.GetMaxHealth();
		float health = handler.GetHealth();
		
		maxHealth = Mathf.Ceil (maxHealth);
		health = Mathf.Ceil (health);
		return ( health + "/" + maxHealth);
	}
}

