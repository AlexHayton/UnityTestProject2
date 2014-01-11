using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using System.Collections;
using TestProject;
using Random = UnityEngine.Random;

public class KeyManager : MonoBehaviour
{
	public List<GameObject> keyPrefabs;

    // Use this for initialization
    void Start()
    {
    	if (keyPrefabs.Count == 0)
    	{
    		Debug.LogWarning("There are no key prefabs assigned");
    	}
    }

    public void PopulateKeyLocations()
    {
    	GameObject[] keyLocationObjects = GameObject.FindGameObjectsWithTag("KeyLocation");
		List<KeyLocation> keyLocations = keyLocationObjects.Select (k => k.GetComponent<KeyLocation>()).ToList();
		GameObject[] doorLocationObjects = GameObject.FindGameObjectsWithTag("Door");
		List<DoorScript> doorLocations = doorLocationObjects.Select (d => d.GetComponent<DoorScript>()).ToList();
    
    	if (keyPrefabs.Count > 0)
 	    {
			List<GameObject> keyPrefabsToPop = new List<GameObject>(keyPrefabs);
			List<KeyLocation> keyLocationsToPop = keyLocations.Where(k => k.keyPrefab == null).ToList ();
			List<DoorScript> doorLocationsToPop = doorLocations.Where(d => d.keyPrefab == null).ToList ();

			keyPrefabs.Shuffle();
			keyLocationsToPop.Shuffle();
			doorLocationsToPop.Shuffle();

			for (int i = 0; i < doorLocationsToPop.Count; i++)
    		{
				int keyNum = keyPrefabs.Count % i;
				GameObject keyPrefab = keyPrefabsToPop[keyNum];
				DoorScript door = doorLocationsToPop[i];
				KeyLocation keyloc = keyLocationsToPop[i];

				// We know this is not null because we filtered beforehand.
				keyloc.keyPrefab = keyPrefabs[i];
				door.keyPrefab = keyPrefabs[i];
    		}
    	}
    	
    	// Now actually instantiate the keys
    	foreach (KeyLocation location in keyLocations)
    	{
    		location.SpawnKey();
    	}
    }
    
    public void PickupKey(GameObject keyPrefab)
    {
    }
}