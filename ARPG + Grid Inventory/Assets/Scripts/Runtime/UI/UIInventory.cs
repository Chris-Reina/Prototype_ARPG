using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using DoaT;
using UnityEngine;

public enum CardinalDirection
{
    Left,
    Right,
    Up,
    Down
}

public class UIInventory : MonoBehaviour
{
    public CursorGameSelection selection;
    public InventoryData inventoryData;
    public InventoryActionData actionData;
    public InventoryGrid grid;

    private void Awake()
    {
        grid = FindObjectOfType<InventoryGrid>();
    }

    private void Start()
    {
        EventManager.Subscribe(EventsData.OnInventoryActionUpdate, UpdateActionData);
    }

    private void UpdateActionData(params object[] parameters)
    {
        var currentPointerTarget = selection.GetPointerTarget();
        
        if (currentPointerTarget == null)
        {
            actionData.slots = new List<UIInventorySlotData>();
        }
        else if (currentPointerTarget is UIInventorySlot slot)
        {
            actionData.targetSlot = new UIInventorySlotData(slot);

            if (actionData.isItemInCursor)
            {
                var temp = InventoryUtility.GetInventorySlotList(inventoryData.cursorItem.inventorySize, slot, slot.Grid);
                var data = temp.Select(uiSlot => new UIInventorySlotData(uiSlot)).ToList();

                actionData.slots = data;
            }
            else
            {
                actionData.slots = inventoryData.GetItemPositionFromSlot(slot)
                    .Select(pos => new UIInventorySlotData(grid.gridSlots[pos.x, pos.y]))
                    .ToList();
            }
        }
    }
}
