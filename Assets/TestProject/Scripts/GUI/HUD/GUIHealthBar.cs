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
		float maxHealth = HealthUtility.GetMaxHealth (player);
		float health = HealthUtility.GetHealth (player);
		
		if (health > 0) {
			 return maxHealth / health;
		} else {
			 return 0;
		}
	}	
	
	public override string GetText (){
		float maxHealth = HealthUtility.GetMaxHealth (player);
		float health = HealthUtility.GetHealth (player);
		maxHealth = Mathf.Ceil (maxHealth);
		health = Mathf.Ceil (health);
		return ( health + "/" + maxHealth);
	}
}

