using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIPointerItemTarget : MonoBehaviour
{
    public InventoryData data;
    public CanvasGroup cGroup;
    public Image img;

    private Vector3 mPos;
    [SerializeField] private int initialSize = default;
    
    private void Awake()
    {
        if(!cGroup)
            cGroup = GetComponentInParent<CanvasGroup>();
    }

    private void Update () 
    {
        if (data.cursorItem == null)
        {
            if (cGroup.alpha == 1)
                cGroup.alpha = 0;
        }
        else
        {
            img.rectTransform.sizeDelta = (data.cursorItem.inventorySize - new Vector2Int(1, 1)) * initialSize;
            img.sprite = data.cursorItem.sprite;
            
            if (cGroup.alpha == 0)
                cGroup.alpha = 1;
        }
        
        mPos = Input.mousePosition;
    }

    private void LateUpdate()
    {
        transform.position = mPos;
    }
}
