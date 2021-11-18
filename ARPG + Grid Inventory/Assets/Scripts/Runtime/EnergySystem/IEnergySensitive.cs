using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IEnergySensitive
{
    bool IsAffected { get; set; }

    bool Explode(EnergyType type);

}