using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class InventoryItem
{
    public Item item;
    
    public List<Vector2Int> positions;

    public InventoryItem(){}
    public InventoryItem(Item item)
    {
        this.item = item;
        
    }
    public InventoryItem(Item item, List<Vector2Int> positions)
    {
        this.item = item;
        this.positions = positions;
    }

    public Item ToItem()
    {
        return item;
    }
}
