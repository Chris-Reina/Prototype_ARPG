using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckDebugStatus : MonoBehaviour
{
    protected DebugManager manager;

    protected virtual void Awake()
    {
        manager = FindObjectOfType<DebugManager>();

        if (!manager)
            gameObject.SetActive(false);
        else
            manager.OnDebugStatusChange += Toggle;

        if (!manager.ShowDebugElements) Toggle(false);
    }

    protected virtual void Toggle(bool status)
    {
        gameObject.SetActive(status);
    }
}
