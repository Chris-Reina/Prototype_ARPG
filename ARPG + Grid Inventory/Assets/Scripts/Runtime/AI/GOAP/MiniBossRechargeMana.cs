using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using FSM;
using UnityEngine;

public class MiniBossRechargeMana : MonoBaseState
{
    public override event Action OnNeedsReplan;
    private MiniBossModel _m;
    private MiniBossView _v;
    
    public float _waitTimer;
    public bool finishedRecharge;
        
    private float _timer, _timerMax, _animationDelay;
    private bool _effectTriggered, _attackTriggered;
    
    private bool AmDead => _m.Health <= 0;
    private bool CanUseRangeAttack => _m.RangeAttackCooldown <= 0 && _m.rangeAttackManaCost <= _m.Mana;
    private bool CanRechargeEnergy => _m.RechargeManaCooldown <= 0;
    private bool AtMeleeRange => _m.DistanceToTarget <= _m.meleeDistance;
    private bool KnowsPlayerPosition => _m.PlayerLocation == (PlayerLocation.Known | PlayerLocation.Visible);
    
    private void Awake() 
    {
        _m = GetComponent<MiniBossModel>();
        _v = GetComponent<MiniBossView>();
        
        OnEnter += OnEnterEvent;
        OnExit += OnExitEvent;
        
        OnNeedsReplan += () => OnEnterEvent(null, null);
    }

    private void OnEnterEvent(IState from, IState to)
    {
        Debug.Log("Entering RechargeMana");
        
        _v.isMoving = false;
        _v.kneel = true;
        _v.isKneeling = true;
        finishedRecharge = false;
        
        _timer = 0;
        _timerMax = 2.8f;
        if (AtMeleeRange)
        {
            _m.rangeAttackManaCost = 2;
            _m.rangeAttackCooldownMax = 3f;
        }
        else
        {
            _m.rangeAttackManaCost = 10;
            _m.rangeAttackCooldownMax = 7f;
        }
            
    }
    
    public override void UpdateLoop()
    {
        if (AmDead)
        {
            OnExitEvent(null, null);
            OnNeedsReplan?.Invoke();
            return;
        }

        _m.AddMana();

        if (!_m.IsAtMaxMana) return;

        _v.isKneeling = false;
        _timer += Time.deltaTime;
        
        if (_timer < _timerMax && !finishedRecharge) return;

        finishedRecharge = true;
        _m.SetRechargeOnCooldown();
    }
    
    private void OnExitEvent(IState from, IState to)
    {
        _timer = 0;
        Debug.Log("Exiting RechargeMana");
    }

    public override IState ProcessInput()
    {
        if (finishedRecharge && Transitions.ContainsKey(MiniBossController.AttackRangeState))
            return Transitions[MiniBossController.AttackRangeState];
        
        return this;
    }
}
