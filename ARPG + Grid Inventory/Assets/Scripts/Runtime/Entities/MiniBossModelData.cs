using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable, CreateAssetMenu(fileName = "MiniBossModelData", menuName = "Data/Entity/MiniBossModelData")]
public class MiniBossModelData : ScriptableObject
{
    [Range(0.1f, 2f)] public float rotationSpeed = 1f;
    [Range(0.0001f, 0.1f)] public float movementDetectionThreshold;
    
    public AnimationCurve rotationCurve;
    public float movementSpeed;
    public float nodeDetection;
    public float neighbourRadiusDetection;
    public Ability attack;
    public Ability attackRange;

    public float viewDistance;
    public float minDamage;
    public float maxDamage;

    public Tuple<float, float> GetDamageRange()
    {
        return new Tuple<float, float>(minDamage, maxDamage);
    }
}