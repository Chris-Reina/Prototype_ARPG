using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipmentSlotData
{
    public EquipmentItemSlot EquipmentItemSlot { get; }
    public UIEquipmentSlot.SlotType SlotType { get; }
    public bool IsMain { get; }

    public EquipmentSlotData(EquipmentItemSlot eSlot, UIEquipmentSlot.SlotType tSlot, bool isMain)
    {
        EquipmentItemSlot = eSlot;
        SlotType = tSlot;
        IsMain = isMain;
    }
}
