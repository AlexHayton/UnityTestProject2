using UnityEngine;
using System.Collections;

public class HealthHud : BoxHud {	
	
	public override Color GetColor()
	{
		return new Color(1,0,0,1);
	}
	
	public override float GetWidth() {
		float tempWidth = width;
		float maxHealth = HealthUtility.GetMaxHealth (player);
		float health = HealthUtility.GetHealth (player);
		
		if (health > 0) {
			 tempWidth /= (maxHealth / health);
		} else {
			 tempWidth = 0;
		}

		return tempWidth; 
	}
	
	public override string GetText (){
		float maxHealth = HealthUtility.GetMaxHealth (player);
		float health = HealthUtility.GetHealth (player);
		maxHealth = Mathf.Ceil (maxHealth);
		health = Mathf.Ceil (health);
		return ( health + "/" + maxHealth);
	}

}
