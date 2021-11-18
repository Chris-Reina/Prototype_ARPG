using System;
using System.Collections;
using System.Collections.Generic;
using FSM;
using UnityEngine;
using Random = UnityEngine.Random;

public class MiniBossAttackRange : MonoBaseState
{
    public override event Action OnNeedsReplan;
    private MiniBossModel _m;
    private MiniBossView _v;
    
    public float _waitTimer;
        
    private float _timer, _timerMax, _animationDelay;
    private bool _effectTriggered, _attackTriggered;
    
    private bool AmDead => _m.Health <= 0;
    private bool CanUseRangeAttack => _m.RangeAttackCooldown <= 0 && _m.rangeAttackManaCost <= _m.Mana;
    
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
        Debug.Log("Entering Attack Range");
        
        _attackTriggered = false;
        _effectTriggered = false;
        _m.animationOverrider.ChangeAttackAnimation(_m.data.attackRange.animationClip);
        _v.isMoving = false;
        
        _timer = 0;
        _timerMax = _m.data.attackRange.AnimationDuration;
        _animationDelay = _timerMax * _m.data.attackRange.animationEffectDelay;
    }
    
    public override void UpdateLoop()
    {
        Debug.Log(GetType() + " Update");
    
        if (AmDead || !CanUseRangeAttack)
        {
            OnExitEvent(null, null);
            OnNeedsReplan?.Invoke();
            return;
        }

        _m.RotationPoint = _m.targetData.Position;

        if (!_attackTriggered)
            TriggerAttack();

        _timer += Time.deltaTime;

        if (_timer >= _animationDelay && !_effectTriggered)
        {
            _effectTriggered = true;
            AbilityEffectData.AbilityById[_m.data.attackRange.ID].Invoke(_m.data.attackRange, _m);
        }

        Debug.Log("Timer: " + _timer);
        if (_timer < _timerMax) return;

        _m.SetRangeAttackOnCooldown();
        _m.SpendMana();
        
        OnExitEvent(null, null);
        OnNeedsReplan?.Invoke();
        
    }
    
    private void OnExitEvent(IState from, IState to)
    {
        Debug.Log("Exiting Attack Range");
    }

    public override IState ProcessInput()
    {
        return this;
    }

    private void TriggerAttack()
    {
        _attackTriggered = true;
        _v.BeginAttack(_m.data.attackRange.animationSpeedMultiplier);
    }
}
