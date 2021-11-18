using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public static class InventoryUtility
{
    public static Quadrant GetPointerQuadrant(RectTransform rectTransform)
    {
        var ped = new PointerEventData(EventSystem.current) {position = Input.mousePosition};

        if (!RectTransformUtility.ScreenPointToLocalPointInRectangle(rectTransform, ped.position,
            ped.pressEventCamera, out var localCursor))
            return new Quadrant(CardinalDirection.Right, CardinalDirection.Down);
        
        var horizontalQ = localCursor.x >= 0 ? CardinalDirection.Right : CardinalDirection.Left;
        var verticalQ = localCursor.y > 0 ? CardinalDirection.Up : CardinalDirection.Down;
        
        return new Quadrant(horizontalQ, verticalQ);
    }
    
    public static List<UIInventorySlot> GetInventorySlotList(Vector2Int dimensions, UIInventorySlot centralSlot, InventoryGrid grid)
    {
        var list = new List<UIInventorySlot>();

        var columns = GetColumnNumbers(dimensions, centralSlot);
        var rows = GetRowNumbers(dimensions, centralSlot);
        
        for (int i = 0; i < columns.Count; i++)
        {
            for (int j = 0; j < rows.Count; j++)
            {
                //Debug.Log($"grid slot [{columns[i]}, {rows[j]}]");
                list.Add(grid.gridSlots[columns[i], rows[j]]);
            }
        }
        
        return list;
    }
    private static List<int> GetColumnNumbers(Vector2Int dimensions,UIInventorySlot target)
    {
        var list = new List<int> {target.Position.x};

        list = dimensions.x % 2 == 0
            ? GetDispositionEven(list, CardinalDirection.Right, CardinalDirection.Left, dimensions.x, true, target)
            : GetDispositionOdd(list, CardinalDirection.Right, CardinalDirection.Left, dimensions.x, true, target);

        return list;
    }
    private static List<int> GetRowNumbers(Vector2Int dimensions,UIInventorySlot target)
    {
        var list = new List<int> {target.Position.y};

        list = dimensions.x % 2 == 0
            ? GetDispositionEven(list, CardinalDirection.Down, CardinalDirection.Up, dimensions.y, false, target)
            : GetDispositionOdd(list, CardinalDirection.Down, CardinalDirection.Up, dimensions.y, false, target);

        return list;
    }
    
    
    private static List<int> GetDispositionEven(IEnumerable<int> list, CardinalDirection first,CardinalDirection second,int dimension,bool isAxisX,UIInventorySlot target)
    {
        var result = new List<int>(list);
        var quadrant = GetPointerQuadrant(target.transform as RectTransform);
        var checkMain = isAxisX ? quadrant.Horizontal == first : quadrant.Vertical == first;
        uint iterator = 0;

        CardinalDirection mainDirection;
        CardinalDirection offDirection;

        if (checkMain)
        {
            mainDirection = first;
            offDirection = second;
        }
        else
        {
            mainDirection = second;
            offDirection = first;
        }

        while (result.Count < dimension)
        {
            UIInventorySlot slot;
            if (checkMain)
            {
                if (CheckDirection(mainDirection, iterator, target, out slot))
                    result.Add(isAxisX ? slot.Position.x : slot.Position.y);
            }
            else
            {
                if (CheckDirection(offDirection, iterator, target, out slot))
                    result.Add(isAxisX ? slot.Position.x : slot.Position.y);

                iterator += 1;
            }
                
            checkMain = !checkMain;
        }

        return result;
    }
    private static List<int> GetDispositionOdd(IEnumerable<int> list, CardinalDirection first,CardinalDirection second,int dimension,bool isAxisX,UIInventorySlot target)
    {
        var result = new List<int>(list);
        uint iterator = 0;
        var checkRight = true;

        var mainDirection = first;
        var offDirection = second;
        

        while (result.Count < dimension)
        {
            UIInventorySlot slot;
            if (checkRight)
            {
                if (CheckDirection(first, iterator, target, out slot))
                {
                    result.Add(slot.Position.x);
                }
            }
            else
            {
                if (CheckDirection(second, iterator, target, out slot))
                {
                    result.Add(slot.Position.x);
                }
                    
                iterator += 1;
            }
            checkRight = !checkRight;
        }

        return result;
    }
    private static bool CheckDirection(CardinalDirection direction, uint depth, UIInventorySlot slot, out UIInventorySlot target)
    {
        while (true)
        {
            var neighbourInDirection = slot.GetNeighbour(direction);
            
            switch (depth)
            {
                case 0:
                    target = default;
                    return false;
                case 1:
                    target = neighbourInDirection;
                    return target != null;
                default:
                    if(neighbourInDirection)
                        slot = neighbourInDirection;
                    else
                    {
                        target = default;
                        return false;
                    }
                    depth -= 1;
                    break;
            }
        }
    }
}
