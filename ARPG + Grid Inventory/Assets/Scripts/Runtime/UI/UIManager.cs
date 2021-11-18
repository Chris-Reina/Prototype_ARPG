using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UIManager : MonoBehaviour
{
    public GameObject CharacterInventory;


    private void Start()
    {
        //EventManager.Subscribe(EventsData.OnItemPickUp,ActivateInventory);
    }

    private void Update()
    {
        if (InputManager.OpenInventoryKey(GetKeyType.GetKeyDown))
        {
            if(!CharacterInventory.activeSelf)
                CharacterInventory.gameObject.SetActive(true);
            else
                CharacterInventory.gameObject.SetActive(false);
        }    
    }

    private void ActivateInventory(params object[] parameters)
    {
        
            CharacterInventory.gameObject.SetActive(true);
    }
}
