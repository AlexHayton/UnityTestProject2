using UnityEngine;
using System.Collections;
using System;
	
[ExecuteInEditMode()]
public class GUIHealthBar : GUIHorizontalBar {

	private GameObject player;

	public override void Start()
	{
		base.Start();
		player = PlayerUtility.GetPlayer ();
	}
	
	public override float GetFullScalar()
	{
		if (Application.isPlaying)
		{
			HealthHandler handler = player.GetComponent<HealthHandler>();
			float maxHealth = handler.GetMaxHealth();
			float health = handler.GetHealth();
			
			if (health > 0) {
				 return health / maxHealth;
			} else {
				 return 0;
			}
		}
		else
		{
			return 1.0f;
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

