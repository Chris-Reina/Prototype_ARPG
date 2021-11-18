using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetBehaviour : SteeringBehaviour
{
    public GameObject target;
    public float distanceToMaxForce;

    protected override Vector3 CalculateDirection(List<GameObject> neighbours)
    {
        float distanceClamped = Mathf.Clamp(Vector3.Distance(target.transform.position, transform.position), 0, distanceToMaxForce);
        float forceCorrection = distanceClamped/distanceToMaxForce;

        var forceNormalized = (target.transform.position - transform.position).normalized * forceCorrection;

        if (Vector3.Distance(transform.position,target.transform.position) <= 1)
        {
            forceNormalized *= -1;
        }
        
        return forceNormalized;
    }
}