using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestRandomItem : MonoBehaviour
{
    public InventoryData data;
    public LootTable loot;

    private void Awake()
    {
        loot = GetComponent<LootTable>();
    }

    public void GenerateItem()
    {
        data.cursorItem = loot.DropItems()[0];
    }
}
