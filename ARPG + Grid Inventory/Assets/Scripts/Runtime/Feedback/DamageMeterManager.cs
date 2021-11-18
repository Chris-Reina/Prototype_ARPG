using System;
using System.Collections;
using System.Collections.Generic;
using DoaT;
using UnityEngine;

public class DamageMeterManager : MonoBehaviour
{
    [SerializeField] private DamageFeedback feedback = default;
    
    private void Start()
    {
        EventManager.Subscribe(EventsData.OnEntityDamageTaken,SpawnDamageFeedback);
    }
    
    private void SpawnDamageFeedback(params object[] parameters)
    {
        var location = (Vector3) parameters[0];
        var target = (IAttackable) parameters[1];
        var damage = (float) parameters[2];
        var isCrit = (bool) parameters[3];

        var temp = Instantiate(feedback);
        temp.transform.position = location + new Vector3(0, 2f, 0);
        temp.Initialize((int)damage, isCrit);
    }
}
