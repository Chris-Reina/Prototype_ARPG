using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataObjectsInitializer : MonoBehaviour
{
    public List<ScriptableObject> awakeList;

    private void Awake()
    {
        foreach (var so in awakeList)
        {
            if(so is IAwake awake)
                awake.OnAwake();
        }
    }
}
