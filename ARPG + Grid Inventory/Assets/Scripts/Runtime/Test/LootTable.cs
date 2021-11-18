using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LootTable : MonoBehaviour
{
    public LootTableItemPool pool;

    public List<Item> DropItems()
    {
        return pool.RollItemsDrop();
    }
}
