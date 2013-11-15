using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TestProject;

public class PlayerSpawner : MonoBehaviour {
	
	public GameObject playerPrefab;
	public Vector3 spawnOffset;
	public Quaternion initialRotation;
	public List<GameObject> guiPrefabs = new List<GameObject>();
	
	void Start() {
		// Spawn the player
		Vector3 spawnPos = this.GetSpawnPosition();
		GameObject newPlayer = Instantiate(playerPrefab, spawnPos + spawnOffset, initialRotation) as GameObject;
		
		// Create and register GUIs
		Transform guiAttachPoint = newPlayer.transform.FindChild("HUD");
		GUIHandler handler = newPlayer.GetComponent<GUIHandler>();
		this.CreateAndRegisterGUI(handler, guiAttachPoint, guiPrefabs);
	}
	
	private void CreateAndRegisterGUI(GUIHandler handler, Transform guiAttachPoint, List<GameObject> guiPrefabList)
	{
		foreach (GameObject guiPrefab in guiPrefabs)
		{
			GameObject guiInstance = Instantiate(guiPrefab, this.transform.position, initialRotation) as GameObject;
			
			// Attach the GUI GameObject here.
			if (guiAttachPoint)
			{
				guiInstance.transform.parent = guiAttachPoint;
			}
			
			// TODO: Find and register any GUIs here.
			if (handler)
			{
				//handler.RegisterGUI(guiType, guiInstance);
			}
		}
	}
		
	private Vector3 GetSpawnPosition()
	{
		// TODO: Make this a random point inside our box
		// For now, take the origin.
		return this.transform.position;
	}
}
