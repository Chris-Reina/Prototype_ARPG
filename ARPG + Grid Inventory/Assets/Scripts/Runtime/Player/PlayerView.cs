using System;
using System.Collections;
using System.Collections.Generic;
using DoaT;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerView : MonoBehaviour
{
    private PlayerModel _model;
    private PlayerController _controller;
    private Animator _animator;

    private static readonly int Attack = Animator.StringToHash("Attack");
    private static readonly int IsDead = Animator.StringToHash("IsDead");
    private static readonly int IsMoving = Animator.StringToHash("IsMoving");
    private static readonly int ShouldStandUp = Animator.StringToHash("ShouldStandUp");
    
    private static readonly int IdleSpeedMultiplier = Animator.StringToHash("IdleSpeedMultiplier");
    private static readonly int DeathSpeedMultiplier = Animator.StringToHash("DeathSpeedMultiplier");
    private static readonly int AttackSpeedMultiplier = Animator.StringToHash("AttackSpeedMultiplier");
    private static readonly int StandUpSpeedMultiplier = Animator.StringToHash("StandUpSpeedMultiplier");
    private static readonly int MovementSpeedMultiplier = Animator.StringToHash("MovementSpeedMultiplier");

    [Header("Main Armor")]
    //[SerializeField] private GameObject helmetAnchor;
    //[SerializeField] private GameObject chestAnchor;
    //[SerializeField] private GameObject pantsAnchor;
    //[SerializeField] private GameObject shoulderLeftAnchor;
    //[SerializeField] private GameObject shoulderRightAnchor;
    public EquipmentItemSlot armorSlot;
    public GameObject pants;
    public GameObject chest;
    
    [Header("Belt")]
    public EquipmentItemSlot beltSlot;
    public GameObject belt;
    
    [Header("Helmet")]
    public EquipmentItemSlot helmetSlot;
    public GameObject helmet;
    
    [Header("Weapons")]
    //[SerializeField] private GameObject mainHandAnchor;
    //[SerializeField] private GameObject offHandAnchor;
    public EquipmentItemSlot weaponSlot;
    public GameObject sword;
    public GameObject swordOff;
    
    [Header("Gloves")]
    public EquipmentItemSlot glovesSlot;
    public GameObject gloves;
    //[SerializeField] private GameObject gloveLeftAnchor;
    //[SerializeField] private GameObject gloveRightAnchor;

    [Header("Boots")] 
    public EquipmentItemSlot bootSlot;
    public GameObject boots;
    //[SerializeField] private GameObject bootLeftAnchor;
    //[SerializeField] private GameObject bootRightAnchor;


    private void Awake()
    {
        _model = GetComponent<PlayerModel>();
        _animator = GetComponent<Animator>();
        
        _model.TriggerAttack += TriggerAttack;

        _model.OnAnimatorMovementChange += UpdateMovement;
        _model.OnAnimatorDeathChange += UpdateDeath;
        _model.OnAnimatorStandUpStatusChange += UpdateStandUpStatus;

        _model.AnimationStandUpSpeed = _animator.GetFloat(StandUpSpeedMultiplier);
        _model.AnimationIdleSpeed = _animator.GetFloat(IdleSpeedMultiplier);
        _model.AnimationMovementSpeed = _animator.GetFloat(MovementSpeedMultiplier);
        _model.AnimationAttackSpeed = _animator.GetFloat(AttackSpeedMultiplier);
        _model.AnimationDeathSpeed = _animator.GetFloat(DeathSpeedMultiplier);

        _model.OnAnimatorStandUpSpeedChange += (value) => _animator.SetFloat(StandUpSpeedMultiplier, value);
        _model.OnAnimatorIdleSpeedChange += (value) => _animator.SetFloat(IdleSpeedMultiplier, value);
        _model.OnAnimatorMovementSpeedChange += (value) => _animator.SetFloat(MovementSpeedMultiplier, value);
        _model.OnAnimatorAttackSpeedChange += (value) => _animator.SetFloat(AttackSpeedMultiplier, value);
        _model.OnAnimatorDeathSpeedChange += (value) => _animator.SetFloat(DeathSpeedMultiplier, value);
    }

    private void Start()
    {
        EventManager.Subscribe(EventsData.OnInteractionWithUI, UpdateArmor);
        UpdateArmor();
    }

    // private void LateUpdate()
    // {
    //     UpdateArmor();
    // }


    private void TriggerAttack()
    {
        _model.Animator.SetTrigger(Attack);
    }

    private void UpdateMovement(bool isMoving)
    {
        _model.Animator.SetBool(IsMoving, isMoving);
    }
    
    private void UpdateDeath(bool isDead)
    {
        _model.Animator.SetBool(IsMoving, isDead);
    }
    
    private void UpdateStandUpStatus(bool shouldStandUp)
    {
        _model.Animator.SetBool(ShouldStandUp, shouldStandUp);
    }

    private void UpdateArmor(params object[] parameters)
    {
        var data = _model.Inventory.data;

        var weaponOccupied = data.characterItems.IsOccupied(weaponSlot, true);
        var weaponOccupiedOff = data.characterItems.IsOccupied(weaponSlot, false);
        sword.SetActive(weaponOccupied);
        swordOff.SetActive(weaponOccupiedOff);
        
        var armorOccupied = data.characterItems.IsOccupied(armorSlot);
        pants.SetActive(armorOccupied);
        chest.SetActive(armorOccupied);

        var helmetOccupied = data.characterItems.IsOccupied(helmetSlot);
        helmet.SetActive(helmetOccupied);
        
        var beltOccupied = data.characterItems.IsOccupied(beltSlot);
        belt.SetActive(beltOccupied);

        var glovesOccupied = data.characterItems.IsOccupied(glovesSlot);
        gloves.SetActive(glovesOccupied);
        
        var bootsOccupied = data.characterItems.IsOccupied(bootSlot);
        boots.SetActive(bootsOccupied);
    }
}