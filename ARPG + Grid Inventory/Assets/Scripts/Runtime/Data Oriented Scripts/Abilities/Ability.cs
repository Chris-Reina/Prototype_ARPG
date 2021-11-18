using System;
using UnityEngine;

[Serializable, CreateAssetMenu(fileName = "Ability", menuName = "Abilities/Ability")]
public class Ability : ScriptableObject
{
    public uint ID;
    public new string name;
    public string description;
    public float range;
    
    
    public AnimationClip animationClip;
    public Action abilityEffect;
    [Range(0f, 1f)] public float animationEffectDelay;
    [Range(0.5f, 4f)] public float animationSpeedMultiplier = 1f;

    public AudioClip sound;
    public Sprite icon;


    public float AnimationDuration => animationClip ? animationClip.length / animationSpeedMultiplier : 0f;
}