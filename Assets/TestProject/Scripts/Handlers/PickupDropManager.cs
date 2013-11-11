using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Linq;

public class PickupDropManager : MonoBehaviour, ISelfTest {	
		
	public List<int> chances;
	public List<GameObject> pickups;
	public float PickupDropChance = 100;
	
	public void Start()
	{
		this.SelfTest();
	}
	
	public bool SelfTest()
	{
		bool fail = false;
		
		if (this.chances.Count != this.pickups.Count)
		{
			fail = true;
			Debug.Log ("There must be as many entries in the pickups list as the chances list.");
		}
		
		return fail;
	}
	
	private IEnumerable<WeightedRandomItem<GameObject>> GetAsWeightedRandomItems()
	{
		return pickups.Select(pickup => new WeightedRandomItem<GameObject>(pickup, chances[pickups.IndexOf(pickup)]));
	}
	
	public void SpawnAPickup()
	{
		float spawnRandom = UnityEngine.Random.Range(0.0f, 100.0f);
		// Only spawn once every so often.
		if (spawnRandom <= PickupDropChance)
		{
			IEnumerable<WeightedRandomItem<GameObject>> weightedItems = this.GetAsWeightedRandomItems();
			WeightedRandom<GameObject> picker = new WeightedRandom<GameObject>(weightedItems.ToArray());
			GameObject pickupToSpawn = picker.Next();
			Instantiate(pickupToSpawn, gameObject.transform.position, pickupToSpawn.transform.rotation);
		}
	}
}
