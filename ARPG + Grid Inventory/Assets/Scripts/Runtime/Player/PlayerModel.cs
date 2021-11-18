using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DoaT;
using DoaT.Attributes;
using DoaT.AI;
using UnityEngine;
using Attribute = DoaT.Attributes.Attribute;
using Object = UnityEngine.Object;

public class PlayerModel : Model
{
    public CursorGameSelection selection;
    public PlayerView view;
    public PlayerController controller;
    public PlayerStreamedData streamedData;

    public float movementSpeed;
    public float nodeDetection;
    public float minMoveDistance = 1f;
    [Range(1, 50)] public int shortPathStepAmount;
    [Range(0.01f, 10f)] public float interactRange;
    

    private bool _isMoving;
    private bool _isDead;
    private bool _shouldStandUp;
    private Attribute _mana = default;
    private float _animationDeathSpeed;
    private float _animationSpeedStandUp;
    private float _animationSpeedMovement;
    private float _animationSpeedAttack;
    private float _animationSpeedIdle;

    [Range(0.1f, 2f), 
     SerializeField] private float rotationSpeed = 1f;

    [SerializeField] private string _resourceAttributeTag;
    [SerializeField] private Health _health = default;
    [SerializeField] private Animator _animator = default;
    [SerializeField] private Inventory _inventory = default;
    [SerializeField] private Pathfinder _pathfinder = default;
    [SerializeField] private EnergyHolder _energyHolder = default;
    [SerializeField] private AnimationCurve _rotationCurve = default;
    [SerializeField] private PlayerAnimationOverrider _animationOverrider = default;
    [SerializeField] private Vector3 _raycastDisplacement = new Vector3(0, 0.5f, 0);


    public Attribute Mana => _mana;
    public Health Health => _health;
    public Animator Animator => _animator;
    public Inventory Inventory => _inventory;
    public Pathfinder Pathfinder => _pathfinder;
    public PlayerAnimationOverrider AnimationOverrider => _animationOverrider;

    public bool IsMoving
    {
        get => _isMoving;
        set
        {
            if (value == _isMoving) return;
            
            OnAnimatorMovementChange?.Invoke(value);
            _isMoving = value;
        }
    }
    public bool IsDead
    {
        get => _isDead;
        set
        {
            if (value == _isDead) return;   
            
            OnAnimatorDeathChange?.Invoke(value);
            _isDead = value;
        }
    }
    public bool ShouldStandUp
    {
        get => _shouldStandUp;
        set
        {
            if (value == _shouldStandUp) return;
            
            OnAnimatorStandUpStatusChange?.Invoke(value);
            _shouldStandUp = value;
        }
    }
    public float AnimationDeathSpeed
    {
        get => _animationDeathSpeed;
        set
        {
            if (Math.Abs(value - _animationDeathSpeed) < 0.0001f) return;
            
            OnAnimatorDeathSpeedChange?.Invoke(value);
            _animationDeathSpeed = value;
        }
    }
    public float AnimationStandUpSpeed 
    {
        get => _animationSpeedStandUp;
        set
        {
            if (Math.Abs(value - _animationSpeedStandUp) < 0.0001f) return;
            
            OnAnimatorStandUpSpeedChange?.Invoke(value);
            _animationSpeedStandUp = value;
        }
    }
    public float AnimationMovementSpeed
    {
        get => _animationSpeedMovement;
        set
        {
            if (Math.Abs(value - _animationSpeedMovement) < 0.0001f) return;
            
            OnAnimatorMovementSpeedChange?.Invoke(value);
            _animationSpeedMovement = value;
        }
    }
    public float AnimationAttackSpeed
    {
        get => _animationSpeedAttack;
        set
        {
            if (Math.Abs(value - _animationSpeedAttack) < 0.0001f) return;
            
            OnAnimatorAttackSpeedChange?.Invoke(value);
            _animationSpeedAttack = value;
        }
    }
    public float AnimationIdleSpeed
    {
        get => _animationSpeedIdle;
        set
        {
            OnAnimatorIdleSpeedChange?.Invoke(value);
            _animationSpeedIdle = value;
        }
    }
    
    
    public bool IsInAbility { get; set; }
    
    
    public Vector3 RotationPoint { get; set; }
    public Vector3 Position => GetPosition();
    public Vector3 RayInitPosition => transform.position + _raycastDisplacement;
    private Vector3 RotationDirection => RotationPoint - transform.position;
    public Vector3 RotationDirectionNormalized => RotationDirection.normalized;
    public float RotationSpeedCurve => _rotationCurve.Evaluate(Vector3.Angle(RotationDirection, transform.forward) / 180) * 360 * rotationSpeed;

    #region Observer Events
    public Action TriggerAttack;
    public event Action<bool> OnAnimatorMovementChange;
    public event Action<bool> OnAnimatorDeathChange;
    public event Action<bool> OnAnimatorStandUpStatusChange;
    public event Action<float> OnAnimatorDeathSpeedChange;
    public event Action<float> OnAnimatorMovementSpeedChange;
    public event Action<float> OnAnimatorIdleSpeedChange;
    public event Action<float> OnAnimatorAttackSpeedChange;
    public event Action<float> OnAnimatorStandUpSpeedChange;
    #endregion
    

    private void Awake()
    {
        if (_animationOverrider == null) _animationOverrider = GetComponent<PlayerAnimationOverrider>();
        if (_energyHolder == null) _energyHolder = GetComponent<EnergyHolder>();
        if (_pathfinder == null) _pathfinder = FindObjectOfType<Pathfinder>();
        if (_inventory == null) _inventory = GetComponent<Inventory>();
        if (_animator == null) _animator = GetComponent<Animator>();
        if (_health == null) _health = GetComponent<Health>();
        
        if (controller == null) controller = GetComponent<PlayerController>();
        if (view == null) view = GetComponent<PlayerView>();

        _health.OnDeath += () => IsDead = true;
        _health.OnHealthRefill += () => IsDead = false;
        
        EventManager.Subscribe(EventsData.OnEntityKilled, (parameters) => Health.Heal(40)); //TEMP
        
        streamedData.positionCallback += GetPosition;
        streamedData.energyStoredCallback += GetEnergies;
        streamedData.isDeadCallback += () => IsDead;
    }

    private void Start()
    {
        _mana = GetComponent<AttributeManager>().TryGetAttribute(_resourceAttributeTag);
        ShouldStandUp = true;
    }

    private Vector3 GetPosition()
    {
        return transform.position;
    }
    private EnergyType[] GetEnergies()
    {
        var array = new EnergyType[3]
            {_energyHolder.firstEnergy, _energyHolder.secondEnergy, _energyHolder.thirdEnergy};

        return array.Where(x => x != _energyHolder.None).ToArray();
    }

    // private void OnDeath() { IsDead = true; }
    // private void OnHealthRefill() { IsDead = false; }
}
