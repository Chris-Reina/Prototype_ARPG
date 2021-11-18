using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIPointer : MonoBehaviour
{
    public CanvasGroup cGroup;
    public CursorGameSelection selection;
    public Image cursorImage;
    public Image cursorMaskImage;

    private Vector3 mPos;

    [SerializeField] private Sprite CursorIdle = default;
    [SerializeField] private Sprite CursorPressed = default;

    public GameObject inventoryRectT;
    
    private void Awake()
    {
        Cursor.visible = false;
        
        if(!cGroup)
            cGroup = GetComponentInParent<CanvasGroup>();
        
        
    }
    void Update () 
    {
        cursorMaskImage.gameObject.SetActive(selection.raycastResult is EntityRaycastResult);

        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            cursorImage.sprite = CursorPressed;
            cursorMaskImage.sprite = CursorPressed;
        }
        else if (Input.GetKeyUp(KeyCode.Mouse0))
        {
            cursorImage.sprite = CursorIdle;
            cursorMaskImage.sprite = CursorIdle;
        }

        mPos = Input.mousePosition;
    }

    private void LateUpdate()
    {
        transform.position = mPos;
    }
}
