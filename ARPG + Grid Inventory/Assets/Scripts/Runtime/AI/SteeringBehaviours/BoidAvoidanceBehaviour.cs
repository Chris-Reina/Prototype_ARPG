using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class BoidAvoidanceBehaviour : SteeringBehaviour
{ 
    public float separationDistance = 2f;

    public bool debugThis;
    protected override Vector3 CalculateDirection(List<GameObject> neighbours)
    {
        var boidAvoid = new Vector3(0, 0, 0);

        if (debugThis)
        {
            Debug.Log($"neighbours: {neighbours.Count}");
        }

        if (neighbours.Count == 0) return boidAvoid;
        
        foreach (GameObject neigh in neighbours)
        {
            if (neigh == null) continue;
            var dir = neigh.transform.position - transform.position;
            boidAvoid -= dir.normalized / dir.magnitude;
        }

        return boidAvoid;
    }
}