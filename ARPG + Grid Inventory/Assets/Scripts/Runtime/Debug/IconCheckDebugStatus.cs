using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IconCheckDebugStatus : CheckDebugStatus
{
    public Image icon;

    protected override void Awake()
    {
        base.Awake();
        
        icon.color = manager.ShowDebugElements ? manager.activeDebugColor : manager.inactiveDebugColor;
    }

    protected override void Toggle(bool status)
    {
        icon.color = status ? manager.activeDebugColor : manager.inactiveDebugColor;
    }
}
