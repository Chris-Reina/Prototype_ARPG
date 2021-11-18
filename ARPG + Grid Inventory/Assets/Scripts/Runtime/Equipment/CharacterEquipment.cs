using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[Serializable]
public class CharacterEquipment
{
    private Dictionary<EquipmentItemSlot, EquipmentItem> _slotToItemMap;
    
    [Header("Weapons")]
    [SerializeField] private EquipmentItemPair weapons = default;
    
    [Header("Armor")]
    [SerializeField] private EquipmentItem mainArmor = default;
    [SerializeField] private EquipmentItem boots = default;
    [SerializeField] private EquipmentItem gloves = default;
    [SerializeField] private EquipmentItem helmet = default;
    
    [Header("Accessory")]
    [SerializeField] private EquipmentItem belt = default;
    [SerializeField] private EquipmentItem talisman = default;
    [SerializeField] private EquipmentItemPair rings = default;

    

    public bool IsOccupied(EquipmentItemSlot slot, bool isMain = false)
    {
        if (slot == null) return false;

        if (!_slotToItemMap.ContainsKey(slot)) return false;
        
        var eItem = _slotToItemMap[slot];

        if (eItem is EquipmentItemPair eItemPair)
        {
            return eItemPair.IsSlotOccupied(isMain);
        }

        return eItem.IsSlotOccupied();
    }

    public Item PeekItem(EquipmentItemSlot slot, bool isMain = false)
    {
        if (slot == null) return null;
        
        if (_slotToItemMap.ContainsKey(slot))
        {
            var eItem = _slotToItemMap[slot];

            if (eItem is EquipmentItemPair eItemPair)
            {
                return isMain ? eItemPair.mainItem : eItemPair.offItem;
            }

            return eItem.mainItem;
        }

        return null;
    }
    public bool PeekItem(EquipmentItemSlot slot, out Item item, bool isMain = false)
    {
        if (slot == null)
        {
            item = null;
            return false;
        }

        if (_slotToItemMap.ContainsKey(slot))
        {
            var eItem = _slotToItemMap[slot];

            if (eItem is EquipmentItemPair eItemPair)
                item = isMain ? eItemPair.mainItem : eItemPair.offItem;
            else
                item = eItem.mainItem;
        }
        else
        {
            item = null;
        }
        
        return item != null;
    }

    public Item GetItem(EquipmentItemSlot slot, bool isMain = false)
    {
        if (slot == null) return null;
        if (!_slotToItemMap.ContainsKey(slot)) return null;
        
        Item item;
        var eItem = _slotToItemMap[slot];

        if (eItem is EquipmentItemPair eItemPair)
        {
            if (isMain)
            {
                item = eItemPair.mainItem;
                eItemPair.mainItem = null;
            }
            else
            {
                item = eItemPair.offItem;
                eItemPair.offItem = null;
            }
        }
        else
        {
            item = eItem.mainItem;
            eItem.mainItem = null;
        }

        return item;

    }
    public void SetItem(Item item, bool isMain = false)
    {
        if (!_slotToItemMap.ContainsKey(item.slot)) return;
        
        if (_slotToItemMap[item.slot] is EquipmentItemPair eItemPair)
        {
            if (isMain)
                eItemPair.mainItem = item;
            else
                eItemPair.offItem = item;
        }
        else
        {
            _slotToItemMap[item.slot].mainItem = item;
        }
    }
    

    public void Initialize()
    {
        _slotToItemMap = new Dictionary<EquipmentItemSlot, EquipmentItem>
        {
            {weapons.AdmittedSlot, weapons},
            {mainArmor.AdmittedSlot, mainArmor},
            {boots.AdmittedSlot, boots},
            {gloves.AdmittedSlot, gloves},
            {helmet.AdmittedSlot, helmet},
            {belt.AdmittedSlot, belt},
            {talisman.AdmittedSlot, talisman},
            {rings.AdmittedSlot, rings}
        };
    }
    public void Release()
    {
        _slotToItemMap?.Clear();
    }
}
