using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlignmentBehaviour: SteeringBehaviour
{
    protected override Vector3 CalculateDirection(List<GameObject> neighbours)
    {
        Vector3 viewDir = Vector3.zero;

        foreach (GameObject soldier in neighbours)
        {
            if (soldier == null) continue;
            
            viewDir += soldier.transform.forward;
        }

        viewDir.y = 0;
        viewDir = viewDir.normalized;
        
        if(viewDir.normalized == transform.forward)
            viewDir = Vector3.zero;
        
        return viewDir;
    }
}