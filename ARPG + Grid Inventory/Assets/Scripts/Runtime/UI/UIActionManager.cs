using System;
using System.Collections;
using System.Collections.Generic;
using DoaT;
using Substance.Game;
using UnityEngine;

public class UIActionManager : MonoBehaviour
{
    public InventoryData inventoryData;
    public InventoryActionData inventoryActionData;
    public CursorGameSelection selection;

    [SerializeField] private UIInventory inventory;
    [SerializeField] private UICharacterEquipment characterEquipment;

    private void Start()
    {
        EventManager.Subscribe(EventsData.OnInteractionWithUI, OnInteractionWithUI);
    }

    private void OnInteractionWithUI(params object[] parameters)
    {
        var element = (ITargetableUI) parameters[0];
        
        switch (element)
        {
            case UIEquipmentSlot equipmentSlot:
                inventoryData.InteractEquipment?.Invoke(new EquipmentSlotData(equipmentSlot.ItemSlot,
                                                                                   equipmentSlot.Slot, 
                                                                                   equipmentSlot.IsMain));
                break;
            case UIInventorySlot _:
                inventoryData.InteractInventory?.Invoke();
                break;
        }
    }
}


