using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(PlayerController))]
public class PlayerControllerInspector : Editor
{
    private PlayerController _t;

    private void OnEnable()
    {
        _t = (PlayerController) target;
    }

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        
        GUILayout.Space(10);

        GUI.enabled = false;
        EditorGUILayout.TextField("Current State", _t.currentState);
        GUI.enabled = true;
    }
}
