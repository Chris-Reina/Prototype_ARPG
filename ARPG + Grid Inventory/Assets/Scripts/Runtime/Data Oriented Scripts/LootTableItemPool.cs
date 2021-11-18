using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable, CreateAssetMenu(fileName = "Loot Table Pool", menuName = "Item/Loot Table Pool")]
public class LootTableItemPool : ScriptableObject
{
    public List<LootTableWeightedDrop> drops;

    public List<Item> RollItemsDrop()
    {
        var items = new List<Item>();
        if (drops.Count == 0) return items;
        
        foreach (var drop in drops)
        {
            var rolledItem = drop.RollForItem(out var success);

            if (success)
                items.Add(rolledItem);
        }

        return items;
    }
}
