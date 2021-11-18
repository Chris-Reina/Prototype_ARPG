using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

[Serializable]
public class LootTableWeightedDrop
{
    [Range(0f, 100f)] public float _dropChance = 5f;
    public List<Item> items;

    public Item RollForItem()
    {
        return RollForItem(out var success);
    }

    public Item RollForItem(out bool successful)
    {
        var rand = Random.Range(0, 100f);

        if (rand <= _dropChance)
        {
            successful = true;
            return items[Random.Range(0, items.Count)];
        }
        
        successful = false;
        return null;
    }
}
