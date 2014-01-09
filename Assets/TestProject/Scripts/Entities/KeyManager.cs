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
    	if (keyPrefabs.Count == 0)
    	{
    		foreach (GameObject location in GameObject.FindWithTag("KeyLocation"))
    		{
    			int rand = Random.IntRange(0, keyPrefabs.Count - 1);
    			GameObject prefab = keyPrefabs
    			GameObject key =
    		}
    	}
    }
    
    public void PickupKey()
    {
    }
}