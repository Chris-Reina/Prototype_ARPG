using System;
using UnityEngine;

[Serializable, CreateAssetMenu(fileName = "ArcherModelData", menuName = "Data/Entity/ArcherModelData")]
public class ArcherModelData : ScriptableObject
{
    [Range(0.1f, 2f)] public float rotationSpeed = 1f;
    [Range(0.0001f, 0.1f)] public float movementDetectionThreshold;
    
    public AnimationCurve rotationCurve;
    public float movementSpeed;
    public float nodeDetection;
    public Attack arrowAttack;
    public Attack meleeAttack;

    public float viewDistance;
    public float minDamage;
    public float maxDamage;

    public Tuple<float, float> GetDamageRange()
    {
        return new Tuple<float, float>(minDamage, maxDamage);
    }
}