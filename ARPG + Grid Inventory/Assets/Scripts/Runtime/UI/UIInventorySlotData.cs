using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class UIInventorySlotData
{
    public Vector2Int Position;
    public int itemID = -1;

    public UIInventorySlotData(UIInventorySlot slot)
    {
        this.Position = slot.Position;
        itemID = slot.ItemID;
    }

}
