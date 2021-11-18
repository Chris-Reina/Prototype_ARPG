using System;
using System.Collections;
using System.Collections.Generic;
using DoaT;
using UnityEngine;
using Random = UnityEngine.Random;

public class WorldItemManager : MonoBehaviour
{
    public WorldItem prefab;

    private void Start()
    {
        EventManager.Subscribe(EventsData.OnWorldLootSpawn, SpawnItem);
    }

    private void SpawnItem(params object[] parameters)
    {
        var location = (Vector3) parameters[0];
        var item = (Item) parameters[1];

        var temp = Instantiate(prefab, GetValidLocation(location, prefab.Radius, 1f), Quaternion.identity);
        temp.Initialize(item);
    }

    private Vector3 GetValidLocation(Vector3 anchorPosition, float radius, float areaRadius)
    {
        var watchdog = 0;
        var position = anchorPosition;

        while (watchdog < 100)
        {
            if (watchdog == 0)
            {
                if (IsValidPosition(anchorPosition, radius, out var newPosition))
                {
                    position = newPosition;
                    break;
                }
            }
            else
            {
                var randomDisplacement = new Vector3(Random.Range(0, areaRadius), 0, Random.Range(0, areaRadius));
                Debug.Log(randomDisplacement);

                if (IsValidPosition(anchorPosition + randomDisplacement, radius, out var newPosition))
                {
                    position = newPosition;
                    break;
                }
            }

            watchdog += 1;
        }

        return position;
    }
    private bool IsValidPosition(Vector3 position, float radius, out Vector3 newPosition)
    {
        if (Physics.Raycast(new Ray(position + new Vector3(0, 4, 0), Vector3.down),
            out var hit,
            5,
            LayersUtility.TraversableMask))
        {
            //Debug.Log("There was traversable");
            
            var pos = hit.point + new Vector3(0, prefab.Radius, 0);

            if (Physics.OverlapSphere(pos, radius, LayersUtility.WorldItemMask,
                    QueryTriggerInteraction.Collide).Length == 0)
            {
                //Debug.Log("There was not another sphere");
                newPosition = hit.point;
                return true;
            }
        }
        
        //Debug.Log("You cannot put it there!!!");
        newPosition = position;
        return false;
    }
}
