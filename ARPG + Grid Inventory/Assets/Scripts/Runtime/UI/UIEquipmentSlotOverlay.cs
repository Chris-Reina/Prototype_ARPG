using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIEquipmentSlotOverlay : MonoBehaviour
{
    [SerializeField] private Color baseColor = default; 
    [SerializeField] private Color canInteractColor = default; 
    [SerializeField] private Color cannotInteractColor = default;
    [SerializeField]private Image overlayImage = default; 
    
    private bool _overlayEnabled = true;
    private UIEquipmentSlot _slot;

    private void Awake()
    {
        if (!overlayImage)
            overlayImage = GetComponent<Image>();
            
        if (!_slot)
            _slot = GetComponent<UIEquipmentSlot>();

        if (!_slot || !overlayImage)
            _overlayEnabled = false;
    }

    private void Start()
    {
        if (_overlayEnabled)
            _slot.OnPointerInteract += OnPointerInteract;
    }

    private void OnPointerInteract(bool hover, bool canInteract)
    {
        var hoveredColor = canInteract ? canInteractColor : cannotInteractColor;
        overlayImage.color = hover ? hoveredColor : baseColor;

    }
}
