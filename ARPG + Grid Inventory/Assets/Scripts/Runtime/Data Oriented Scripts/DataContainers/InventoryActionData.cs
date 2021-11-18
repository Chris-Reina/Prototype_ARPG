using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum InventoryAction
{
    Add,
    Take,
    Swap,
    Cancel
}

[Serializable,CreateAssetMenu(fileName = "InventoryActionData", menuName = "Data/Inventory/InventoryActionData")]
public class InventoryActionData : ScriptableObject
{
    public bool isItemInCursor;
    public bool allowsAdd;
    public bool allowsRemove;
    public bool allowsSwap;
    public bool allowsCancel;
    public UIInventorySlotData targetSlot;
    public List<UIInventorySlotData> slots;
    
    private void OnDisable()
    {
        isItemInCursor = false;
        allowsAdd = false;
        allowsRemove = false;
        allowsSwap = false;
        allowsCancel = false;
        targetSlot = default;
        slots = new List<UIInventorySlotData>();
    }
}
