using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(InventoryGrid))]
public class InventoryGridInspector : Editor
{
    private InventoryGrid _ig;
    
    private void OnEnable()
    {
        _ig = (InventoryGrid) target;
    }

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        // GUILayout.Space(10);
        //
        // if (GUILayout.Button("GenerateNodes"))
        // {
        //     _ig.GenerateInventoryGrid();
        // }
    }
}
