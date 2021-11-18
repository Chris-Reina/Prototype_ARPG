using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(DebugManager))]
public class DebugManagerInspector : Editor
{
    private DebugManager _t;

    private void OnEnable()
    {
        _t = (DebugManager)target;
    }

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        
        var status = _t.ShowDebugElements ? _t.activeDebugColor : _t.inactiveDebugColor;
        
        GUILayout.Space(10);
        EditorGUI.DrawRect(GUILayoutUtility.GetRect(1, 10), status);
        
        if(GUILayout.Button("Toggle Debug Mode"))
        {
            _t.ToggleDebugElements();
        }
    }
}
