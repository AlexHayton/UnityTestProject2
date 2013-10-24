using UnityEngine;
using System.Collections;

public class GUIEnemyHealth : GUIHorizontalBar, ISelfTest
{
	private GameObject enemy;

	public override void Start()
	{
		base.Start();
		enemy = this.gameObject;
		
		this.SelfTest();
	}
	
	public override bool SelfTest()
	{
		bool fail = false;
		
		fail = base.SelfTest();
		SelfTestUtility.HasComponent<HealthHandler>(ref fail, enemy);
		
		return fail;
	}
	
	private Vector2 GetScreenPixelDimensions()
	{
		Vector2 screenDimensions = new Vector2();
		screenDimensions.x = Screen.width;
		screenDimensions.y = Screen.height;
		return screenDimensions;
	}
	
	private Vector3 WorldToScreenPoint()
	{
		// Get the point at the centre of the top of the hitbox.
		Collider collider = this.gameObject.collider; 
		Vector3 worldPoint = collider.bounds.center;
		Vector3 screenPosition = new Vector3(0, 0, 0);
		worldPoint.y = collider.bounds.max.y;
		
		// Project the point from world space to screen space.
		if (this.GetPixelWidth() > 0)
		{
			if (Camera.current != null)
			{
				screenPosition = Camera.current.WorldToScreenPoint(worldPoint); // gets screen position.
				screenPosition.y = Screen.height - (screenPosition.y + 1);// inverts y
			}
		}
		
		return screenPosition;
	}
	
	public override float GetLeft()
	{
		Vector3 screenPosition = this.WorldToScreenPoint();
		return screenPosition.x - this.GetPixelWidth() / 2.0f;
	}
	
	public override float GetTop()
	{
		Vector3 screenPosition = this.WorldToScreenPoint();
		return screenPosition.y - this.GetPixelHeight();
	}
	
	public override float GetFullScalar()
	{
		if (Application.isPlaying)
		{
			HealthHandler handler = enemy.GetComponent<HealthHandler>();
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
	
	protected override bool GetIsVisible()
	{
		return base.GetIsVisible() && GetFullScalar() < 0.9f;
	}
	
	public override string GetText (){
		return string.Empty;
	}

}
