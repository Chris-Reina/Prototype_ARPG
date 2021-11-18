using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DebugManager : MonoBehaviour
{
    private bool _showDebugElements;
    public bool ShowDebugElements => _showDebugElements;
    public Color activeDebugColor = Color.green;
    public Color inactiveDebugColor = Color.grey;

    public event Action<bool> OnDebugStatusChange;
    
    public void ToggleDebugElements()
    {
        _showDebugElements = !_showDebugElements;
        OnDebugStatusChange?.Invoke(_showDebugElements);
    }
}
