using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIInventorySlotOverlay : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private UIInventorySlot slot = default;
    [SerializeField] private Image image = default;

    //public InventorySlotData colorData;
    
    [Header("Overlay Colors")]
    [SerializeField] private Color availableSlot = default;
    [SerializeField] private Color unavailableSlot = default;
    [SerializeField] private Color occupiedSlot = default;
    [SerializeField] private Color unoccupiedSlot = default;
    [SerializeField] private Color swapSlot = default;

    private Color _displayColor;
    
    private bool _available;

    private void Awake()
    {
        if(slot == null)
            slot = GetComponentInParent<UIInventorySlot>();

        _displayColor = unoccupiedSlot;
    }

    private void Start()
    {
        slot.OnStateChange += UpdateState;
    }

    private void UpdateState(UIInventorySlot.SlotState state)
    {
        switch (state)
        {
            case UIInventorySlot.SlotState.Empty: // null
                _displayColor = unoccupiedSlot;
                break;
            case UIInventorySlot.SlotState.Occupied: // azul
                _displayColor = occupiedSlot;
                break;
            case UIInventorySlot.SlotState.TakeHovered: //verde 
                _displayColor = availableSlot;
                break;
            case UIInventorySlot.SlotState.SwapHovered: //amarillo
                _displayColor = swapSlot;
                break;
            case UIInventorySlot.SlotState.Available: //Verde
                _displayColor = availableSlot;
                break;
            case UIInventorySlot.SlotState.Unavailable:  //Rojo
                _displayColor = unavailableSlot;
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(state), state, null);
        }
    }

    private void LateUpdate()
    {
        image.color = _displayColor;
    }
}
