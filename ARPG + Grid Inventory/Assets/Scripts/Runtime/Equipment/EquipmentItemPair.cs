
using System;

[Serializable]
public class EquipmentItemPair : EquipmentItem
{
    public Item offItem;

    public override bool IsSlotOccupied(bool isMain = true)
    {
        return isMain ? mainItem != null : offItem != null;
    }
}
