using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Linq;

public class PickupDropManager : MonoBehaviour, ISelfTest
{
    [Serializable]
    public class DroppableItem
    {
        public GameObject Item;
        public int DropChance;
    }

    public List<DroppableItem> Items; 
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
        
        return fail;
    }

    private IEnumerable<WeightedRandomItem<GameObject>> GetAsWeightedRandomItems()
    {
        return Items.Select(item => new WeightedRandomItem<GameObject>(item.Item, item.DropChance));
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
