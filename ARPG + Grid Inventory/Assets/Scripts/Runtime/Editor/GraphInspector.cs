using System.Collections;
using System.Collections.Generic;
using DoaT.AI;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Graph))]
public class GraphInspector : Editor
{
    private Graph _target;

    private void OnEnable()
    {
        _target = (Graph) target;
    }

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        
        if(GUILayout.Button("Recalculate Nodes"))
        {
            _target.RecalculateNodeList();
        }

        if (GUILayout.Button("CalculateBlockStatus"))
        {
            _target.CalculateNodeBlockState();
        }
        
        if(GUILayout.Button("Recalculate Neighbours"))
        {
            _target.CalculateNodeNeighbours();
        }
    }
}