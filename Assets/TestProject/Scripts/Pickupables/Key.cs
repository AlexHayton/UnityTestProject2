using UnityEngine;
using System.Collections;

public class KeyItem : Pickupable {
	
	public string name;
	public Color color;
	public Texture2D icon;
	
	KeyManager manager = null;
	
	public override void Start()
	{
		base.Start();
		GameObject gameManager = GameObject.FindWithTag("GameManager");
		if (gameManager)
		{
			manager = gameManager.GetComponent<KeyManager>();
		}
		
		if (manager == null)
		{
			Debug.LogError("Cannot find key manager");
		}
	}

	public override bool CanBePickedUpBy(GameObject player)
	{
		return true;
	}
		
	public override bool OnPickUp (GameObject player)
	{
		bool success = true;

		if (manager) {
			manager.PickupKey(key);
			success = true;
		}
		
		return success;
	}
		
}
