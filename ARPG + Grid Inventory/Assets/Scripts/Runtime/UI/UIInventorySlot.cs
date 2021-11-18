using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DoaT;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

//Knows about its state and whether it has any neighbours and who they are.
public class UIInventorySlot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, ITargetableUI
{
    public enum SlotState
    {
        Empty,
        Occupied,
        TakeHovered,
        SwapHovered,
        Available,
        Unavailable
    }
    
    [Serializable]
    public class SlotNeighbours
    {
        public UIInventorySlot upSlot;
        public UIInventorySlot downSlot;
        public UIInventorySlot leftSlot;
        public UIInventorySlot rightSlot;
    }

    public event Action<SlotState> OnStateChange;

    [SerializeField] private TextMeshProUGUI slotDebugName;
    [SerializeField] private SlotState _state;
    [SerializeField] private InventoryGrid _grid;
    [SerializeField] private Vector2Int _position;
    [SerializeField] private SlotNeighbours _neighbours = new SlotNeighbours();

    public int ItemID;
    
    public GameObject Parent => transform.parent.gameObject;
    public Vector2Int Position => _position;
    public InventoryGrid Grid => _grid;

    public SlotState State
    {
        set
        {
            _state = value;
            OnStateChange?.Invoke(value);
        }
    }

    public void Initialize(int x, int y, InventoryGrid grid)
    {
        var parent = transform.parent;
        if (slotDebugName == null) slotDebugName = parent.GetComponentInChildren<TextMeshProUGUI>();
        slotDebugName.text = $"{x},{y}";
        parent.gameObject.name = $"Slot [{x},{y}]";
        _position = new Vector2Int(x, y);
        _grid = grid;
        ItemID = -1;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        EventManager.Trigger(EventsData.OnSlotPointerEnter,  this);
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        EventManager.Trigger(EventsData.OnSlotPointerExit, this);
    }
    
    public UIInventorySlot GetNeighbour(CardinalDirection direction)
    {
        switch (direction)
        {
            case CardinalDirection.Left:
                return _neighbours.leftSlot;
            case CardinalDirection.Right:
                return _neighbours.rightSlot;
            case CardinalDirection.Up:
                return _neighbours.upSlot;
            case CardinalDirection.Down:
                return _neighbours.downSlot;
            default:
                throw new ArgumentOutOfRangeException(nameof(direction), direction, null);
        }
    }
    public void SetNeighbour(CardinalDirection direction, UIInventorySlot neighbour)
    {
        switch (direction)
        {
            case CardinalDirection.Left:
                _neighbours.leftSlot = neighbour;
                break;
            case CardinalDirection.Right:
                _neighbours.rightSlot = neighbour;
                break;
            case CardinalDirection.Up:
                _neighbours.upSlot = neighbour;
                break;
            case CardinalDirection.Down:
                _neighbours.downSlot = neighbour;
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(direction), direction, null);
        }
    }

    public void OnClick()
    {
        EventManager.Trigger(EventsData.OnInteractionWithUI, this);
    }

    public void OnTarget()
    {
        
    }
}