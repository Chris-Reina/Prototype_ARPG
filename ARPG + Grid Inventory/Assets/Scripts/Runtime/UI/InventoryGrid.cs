using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DoaT;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventoryGrid : MonoBehaviour
{
    [Space(5)] 
    public GridLayoutGroup layoutGroup;

    [Header("Slots")] 
    public GameObject slotPrefab;
    public Vector2 slotPixelSize;
    public Vector2 slotPixelSpacing;

    [Header("Grid")] 
    public Vector2Int gridSize;

    [Header("Inventory")] 
    public InventoryData inventoryData;
    public InventoryActionData inventoryAction;

    public UIInventorySlot[,] gridSlots;
    public List<UIInventorySlot> currentSlots = new List<UIInventorySlot>();

    private void Awake()
    {
        GenerateInventoryGrid();

        inventoryData.ReleaseSlotID += CleanSlotItemID;
        inventoryData.UpdateSlotID += UpdateSlotItemID;
    }

    private void Start()
    {
        EventManager.Subscribe(EventsData.OnInventoryDraw, UpdateUI);
        EventManager.Subscribe(EventsData.OnItemPickUp, UpdateUI);
    }

    private void UpdateUI(params object[] parameters) //Should Redraw the Inventory Grid Color Disposition
    {
        UpdateSlotItemID();
        
        if (inventoryData.inventoryItems.Count == 0) //Si el inventario esta vacio
        {
            UpdateInventoryWhenEmpty();
        }
        else
        {
            UpdateInventoryWhenOccupied();
        }
    }

    private void UpdateSlotItemID()
    {
        foreach (var item in inventoryData.inventoryItems)
        {
            foreach (var position in item.positions)
            {
                gridSlots[position.x, position.y].ItemID = inventoryData.inventoryItems.FindIndex(n => n == item);
            }
        }
    }

    private void CleanSlotItemID(Vector2Int position)
    {
        gridSlots[position.x, position.y].ItemID = -1;
    }
    private void UpdateInventoryWhenEmpty()
    {
        if (inventoryAction.isItemInCursor)
        {
            if (inventoryData.PointerOnUI())
            {
                var targetedPositions = inventoryAction.slots.Select(x => x.Position).ToList();

                foreach (var slot in currentSlots)
                {
                    slot.State = targetedPositions.Contains(slot.Position)
                        ? UIInventorySlot.SlotState.Available
                        : UIInventorySlot.SlotState.Empty;

                } 
                
                SetInventoryActions(true, false, false, false);
            }
            else
            {
                foreach (var slot in currentSlots)
                {
                    slot.State = UIInventorySlot.SlotState.Empty;
                }
                
                SetInventoryActions(true, false, false, false);
            }
        }
        else
        {
            foreach (var slot in currentSlots)
            {
                slot.State = UIInventorySlot.SlotState.Empty;
            }
            
            SetInventoryActions(true, false, false, false);
        }
    }
    private void UpdateInventoryWhenOccupied()
    {
        List<Vector2Int> targetedPositions; 
        var occupiedPositions = inventoryData.inventoryItems.SelectMany(x => x.positions).ToList(); //IA2-P3 SelectMany
        var state = UIInventorySlot.SlotState.Empty;


        if (inventoryAction.targetSlot == null)
        { 
            targetedPositions = new List<Vector2Int>();
        }
        else
        {
            targetedPositions = inventoryAction.slots.Select(x => x.Position).ToList();
                
            var targetedSlots = targetedPositions.Select(position => gridSlots[position.x, position.y]).ToList();
            
            var countId = new List<int>();
            foreach (var slot in targetedSlots.Where(slot => !countId.Contains(slot.ItemID) && slot.ItemID != -1))
            {
                countId.Add(slot.ItemID);
            }

            switch (countId.Count)
            {
                case 0:
                    state = inventoryAction.isItemInCursor ? UIInventorySlot.SlotState.Available : UIInventorySlot.SlotState.Empty;
                    SetInventoryActions(true,true,false,false);
                    break;
                case 1:
                    if (inventoryAction.isItemInCursor)
                    {
                        state = UIInventorySlot.SlotState.SwapHovered;
                        SetInventoryActions(false, false, false, true);
                    }
                    else
                    {
                        state = UIInventorySlot.SlotState.TakeHovered;
                        SetInventoryActions(false, false, true, false);
                    }
                    break;
                default:
                    state = UIInventorySlot.SlotState.Unavailable;
                    SetInventoryActions(false,false,false,false);
                    break;
            }
        }

        foreach (var slot in currentSlots)
        {
            if (targetedPositions.Contains(slot.Position))
            {
                slot.State = state;
            }
            else if (occupiedPositions.Contains(slot.Position))
            {
                slot.State = UIInventorySlot.SlotState.Occupied;
            }
            else
            {
                slot.State = UIInventorySlot.SlotState.Empty;
            }
        }     
    }

    private void SetInventoryActions(bool add, bool cancel, bool remove, bool swap)
    {
        inventoryAction.allowsAdd = add;
        inventoryAction.allowsCancel = cancel;
        inventoryAction.allowsRemove = remove;
        inventoryAction.allowsSwap = swap;        
    }

    private void GenerateInventoryGrid()
    {
        SetupLayoutGroup();
        GenerateGridNodes();
    }
    private void SetupLayoutGroup()
    {
        layoutGroup.cellSize = slotPixelSize;
        layoutGroup.spacing = slotPixelSpacing;
    }
    private void GenerateGridNodes()
    {
        currentSlots = new List<UIInventorySlot>();
        gridSlots = new UIInventorySlot[gridSize.x, gridSize.y];

        for (int i = 0; i < gridSlots.GetLength(1); i++)
        {
            for (int j = 0; j < gridSlots.GetLength(0); j++)
            {
                var temp = Instantiate(slotPrefab, transform, true).GetComponentInChildren<UIInventorySlot>();
                temp.Initialize(j, i, this);
                currentSlots.Add(temp);
                gridSlots[j, i] = temp;
            }
        }

        AssignNeighbours();
    }
    private void AssignNeighbours()
    {
        var xLength = gridSlots.GetLength(0);
        var yLength = gridSlots.GetLength(1);

        foreach (var slot in currentSlots)
        {
            var x = slot.Position.x;
            var y = slot.Position.y;

            if (x - 1 >= 0)
            {
                slot.SetNeighbour(CardinalDirection.Left, gridSlots[x - 1, y]);
            }

            if (x + 1 < xLength)
            {
                slot.SetNeighbour(CardinalDirection.Right, gridSlots[x + 1, y]);
            }

            if (y - 1 >= 0)
            {
                slot.SetNeighbour(CardinalDirection.Down, gridSlots[x, y - 1]);
            }

            if (y + 1 < yLength)
            {
                slot.SetNeighbour(CardinalDirection.Up, gridSlots[x, y + 1]);
            }
        }
    }
    
    
    
    private Vector2Int FindOpenGridSlot(Item item)
    {
        for (int i = 0; i < gridSize.x; i++)
        {
            for (int j = gridSize.y - 1; j >= 0; j--)
            {
                if (CheckSpaceFit(j, i, item))
                {
                    return new Vector2Int(j, i);
                }
            }
        }

        return new Vector2Int(-1, -1);
    }

    private bool CheckSpaceFit(int x, int y, Item item)
    {
        for (int i = 0; i < item.inventorySize.x; i++)
        {
            for (int j = item.inventorySize.y - 1; j >= 0; j--)
            {
                if (y + j >= gridSlots.GetLength(1) || x + i >= gridSlots.GetLength(0))
                    return false;

                if(gridSlots[x + i, y + j].ItemID != -1)
                {
                    return false;
                }
            }
        }

        return true;
    }
}






//private bool hasInitialized = false;

    // public void Initialize()
    // {
    //     if (hasInitialized) return;
    //
    //     gridNodes = new InventoryGridNode[GridSize.y, GridSize.x];
    //
    //     var nodes = GetComponentsInChildren<InventoryGridNode>();
    //
    //     Debug.Log("y" + gridNodes.GetLength(0));
    //     Debug.Log("x" + gridNodes.GetLength(1));
    //
    //     foreach (var node in nodes)
    //     {
    //
    //         gridNodes[node.GridPosition.x, node.GridPosition.y] = node;
    //     }
    //
    //     hasInitialized = true;
    // }
    //
    // public void CleanGrid()
    // {
    //     foreach (var node in gridNodes)
    //     {
    //         node.TryToLiberateNode(false);
    //     }
    // }

    /*public void FitItemIntoGrid(InventoryItemData item)
    {
        Initialize();

        Vector2Int root = FindOpenGridSlot(item);

        if (root == new Vector2(-1, -1))
            return;

        OccupyNode(root.x, root.y, item);
        item.positionInInventory = new Vector2(root.x, root.y);
    }

    private void OccupyNode(int x, int y, InventoryItemData item)
    {
        Color randomC = Random.ColorHSV();

        gridNodes[x, y].IsOccupied = true;
        gridNodes[x, y].IsParentNode = true;
        gridNodes[x, y].itemStored = item;

        for (int i = 0; i < item.inventorySpaceX; i++)
        {
            for (int j = 0; j < item.inventorySpaceY; j++)
            {
                if (i == 0 && j == 0)
                {
                    gridNodes[x, y].PaintedColor = randomC;
                    continue;
                }
                else
                {
                    gridNodes[x + j, y + i].IsOccupied = true;
                    gridNodes[x + j, y + i].PaintedColor = randomC;
                    gridNodes[x + j, y + i].RootItemNode = gridNodes[x, y];
                    gridNodes[x, y].childNodes.Add(gridNodes[x + j, y + i]);
                }
            }
        }
    }*/



    /*private Vector2Int FindOpenGridSlot(InventoryItemData item)
    {
        for (int i = 0; i < GridSize.x; i++)
        {
            for (int j = 0; j < GridSize.y; j++)
            {
                if (CheckSpaceFit(j, i, item))
                {
                    return new Vector2Int(j, i);
                }
            }
        }

        return new Vector2Int(-1, -1);
    }

    private bool CheckSpaceFit(int x, int y, InventoryItemData item)
    {
        for (int i = 0; i < item.inventorySpaceX; i++)
        {
            for (int j = 0; j < item.inventorySpaceY; j++)
            {
                if (x + j >= gridNodes.GetLength(0) || y + i >= gridNodes.GetLength(1))
                    return false;

                if(gridNodes[x + j, y + i].IsOccupied)
                {
                    return false;
                }
            }
        }

        return true;
    }
    */


