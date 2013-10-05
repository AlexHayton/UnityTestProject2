using UnityEngine;
using System.Collections;

public class EnemyHealthHud : BoxHud
{	
	
	public override void OnGUI ()
	{
		base.OnGUI ();
		
		// Get the point at the centre of the top of the hitbox.
		Collider collider = this.gameObject.collider; 
		Vector3 worldPoint = collider.bounds.center;
		worldPoint.y = collider.bounds.max.y;
		
		// Project the point from world space to screen space.
		if (this.GetWidth() > 0)
		{
			if (Camera.current != null)
			{
				Vector3 screenPosition = Camera.current.WorldToScreenPoint(worldPoint); // gets screen position.
				screenPosition.y = Screen.height - (screenPosition.y + 1);// inverts y
				this.SetTop(screenPosition.y - this.GetHeight());
				this.SetLeft(screenPosition.x - this.GetMaxWidth()/2.0f);
			}
		}
	}
	
	public float GetMaxWidth()
	{
		return 100;
	}
	
	private float GetHealth()
	{
		return HealthUtility.GetHealth (this);
	}
	
	private float GetMaxHealth()
	{
		return HealthUtility.GetMaxHealth (this);
	}
	
	public override float GetWidth ()
	{
		float tempWidth = this.GetMaxWidth();
		
		float maxHealth = this.GetMaxHealth();
		float health = this.GetHealth();
		
		if (health > 0 && health < maxHealth) {
			tempWidth /= (maxHealth / health);
		} else {
			tempWidth = 0;
		}

		return tempWidth; 
	}
	
	public override string GetText ()
	{
		return string.Empty;
	}
	
	public override Color GetColor ()
	{
		float maxHealth = this.GetMaxHealth();
		float health = this.GetHealth();
		if (health/maxHealth == 1.0f)
		{
			return new Color(0, 0, 0, 0);
		}
		else if (health/maxHealth >= 0.7f)
		{
			return Color.green;
		}
		else if (health/maxHealth >= 0.4f) 
		{
			return Color.yellow;
		}
		else
		{
			return Color.red;
		}
	}
	
	public override float GetHeight()
	{
		return 5;
	}

}
