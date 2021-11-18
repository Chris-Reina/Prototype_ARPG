using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIEquipmentSlotSprite : MonoBehaviour
{
    [SerializeField] private UIEquipmentSlot _slot = default;
    [SerializeField] private Sprite _baseSprite = default;
    [SerializeField] private Image _spriteImage = default;

    private Color _baseColor = Color.white;
    
    private void Awake()
    {
        if(_slot == null)
            _slot = GetComponent<UIEquipmentSlot>();
        
        if(_spriteImage == null)
            _spriteImage = GetComponent<Image>();
    }

    private void Start()
    { 
        _slot.OnPointerInteract += OnPointerInteract;

        DrawItemSprite();
    }

    private void OnPointerInteract(bool hover, bool canInteract)
    {
        DrawItemSprite();
    }

    private void DrawItemSprite()
    {
        var isSpriteBase = !_slot.Data.characterItems.PeekItem(_slot.ItemSlot, out var item, _slot.IsMain);

        _baseColor.a = isSpriteBase ? 0 : 1;
        _spriteImage.color = _baseColor;
        _spriteImage.sprite = isSpriteBase ? _baseSprite : item.sprite;
    }
}
