using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "WeaponItemSlot", menuName = "Item/Slot/Weapon")]
public class WeaponItemSlot : EquipmentItemSlot
{
    public bool isMainWeaponSlot;
    public WeaponItemSlot slotPair;
    public List<Type> admittedTypes;
}