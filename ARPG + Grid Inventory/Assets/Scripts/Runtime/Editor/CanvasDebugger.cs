using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class CanvasDebugger : MonoBehaviour
{
    public GameObject DebugPannel;

    public void ToggleDebug()
    {
        if(DebugPannel.activeSelf)
            DebugPannel.SetActive(false);
        else
            DebugPannel.SetActive(true);
    }
}

[CustomEditor(typeof(CanvasDebugger))]
public class CanvasDebuggerInspector : Editor
{
    private CanvasDebugger _target;

    private void OnEnable()
    {
        _target = (CanvasDebugger)target;
    }

    public override void OnInspectorGUI()
    {
        if(GUILayout.Button("Debug"))
        {
            _target.ToggleDebug();
        }
    }
}
