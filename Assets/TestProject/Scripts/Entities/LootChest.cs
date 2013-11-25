using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using System.Collections;
using Random = UnityEngine.Random;

public class LootChest : MonoBehaviour
{
    [Serializable]
    public class Loot
    {
        public Loot()
        {
            print("Loot constructor");
        }
        public GameObject Item;
        public int PercentageWithinTier;
    }

    [Serializable]
    public class ItemTier
    {
        public int Tier;
        public List<Loot> Objects;
        private float AccumulatedPoints;
        private float SpawnThreshold;
        private WeightedRandom<GameObject> picker;
        
        private IEnumerable<WeightedRandomItem<GameObject>> GetAsWeightedRandomItems()
        {
            foreach (var loot in Objects)
            {
                print(loot.PercentageWithinTier + ", " + loot.Item);
            }
            return Objects.Select(item => new WeightedRandomItem<GameObject>(item.Item, item.PercentageWithinTier));
        }

        private GameObject NextItem()
        {
            if (picker == null)
            {
                var weightedItems = GetAsWeightedRandomItems();
                picker = new WeightedRandom<GameObject>(weightedItems.ToArray());
            }
            return picker.Next();
        }

        public List<GameObject> GetDrops(float percentage)
        {
            SpawnThreshold = Random.Range(0f, 1f);
            AccumulatedPoints += percentage * .5f;// a math thing - either this or make the random.range(0,2f)
            var objectsToReturn = new List<GameObject>();

            //put this in a loop to allow for multiple drops on one kill
            while (AccumulatedPoints > SpawnThreshold)
            {
                AccumulatedPoints -= SpawnThreshold;
                SpawnThreshold = Random.Range(0f, 1f);
                objectsToReturn.Add(NextItem());
            }
            return objectsToReturn;
        }
    }

    public List<ItemTier> Tiers;

    // Use this for initialization
    void Start()
    {
    }

    public List<GameObject> AskForDrops(int tier, float percentage)
    {
        var thisTier = Tiers.FirstOrDefault(itemTier => itemTier.Tier == tier);
        return thisTier == null ? new List<GameObject>() : thisTier.GetDrops(percentage);
    }
}