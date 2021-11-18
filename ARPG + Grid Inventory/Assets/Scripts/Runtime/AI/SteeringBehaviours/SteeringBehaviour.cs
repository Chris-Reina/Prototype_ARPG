using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SteeringBehaviour : MonoBehaviour
{
    public bool isActive = true;
    [Range(0f, 1f)] public float force = 1f;
    

    public Vector3 GetDirection(List<GameObject> neighbours)
    {
        return CalculateDirection(neighbours) * (isActive ? force : 0);
    }
    
    protected abstract Vector3 CalculateDirection(List<GameObject> neighbours);
}

