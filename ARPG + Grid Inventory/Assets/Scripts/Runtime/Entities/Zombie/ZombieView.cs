using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieView : MonoBehaviour, IUpdate
{
    private ZombieModel _model;
    private ZombieController _controller;

    private static readonly int IsMoving = Animator.StringToHash("IsMoving");
    private static readonly int Attack = Animator.StringToHash("Attack");
    private static readonly int AttackSpeedMultiplier = Animator.StringToHash("AttackSpeedMultiplier");
    private static readonly int IsDead = Animator.StringToHash("IsDead");

    private void Awake()
    {
        _model = GetComponent<ZombieModel>();
        _model.TriggerAttackCallback += SetAttackTrigger;
    }

    private void Start()
    {
        UpdateManager.AddUpdate(this);
    }

    public void OnUpdate()
    {
        _model.animator.SetBool(IsMoving, _model.IsMoving);
        _model.animator.SetBool(IsDead, _model.IsDead);
    }

    private void SetAttackTrigger(float time)
    {
        _model.animator.SetFloat(AttackSpeedMultiplier, time);
        _model.animator.SetTrigger(Attack);
        
    }
}
