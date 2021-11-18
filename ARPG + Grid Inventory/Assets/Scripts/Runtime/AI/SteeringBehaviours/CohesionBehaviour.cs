using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CohesionBehaviour : SteeringBehaviour
{
    public float stoppingThreshold = 1;
    
    private Vector3 debug;
    protected override Vector3 CalculateDirection(List<GameObject> neighbours)
    {
        debug = transform.position;
        Vector3 center = Vector3.zero;
        foreach (var neighbour in neighbours)
        {
            center += neighbour.transform.position;
        }
        if (neighbours.Count != 0)
        {
            center /= neighbours.Count;
            var dir = center - transform.position;

            //dir = dir.magnitude <= stoppingThreshold ? Vector3.zero : dir.normalized;
            dir = Vector3.Lerp(Vector3.zero, dir.normalized,
                Mathf.Clamp(dir.magnitude, 0, stoppingThreshold) / stoppingThreshold);
            dir.y = 0;
            debug = dir;
            return dir;
        }
        
        return Vector3.zero;
    }

    /*private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        
        Gizmos.DrawRay(transform.position,debug);
    }*/
}