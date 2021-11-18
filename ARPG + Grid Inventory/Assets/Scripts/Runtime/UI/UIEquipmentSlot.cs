using System;
using System.Collections;
using System.Collections.Generic;
using DoaT;
using UnityEngine;
using UnityEngine.EventSystems;

public class UIEquipmentSlot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, ITargetableUI
{
    public enum SlotType
    {
        Simple,
        Double
    }
    
    [SerializeField] private EquipmentItemSlot itemSlot = default;
    [SerializeField] private InventoryData inventoryData = default;
    [SerializeField] private bool _isMain = default;
    [SerializeField] private SlotType _slotType = SlotType.Simple;

    public EquipmentItemSlot ItemSlot => itemSlot;
    public SlotType Slot => _slotType;

    public Sprite Sprite => inventoryData.characterItems.GetItem(itemSlot)?.sprite;
    public InventoryData Data => inventoryData;
    public bool IsMain => _isMain;
    
    /// <summary>
    /// bool hover, bool canInteract
    /// </summary>
    public event Action<bool,bool> OnPointerInteract = default;

    #region Events
    public void OnPointerEnter(PointerEventData eventData)
    {
        EventManager.Trigger(EventsData.OnSlotPointerEnter,  this);

        var canInteract = inventoryData.cursorItem == null || inventoryData.cursorItem.slot == itemSlot;

        OnPointerInteract?.Invoke(true, canInteract);
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        EventManager.Trigger(EventsData.OnSlotPointerExit, this);
        OnPointerInteract?.Invoke(false, true);
    }

    public void OnClick()
    {
        EventManager.Trigger(EventsData.OnInteractionWithUI, this);
        var canInteract = inventoryData.cursorItem == null || inventoryData.cursorItem.slot == itemSlot;
        OnPointerInteract?.Invoke(true, canInteract);
    }

    public void OnTarget()
    {
        
    }
    #endregion
}
