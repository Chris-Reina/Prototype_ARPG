using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable,CreateAssetMenu(fileName = "ItemBase",menuName = "Item/Base")]
public class Item : ScriptableObject
{
    public int itemID;

    public new string name;
    public EquipmentItemSlot slot;
    public Sprite sprite;
    public Vector2Int inventorySize;
    public ItemQuality quality;
    public List<AttributeModifier> attributeModifiers;

    public InventoryItem ToInventoryItem(List<Vector2Int> positions)
    {
        return new InventoryItem(this, positions);
    }
}