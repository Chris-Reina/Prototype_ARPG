using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniBossView : MonoBehaviour
{
    private MiniBossModel _m;
    private Animator _anim;

    [Header("States")] 
    public bool isMoving;
    public bool kneel;
    public bool isKneeling;
    public bool dead;

    private static readonly int IsMoving = Animator.StringToHash("IsMoving");
    private static readonly int Attack = Animator.StringToHash("Attack");
    private static readonly int IsDead = Animator.StringToHash("IsDead");
    private static readonly int IsKneeling = Animator.StringToHash("IsKneeling");
    private static readonly int Kneel = Animator.StringToHash("Kneel");
    private static readonly int AttackSpeedMultiplier = Animator.StringToHash("AttackSpeedMultiplier");

    private void Awake()
    {
        _m = GetComponent<MiniBossModel>();
        _anim = GetComponent<Animator>();
    }

    private void Update()
    {
        if (kneel)
        {
            kneel = false;
            _anim.SetTrigger(Kneel);
        }
        _anim.SetBool(IsMoving, isMoving);
        _anim.SetBool(IsDead, dead);
        _anim.SetBool(IsKneeling, isKneeling);
    }

    public void BeginAttack(float attackAnimationSpeedMultiplier)
    {
        _anim.SetFloat(AttackSpeedMultiplier, attackAnimationSpeedMultiplier);
        _anim.SetTrigger(Attack);
    }
}
