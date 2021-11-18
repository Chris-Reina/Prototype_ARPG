using System;
using System.Collections;
using System.Collections.Generic;
using DoaT.AI;
using DoaT.Attributes;
using UnityEngine;

public class ZombieModel : Model
{
    public ZombieController controller;
    public ZombieView view;

    public ZombieModelData data;

    public Health health;
    public LootTable lootTable;
    public Pathfinder pathfinder;
    public Animator animator;
    public ZombieAnimationOverrider animationOverrider;
    public Vector3 lastKnownPosition;
    public PlayerStreamedData targetData;
    public new Collider collider;
    public EnergyOrb energyOrb;

    private Vector3 _rotationPoint;
    private Vector3 _positionLastFrame;
    

    public Action<float> TriggerAttackCallback;
    public Vector3 RotationPoint
    {
        get => _rotationPoint;
        set =>
            //Debug.Log($"Rotation Point Changed from {_rotationPoint} to {value}");
            _rotationPoint = value;
    }

    public EnergyOrb EnergyOrb => energyOrb;
    public bool IsMoving { get; set; }
    public bool IsDead => health.IsDead;
    public float MaxDegreesDelta => data.rotationSpeed * 360f;

    public float RotationSpeedCurve =>
        data.rotationCurve.Evaluate(Vector3.Angle(RotationDirection, transform.forward) / 180) * 360 *
        data.rotationSpeed;
    public Vector3 RotationDirectionNormalized => RotationDirection.normalized;
    public Vector3 RayInitPosition => transform.position + new Vector3(0, 0.5f, 0);
    private Vector3 RotationDirection => RotationPoint - transform.position;

    public bool Dissolve { get; set; }
    
    
    
    public float speedMultiplier = 1f;
    public float damageTakenMultiplier = 1f;
    public float MovementSpeed => data.movementSpeed * speedMultiplier;
    public Renderer zombieRenderer;
    public Color cold;
    public Color fire;
    public Color lightning;
    private static readonly int Tint = Shader.PropertyToID("_Tint");

    private void Awake()
    {
        CheckComponent(ref animationOverrider);
        CheckComponent(ref controller);
        CheckComponent(ref animator);
        CheckComponent(ref health);
        CheckComponent(ref view);
        CheckComponent(ref collider);
        CheckComponent(ref energyOrb);
        
        TryFindObjectOfType(ref pathfinder);
    }

    private void LateUpdate()
    {
        _positionLastFrame = transform.position;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(RotationPoint, 0.3f);
    }
    
    public override void AffectCold()
    {
        speedMultiplier = 0.4f;

        zombieRenderer.material.SetColor(Tint, cold);
    }

    public override void AffectFire()
    {
        health.TakeDamage(50f);
        zombieRenderer.material.SetColor(Tint, fire);
    }

    public override void AffectLightning()
    {
        damageTakenMultiplier = 2f;
        zombieRenderer.material.SetColor(Tint, lightning);
    }
}