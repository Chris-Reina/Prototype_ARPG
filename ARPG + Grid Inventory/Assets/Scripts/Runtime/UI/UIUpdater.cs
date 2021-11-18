using System;
using System.Collections;
using System.Collections.Generic;
using DoaT;
using UnityEngine;

public class UIUpdater : MonoBehaviour
{
    public InventoryData inventoryData;
    public InventoryActionData actionData;
    public CursorGameSelection selectionManager;

    private void Start()
    {
        EventManager.Subscribe(EventsData.OnInventoryActionUpdate, UpdateActionData);
    }

    private void Update()
    {
        if (selectionManager.extendedMovement) return;
        
        EventManager.Trigger(EventsData.OnInventoryActionUpdate, null);
        EventManager.Trigger(EventsData.OnInventoryDraw, null);
    }
    
    private void UpdateActionData(params object[] parameters)
    {
        actionData.isItemInCursor = inventoryData.cursorItem != null;
    }
}
