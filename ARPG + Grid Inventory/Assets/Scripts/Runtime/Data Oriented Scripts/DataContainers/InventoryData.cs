using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[Serializable,CreateAssetMenu(fileName = "InventoryData",menuName = "Data/Inventory/InventoryData")]
public class InventoryData : ScriptableObject, IAwake
{
    public Item cursorItem;
    public List<InventoryItem> inventoryItems;
    public CharacterEquipment characterItems;
    
    public Func<bool> PointerOnUI;
    public Action<Vector2Int> ReleaseSlotID;
    public Action UpdateSlotID;
    public Action InteractInventory;
    public Action<EquipmentSlotData> InteractEquipment;

    public Vector2Int GetItemSizeFromItemSlot(UIInventorySlot slot)
    {
        var positions = new List<Vector2Int>();

        foreach (var item in inventoryItems)
        {
            foreach (var position in item.positions.Where(position => position == slot.Position))
            {
                positions = new List<Vector2Int>(item.positions);
            }

            if (positions.Count == 0)
                positions.Add(slot.Position);
        }

        var xSize = positions.Select(n => n.x).Distinct().ToList();
        var ySize = positions.Select(n => n.y).Distinct().ToList();

        var size = new Vector2Int(xSize.Count, ySize.Count);

        return size;
    }
    public List<Vector2Int> GetItemPositionFromSlot(UIInventorySlot slot)
    {
        var positions = new List<Vector2Int>();
        
        foreach (var item in inventoryItems)
        {
            foreach (var position in item.positions.Where(position => position == slot.Position))
            {
                positions = new List<Vector2Int>(item.positions);
            }

            if (positions.Count == 0)
                positions.Add(slot.Position);
        }

        return positions;
    }
    public InventoryItem GetItemFromPosition(Vector2Int position)
    {
        return inventoryItems.FirstOrDefault(n => n.positions.Contains(position));
    }
    public InventoryItem GetItemFromPositions(List<Vector2Int> positions)
    {
        return positions.Select(position => inventoryItems.FirstOrDefault(n => n.positions.Contains(position)))
                        .FirstOrDefault(n => n != null);
    }

    public void OnAwake()
    {
        characterItems.Initialize();
    }

    private void OnDisable()
    {
        InteractInventory = default;
        InteractEquipment = default;
        UpdateSlotID = default;
        ReleaseSlotID = default;
        cursorItem = default;
        PointerOnUI = null;

        characterItems.Release();
    }
}
