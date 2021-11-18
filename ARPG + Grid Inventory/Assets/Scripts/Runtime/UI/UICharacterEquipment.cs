using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class UICharacterEquipment : MonoBehaviour
{
    public InventoryData inventoryData;
    public InventoryActionData inventoryActionData;
    
    [Header("Slots")]
    public List<UIEquipmentSlot> equipmentSlots;
    
    private void Awake()
    {
        equipmentSlots = GetComponentsInChildren<UIEquipmentSlot>().ToList();
    }
}
