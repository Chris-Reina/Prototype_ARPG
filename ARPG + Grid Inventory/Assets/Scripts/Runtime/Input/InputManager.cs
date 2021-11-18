using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public enum GetKeyType
{
    GetKey,
    GetKeyDown,
    GetKeyUp,
    GetKeyAndDown,
    GetKeyDownOrUp
}

public class InputManager : MonoBehaviour
{
    public static InputManager Instance { get; private set; }

    [SerializeField] private InputKeys _inputKeys = default;

    public static InputKeys InputKeys => GetInputs();

    private void Awake()
    {
        if (Instance == null || Instance == this)
            Instance = this;
        else
            Destroy(this);
    }
    
    

    public static bool CursorMain(GetKeyType pressType)
    {
        return GetKeyPressing(KeyCode.Mouse0, pressType);
    }
    
    public static bool OpenInventoryKey(GetKeyType pressType)
    {
        return GetKeyPressing(Instance._inputKeys.openCloseInventory, pressType);
    }
    
    public static bool ForcePositionKey(GetKeyType pressType)
    {
        return GetKeyPressing(Instance._inputKeys.forceAction, pressType);
    }

    public static int GetAbilityKey(out int mouseIndex, GetKeyType pressingType)
    {
        var keys = Instance._inputKeys.Abilities.ToList();
        mouseIndex = 0;
        
        for (var i = 0; i < keys.Count; i++)
        {
            if (GetKeyPressing(keys[i], pressingType))
            {
                return i;
            }
        }

        return -1;
    }

    public static bool GetKeyPressing(KeyCode key, GetKeyType pressType)
    {
        switch (pressType)
        {
            case GetKeyType.GetKey:
                return Input.GetKey(key);
            case GetKeyType.GetKeyDown:
                return Input.GetKeyDown(key);
            case GetKeyType.GetKeyUp:
                return Input.GetKeyUp(key);
            case GetKeyType.GetKeyAndDown:
                return Input.GetKeyDown(key) || Input.GetKey(key);
            case GetKeyType.GetKeyDownOrUp:
                return Input.GetKeyDown(key) || Input.GetKeyUp(key);
            default:
                throw new ArgumentOutOfRangeException(nameof(pressType), pressType, null);
        }
    }

    private static InputKeys GetInputs()
    {
        return Instance._inputKeys;
    }
}
