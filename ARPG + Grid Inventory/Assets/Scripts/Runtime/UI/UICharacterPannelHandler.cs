using System;
using System.Collections;
using System.Collections.Generic;
using DoaT;
using UnityEngine;

public class UICharacterPannelHandler : MonoBehaviour
{
    public CanvasGroup characterGroup;
    private bool isActive = false;

    private void Awake()
    {
        if (characterGroup == null)
            characterGroup = GetComponent<CanvasGroup>();
        
        isActive = characterGroup.alpha == 1;
        
        Deactivate();
    }

    private void Start()
    {
        EventManager.Subscribe(EventsData.OnItemPickUp,ActivateInventory);
    }
    
    private void ActivateInventory(params object[] parameters)
    {
        
        Activate();
    }

    private void Update()
    {
        if (InputManager.GetKeyPressing(InputManager.InputKeys.openCloseInventory, GetKeyType.GetKeyUp))
        {
            if(isActive)
                Deactivate();
            else
                Activate();
        }
        else if (InputManager.GetKeyPressing(InputManager.InputKeys.closeAllPanels, GetKeyType.GetKeyUp))
        {
            Deactivate();
        }
    }

    private void Activate()
    {
        isActive = true;
        characterGroup.interactable = true;
        characterGroup.blocksRaycasts = true;
        characterGroup.alpha = 1;
    }

    private void Deactivate()
    {
        isActive = false;
        characterGroup.interactable = false;
        characterGroup.blocksRaycasts = false;
        characterGroup.alpha = 0;
    }
}
