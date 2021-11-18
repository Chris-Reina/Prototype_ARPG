using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DoaT;
using UnityEngine;
using UnityEngine.EventSystems;

//Interface to work with the inventory data
[Serializable]
public class Inventory : MonoBehaviour
{
    public InventoryData data;
    public InventoryActionData inventoryActionData;
    public InventoryGrid grid;
    
    //private List<Item> _unequipped = new List<Item>();

    public Tuple<float, float> WeaponDamage => new Tuple<float, float>(weaponDamage.x, weaponDamage.y);

    [SerializeField] private Vector2 weaponDamage = default;

    private void Awake()
    {
        data.InteractInventory += InteractInventory;
        data.InteractEquipment += InteractEquipment;

        grid = FindObjectOfType<InventoryGrid>();
    }

    private void Start()
    {
        EventManager.Subscribe(EventsData.OnItemPickUp, PickUpItem);
    }

    private void InteractInventory()
    {
        if (inventoryActionData.allowsAdd)
        {
            if (!inventoryActionData.isItemInCursor) return;
                
            AddItem(data.cursorItem.ToInventoryItem(inventoryActionData.slots.Select(n => n.Position).ToList()));
            data.cursorItem = default;
        }
        else if (inventoryActionData.allowsSwap)
        {
            var temp = inventoryActionData.slots.Select(x => x.Position).ToList();
            var invItem = data.cursorItem.ToInventoryItem(temp);

            data.cursorItem =
                SwapItem(invItem, inventoryActionData.slots.Select(n => n.Position).ToList()).ToItem(); //NULL
        }
        else if (inventoryActionData.allowsRemove)
        {
            var asd = RemoveItem(inventoryActionData.targetSlot.Position).ToItem();
            data.cursorItem = asd;
        }
    }
    private void InteractEquipment(EquipmentSlotData slotData)
    {
        if (data.cursorItem == null)
        {
            data.cursorItem = data.characterItems.GetItem(slotData.EquipmentItemSlot, slotData.IsMain);
        }
        else
        {
            if (data.cursorItem.slot != slotData.EquipmentItemSlot) return;

            if (data.characterItems.PeekItem(slotData.EquipmentItemSlot, out var item, slotData.IsMain))
            {
                data.characterItems.SetItem(data.cursorItem, slotData.IsMain);
                data.cursorItem = item;
            }
            else
            {
                data.characterItems.SetItem(data.cursorItem, slotData.IsMain);
                data.cursorItem = null;
            }
        }
    }

    private void AddItem(InventoryItem item)
    {
        data.inventoryItems.Add(item);
        data.UpdateSlotID();
    }

    private InventoryItem RemoveItem(Vector2Int position)
    {
        var item = data.GetItemFromPosition(position);
        foreach (var iItem in data.inventoryItems.Where(iItem => iItem == item))
        {
            foreach (var pos in iItem.positions)
            {
                data.ReleaseSlotID(pos);
            }
        }
        data.inventoryItems.Remove(item);
        
        return item;
    }

    private InventoryItem SwapItem(InventoryItem item, List<Vector2Int> positionToExtract)
    {
        var iItem = data.GetItemFromPositions(positionToExtract);
        
        foreach (var iItem2 in data.inventoryItems.Where(iItem2 => iItem2 == iItem))
        {
            foreach (var pos in iItem2.positions)
            {
                data.ReleaseSlotID(pos);
            }
        }
        data.inventoryItems.Remove(iItem);
        
        data.inventoryItems.Add(item);
        data.UpdateSlotID();

        return iItem;
    }

    private void PickUpItem(params object[] parameters)
    {
        var item = (Item) parameters[0];
        var action = (Action) parameters[1];
        
        if (data.cursorItem == null)
        {
            data.cursorItem = item;
            action?.Invoke();
        }
    }
}
