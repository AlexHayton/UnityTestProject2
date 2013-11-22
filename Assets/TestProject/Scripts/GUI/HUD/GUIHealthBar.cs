using UnityEngine;
using System.Collections;
using System;
using TestProject;
	
[ExecuteInEditMode()]
public class GUIHealthBar : GUIHorizontalBar {

	private HealthHandler handler;

	public override void Start()
	{
		base.Start();
		owner = PlayerUtility.GetLocalPlayer ();
		handler = owner.GetComponent<HealthHandler>();
	}
	
	public override float GetFullScalar()
	{
		if (Application.isPlaying)
		{
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
		float maxHealth = handler.GetMaxHealth();
		float health = handler.GetHealth();
		
		maxHealth = Mathf.Ceil (maxHealth);
		health = Mathf.Ceil (health);
		return ( health + "/" + maxHealth);
	}
}

