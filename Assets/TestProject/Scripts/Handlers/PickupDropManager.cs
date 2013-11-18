using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Linq;

public class PickupDropManager : MonoBehaviour, ISelfTest
{

    public List<int> chances;
    public List<GameObject> pickups;
    public int killValue = 1;
    public int DropEvery = 5;
    public int PlusOrMinus = 2;

    static int tryCounter;
    static int nextSpawnTime;
    private WeightedRandom<GameObject> picker;

    public void Start()
    {
        nextSpawnTime = UnityEngine.Random.Range(DropEvery - PlusOrMinus, DropEvery + PlusOrMinus);
        var weightedItems = GetAsWeightedRandomItems();
        picker = new WeightedRandom<GameObject>(weightedItems.ToArray());
        this.SelfTest();
    }

    public bool SelfTest()
    {
        bool fail = false;

        if (this.chances.Count != this.pickups.Count)
        {
            fail = true;
            Debug.Log("There must be as many entries in the pickups list as the chances list.");
        }

        return fail;
    }

    private IEnumerable<WeightedRandomItem<GameObject>> GetAsWeightedRandomItems()
    {
        return pickups.Select(pickup => new WeightedRandomItem<GameObject>(pickup, chances[pickups.IndexOf(pickup)]));
    }

    public void SpawnAPickup()
    {
        tryCounter += killValue;
        for (int i = 0; i < tryCounter / nextSpawnTime; i++)
        {
            nextSpawnTime = UnityEngine.Random.Range(DropEvery - PlusOrMinus, DropEvery + PlusOrMinus);
            var pickupToSpawn = picker.Next();
            Instantiate(pickupToSpawn, gameObject.transform.position, pickupToSpawn.transform.rotation);
        }

        tryCounter = tryCounter % nextSpawnTime;
    }
}
