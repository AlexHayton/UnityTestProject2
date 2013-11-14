using UnityEngine;
using System.Collections;

public class PlayerSpawner : MonoBehaviour {
	
	public GameObject playerPrefab;
	public Quaternion initialRotation;
	public List<GameObject> guiPrefabs = new List<GameObject>();
	
	void Start() {
		// Spawn the player
		Vector3 spawnPos = this.GetSpawnPosition();
		GameObject newPlayer = Instantiate(playerPrefab, spawnPos, initialRotation) as GameObject;
		
		// Create and register GUIs
		GameObject guiAttachPoint = newPlayer.FindChild("GUI");
		this.CreateAndRegisterGUI(guiAttachPoint.transform, guiPrefabs);
	}
	
	private CreateAndRegisterGUI(Transform guiAttachPoint, List<GameObject> guiPrefabList)
	{
		GUIHandler handler = newPlayer.GetComponent<GUIHandler>();
		foreach (GameObject guiPrefab in guiPrefabs)
		{
			GameObject guiInstance = Instantiate(guiPrefab, spawnPos, initialRotation) as GameObject;
			
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
