using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using System.Collections;
using Random = UnityEngine.Random;

public class KeyLocation : MonoBehaviour
{
	// Optionally set the key prefab here... if left blank it will be populated at random
	public GameObject keyPrefab;

	public void SpawnKey()
	{
		if (keyPrefab)
		{
			GameObject key = GameObject.Instantiate(keyPrefab, transform.position, transform.rotation) as GameObject;
			KeyItem keyItem = key.GetComponent<KeyItem>();
			keyItem.keyPrefab = keyPrefab;
		}
		else
		{
			Debug.LogError("Key location " + name + " does not have a key assigned...");
		}
	}
}