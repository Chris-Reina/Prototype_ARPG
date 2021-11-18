using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EquipmentItemSlot", menuName = "Item/Slot/EquipmentItem")]
public class EquipmentItemSlot : ScriptableObject
{
    public virtual string SlotTag => GetType().ToString();
}
