using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable, CreateAssetMenu(fileName = "Type", menuName = "Data/Energy")]
public class EnergyType : ScriptableObject
{
    public string energyName = "";
    [ColorUsage(true, true)] public Color energyColor = Color.white;
}
