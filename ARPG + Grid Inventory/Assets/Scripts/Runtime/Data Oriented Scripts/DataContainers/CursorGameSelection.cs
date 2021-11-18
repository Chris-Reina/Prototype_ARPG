using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable, CreateAssetMenu(fileName = "CursorGameSelection", menuName = "Data/CursorGameSelection")]
public class CursorGameSelection : ScriptableObject
{
    [SerializeField] private InventoryData _inventoryData;
    [HideInInspector] public CursorRaycastResult raycastResult;
    public bool extendedMovement;
    public Func<ITargetableUI> GetPointerTarget;

    public Item ItemInCursor
    {
        get => _inventoryData.cursorItem;
        set => _inventoryData.cursorItem = value;
    }

    public bool IsItemInCursor => _inventoryData.cursorItem != null;
    
    private void OnDisable()
    {
        extendedMovement = false;
        raycastResult = default;
        GetPointerTarget = default;
    }
}
