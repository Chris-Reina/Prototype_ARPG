using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable, CreateAssetMenu(fileName = "PlayerStreamedData",menuName = "Data/Player/StreamedData")]
public class PlayerStreamedData : ScriptableObject, IAwake
{
    public Vector3 Position => positionCallback?.Invoke() ?? Vector3.zero;
    public EnergyType[] EnergyStored => energyStoredCallback?.Invoke();
    public bool IsDead => isDeadCallback?.Invoke() ?? true;
    
    public Func<Vector3> positionCallback;
    public Func<EnergyType[]> energyStoredCallback;
    public Func<bool> isDeadCallback;
    
    public void OnAwake()
    {
        //positionCallback = default;
        //energyStoredCallback = default;
    }

    private void OnDisable()
    {
        positionCallback = default;
        energyStoredCallback = default;
        isDeadCallback = default;
    }
}
