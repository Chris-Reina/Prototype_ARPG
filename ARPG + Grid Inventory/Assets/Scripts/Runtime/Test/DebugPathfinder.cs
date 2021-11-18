using System;
using System.Collections;
using System.Collections.Generic;
using DoaT.AI;
using UnityEditor;
using UnityEngine;

public class DebugPathfinder : MonoBehaviour
{
    public GameObject Init;
    public GameObject Finit;

    public Pathfinder finder;
    
    public Path path;

    public float radius = 0.2f;


    private void Awake()
    {
        finder = FindObjectOfType<Pathfinder>();
    }

    public Path CalculatePath()
    {
        return finder.GetPathStruct(Init.transform.position, Finit.transform.position);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;

        if (Init == null || Finit == null) return;

        Gizmos.DrawSphere(Init.transform.position, radius);
        Gizmos.DrawSphere(Finit.transform.position, radius);

        if (path == null) return;
        if (path == default) return;

        Gizmos.color = Color.black;
        
        var list = new List<Vector3>(path.nodes);
        list.Insert(0, Init.transform.position);
        list.Add(Finit.transform.position);

        for (int i = 0; i < list.Count-1; i++)
        {
            Gizmos.DrawLine(list[i], list[i + 1]);
        }
    }
}

/*
[CustomEditor(typeof(DebugPathfinder))]
public class DebugPathfinderInspector : Editor
{
    private DebugPathfinder _pf;

    private void OnEnable()
    {
        _pf = (DebugPathfinder)target;
    }

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        if (GUILayout.Button("calculate path"))
        {
            _pf.path = _pf.CalculatePath();
        }
    }
}*/

