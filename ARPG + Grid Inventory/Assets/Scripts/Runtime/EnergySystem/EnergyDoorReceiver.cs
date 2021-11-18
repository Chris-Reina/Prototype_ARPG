using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnergyDoorReceiver : MonoBehaviour, IEntity, IEnergySensitive, IEnergyReceiver
{
    public EnergyDoor energyDoor;

    public Transform feedbackPosition;

    public Vector3 Position => transform.position;
    public string Name => "Energy Door Altar";
    public float Durability => 1f;
    public Transform Transform => transform;
    public GameObject GameObject => gameObject;
    public bool IsEnemy => true;
    public bool IsDead => false;
    public Transform FeedbackPosition
    {
        get => feedbackPosition;
        set => feedbackPosition = value;
    }

    public bool IsTarget { get; set; }
    public EnergyType EnergyType { get; set; }

    public bool IsAffected { get; set; }
    
    public bool Explode(EnergyType type)
    {
        if (IsAffected) return false;
    
        if (energyDoor.CanOpenDoor(type))
        {
            energyDoor.OpenDoor(type);
            return true;
        }
        
        return false;
    }
}



