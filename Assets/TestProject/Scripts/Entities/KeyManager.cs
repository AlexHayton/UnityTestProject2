using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using System.Collections;
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
    	GameObject[] keyLocations = GameObject.FindWithTag("KeyLocation");
    	GameObject[] doorLocations = GameObject.FindWithTag("Door");
    	
    	IEnumerable<GameObject> keysToPopWith = keyPrefabs;
    	keyLocationsToPop = keyLocations.Where(k => k.keyPrefab == null);
    	doorLocationsToPop = doorLocations.Where(d => d.keyPrefab == null);
    
    	if (keyPrefabs.Count > 0)
 	   {
    		foreach (GameObject location in keyLocations)
    		{
    			int rand = Random.IntRange(0, keyPrefabs.Count - 1);
    			location.keyPrefab = keyPrefabs[rand];
    		}
    	}
    	
    	// Now actually instantiate the keys
    	foreach (GameObject location in keyLocations)
    	{
    			GameObject key = Instantiate(location.keyPrefab, location.transform.position, location.transform.rotation);
    	}
    }
    
    public void PickupKey()
    {
    }
}