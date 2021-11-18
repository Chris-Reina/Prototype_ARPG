using System;
using System.Collections;
using System.Collections.Generic;
using DoaT.AI;
using UnityEngine;
using UnityEditor;

[ExecuteInEditMode]
public class GraphGenerator : MonoBehaviour
{
    public Vector3 worldSpaceSize;
    public BoundingBox bounds;
    public Node Prefab;
    
    public float nodeSeparation = 0.5f;
    
    private List<Node> gridList;

    [HideInInspector] public bool debugFailsafe = false;
    [HideInInspector] public bool showNodePreview = false;

    private void Awake()
    {
        if (bounds == null)
            bounds = new BoundingBox(this);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;

        if (bounds == null)
            bounds = new BoundingBox(this);
            
        var vertices = bounds.GetVerticesPosition();
        
        if (vertices.Length == 0) return;
        
        foreach (var vertex in vertices)
        {
            Gizmos.DrawSphere(vertex,0.1f);
        }
        //Top Rect
        Gizmos.DrawLine(vertices[0],vertices[1]);
        Gizmos.DrawLine(vertices[1],vertices[2]);
        Gizmos.DrawLine(vertices[2],vertices[3]);
        Gizmos.DrawLine(vertices[3],vertices[0]);
        
        //Bottom Rect
        Gizmos.DrawLine(vertices[4],vertices[5]);
        Gizmos.DrawLine(vertices[5],vertices[6]);
        Gizmos.DrawLine(vertices[6],vertices[7]);
        Gizmos.DrawLine(vertices[7],vertices[4]);
        
        //Rect Conections
        Gizmos.DrawLine(vertices[0],vertices[4]);
        Gizmos.DrawLine(vertices[1],vertices[5]);
        Gizmos.DrawLine(vertices[2],vertices[6]);
        Gizmos.DrawLine(vertices[3],vertices[7]);
    }

    public void Generate()
    {
        var nodeYAmount = Mathf.FloorToInt(worldSpaceSize.z / nodeSeparation)+1;
        var nodeXAmount = Mathf.FloorToInt(worldSpaceSize.x / nodeSeparation)+1;
        var buildStartPosition = transform.position - worldSpaceSize / 2;
        buildStartPosition.y = transform.position.y;

        gridList = new List<Node>();
             
        var gridParent = new GameObject("Grid");
        gridParent.transform.position = transform.position;
            
        var graph = gridParent.AddComponent<Graph>();
        gridParent.AddComponent<Pathfinder>().SetGraph(graph);
            
        for (int i = 0; i < nodeYAmount; i++)
        {
            for (int j = 0; j < nodeXAmount; j++)
            {
                var xTemp = buildStartPosition.x + (j * nodeSeparation);
                var yTemp = buildStartPosition.y;
                var zTemp = buildStartPosition.z + (i * nodeSeparation);

                var ray = new Ray(new Vector3(xTemp, yTemp + 15f, zTemp), Vector3.down);

                if (!Physics.Raycast(ray, out var hit, 100f,
                    LayersUtility.TraversableMask)) continue;
                    
                var temp = Instantiate(Prefab);
                temp.name = $"Node (x: {j}, y: {i})";


                temp.transform.position = new Vector3(xTemp, hit.point.y, zTemp);
                temp.Radius = nodeSeparation / 4;
                temp.SetNeighbourSearchRadius(nodeSeparation);// / 2 + 0.01f);
                temp.transform.parent = gridParent.transform;
            
                gridList.Add(temp);
            }
        }
            
        graph.SetNodeList(gridList);
    }
}


