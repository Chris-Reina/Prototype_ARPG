using System.Collections;
using System.Collections.Generic;
using DoaT.AI;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(GraphGenerator))]
public class GraphGeneratorInspector : Editor
{
    private GraphGenerator _target;

    private void OnEnable()
    {
        _target = (GraphGenerator) target;
    }

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        if (GUILayout.Button("Generate"))
        {
            _target.Generate();
        }
    }
}
