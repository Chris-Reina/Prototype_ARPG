using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using FSM;
using UnityEngine;

public class MiniBossIdle : MonoBaseState
{
    private MiniBossModel _m;
    public override event Action OnNeedsReplan;
    
    private bool AmDead => _m.Health <= 0;
    private bool CanUseRangeAttack => _m.RangeAttackCooldown <= 0 && _m.rangeAttackManaCost <= _m.Mana;
    private bool CanRechargeEnergy => _m.RechargeManaCooldown <= 0;
    private bool AtMeleeRange => _m.DistanceToTarget <= _m.meleeDistance;
    private bool KnowsPlayerPosition => _m.PlayerLocation == PlayerLocation.Known || _m.PlayerLocation == PlayerLocation.Visible;
    
    
    
   
    
    private void Awake() 
    {
        _m = GetComponent<MiniBossModel>();
        OnEnter += OnEnterEvent;
        OnExit += OnExitEvent;

        OnNeedsReplan += () => OnEnterEvent(null, null);
    }
    
    private void OnEnterEvent(IState from, IState to)
    {
        Debug.Log("Entering Idle");

        _m.inCombat = false;
    }
    
    public override void UpdateLoop()
    {
        if (AmDead)
        {
            OnExitEvent(null, null);
            OnNeedsReplan?.Invoke();
            return;
        }

        if (_m.IsTargetVisible(out var pos))
        {
            _m.targetLastKnownPosition = pos;
        }
    }
    
    private void OnExitEvent(IState from, IState to)
    {
        Debug.Log("Exiting Idle");
        _m.inCombat = true;
    }

    public override IState ProcessInput()
    {
        if (KnowsPlayerPosition && Transitions.ContainsKey(MiniBossController.ChaseState))
            return Transitions[MiniBossController.ChaseState];

        return this;
    }


    
    
}
