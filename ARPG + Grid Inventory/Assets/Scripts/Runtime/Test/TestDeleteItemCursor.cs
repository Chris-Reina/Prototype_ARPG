using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestDeleteItemCursor : MonoBehaviour
{
    public InventoryData data;

    public void DestroyItem()
    {
        data.cursorItem = null;
    }
}
