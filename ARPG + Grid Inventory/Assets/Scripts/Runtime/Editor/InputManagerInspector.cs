using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(InputManager))]
public class InputManagerInspector : Editor
{
    private InputManager _target;

    private void OnEnable()
    {
        _target = (InputManager)target;
    }

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        
    }
}
