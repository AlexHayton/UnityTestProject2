using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Linq;

public class PickupDropManager : MonoBehaviour, ISelfTest
{
    [Serializable]
    public class TierPercentage
    {
        public int Tier;
        public int Percentage;
    }
    public List<TierPercentage> TierPercentages;
 
    public void Start()
    {
        this.SelfTest();
    }

    public bool SelfTest()
    {
        bool fail = false;
        return fail;
    }

    

    public void SpawnPickups()
    {
        var pickupsToSpawn = new List<GameObject>();
        var chest = GameObject.FindWithTag("LootChest").GetComponent<LootChest>();
        foreach (var tierPercentage in TierPercentages)
        {
            pickupsToSpawn.AddRange(chest.AskForDrops(tierPercentage.Tier, tierPercentage.Percentage * .01f));
        }
        foreach (var pickupToSpawn in pickupsToSpawn)
        {
            Instantiate(pickupToSpawn, gameObject.transform.position, pickupToSpawn.transform.rotation);
        }
        
    }
}
