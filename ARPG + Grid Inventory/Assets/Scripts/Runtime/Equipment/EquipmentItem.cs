
using System;

[Serializable]
public class EquipmentItem
{
    public EquipmentItemSlot AdmittedSlot;

    public Item  mainItem;
    
    public virtual Item ToItem()
    {
        return mainItem;
    }

    public virtual bool IsSlotOccupied(bool isMain = true)
    {
        return mainItem != null;
    }
}
