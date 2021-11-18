using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectAvoidanceBehaviour : SteeringBehaviour
{
    public LayerMask obstacles;
    public float detectionRadius;
    public float soldierRadius = 0.4f;

    public Vector3 debug = Vector3.zero;

    public bool debugThis = false;
    
    protected override Vector3 CalculateDirection(List<GameObject> neighbours)
    {
        Vector3 avoidObstacle = Vector3.zero;

        var cols = Physics.OverlapSphere(transform.position, detectionRadius, obstacles);

        /*foreach (var obj in cols)
        {
            avoidObstacle += (transform.position - obj.ClosestPoint(transform.position)).normalized;
        }*/
        
        if(debugThis)
            Debug.Log(cols.Length+"  was the length");
        
        if (cols.Length > 0)
        {
            RaycastHit hit;
        
            if (Physics.Raycast(transform.position, transform.forward, out hit, detectionRadius*3, obstacles))
            {
                avoidObstacle += (transform.forward + hit.normal).normalized;
                
                if(debugThis)
                    Debug.Log("Middle");
            }else if (Physics.Raycast(transform.position+(transform.right*soldierRadius), transform.forward, out hit, detectionRadius*3, obstacles))
            {
                avoidObstacle += (transform.forward + hit.normal).normalized;
                
                if(debugThis)
                    Debug.Log("Right");
            }
            else if (Physics.Raycast(transform.position-(transform.right*soldierRadius), transform.forward, out hit, detectionRadius*3, obstacles))
            {
                avoidObstacle += (transform.forward + hit.normal).normalized;
                if(debugThis)
                    Debug.Log("Left");
            }
        }

        avoidObstacle.y = 0;

        debug = avoidObstacle;
        return avoidObstacle.normalized;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;

        Gizmos.DrawRay(transform.position, debug);
        Gizmos.color = Color.blue;
        Gizmos.DrawRay(transform.position-(transform.right*soldierRadius), transform.forward);
        Gizmos.DrawRay(transform.position+(transform.right*soldierRadius), transform.forward);
    }
}
