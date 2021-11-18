using System;
using System.Collections;
using System.Collections.Generic;
using FSM;
using UnityEngine;
using Random = UnityEngine.Random;

public class MiniBossAttackMelee : MonoBaseState
{
    public override event Action OnNeedsReplan;
    private MiniBossModel _m;
    private MiniBossView _v;
    
    public float _waitTimer;
    public float _waitTimerMin = 0.02f;
    public float _waitTimerMax = 0.1f;
        
    private float _timer, _timerMax, _animationDelay;
    private bool _effectTriggered, _attackTriggered;

    private bool AmDead => _m.Health <= 0;
    private bool CanUseRangeAttack => _m.RangeAttackCooldown <= 0 && _m.Mana >= _m.rangeAttackManaCost;
    private bool CanRechargeEnergy => _m.RechargeManaCooldown <= 0;
    private bool AtMeleeRange => _m.DistanceToTarget <= _m.meleeDistance;
    
    
    
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
        Debug.Log("Entering Attack Melee");
        _attackTriggered = false;
        _effectTriggered = false;
        _m.animationOverrider.ChangeAttackAnimation(_m.data.attack.animationClip);
        _v.isMoving = false;
                
        _waitTimer = Random.Range(_waitTimerMin, _waitTimerMax);
        _timer = 0;
        _timerMax = _m.data.attack.AnimationDuration;
        _animationDelay = _timerMax * _m.data.attack.animationEffectDelay;
    }
    
    public override void UpdateLoop()
    {
        Debug.Log(GetType() + " Update");
    
        if (AmDead || CanUseRangeAttack || (!CanUseRangeAttack && CanRechargeEnergy))
        {
            OnExitEvent(null, null);
            OnNeedsReplan?.Invoke();
            return;
        }

        _m.RotationPoint = _m.targetData.Position;
        
        if (_waitTimer > 0)
        {
            _waitTimer -= Time.deltaTime;
        }
        else
        {
            if (!_attackTriggered)
                TriggerAttack();

            _timer += Time.deltaTime;

            if (_timer >= _animationDelay && !_effectTriggered)
            {
                _effectTriggered = true;
                AbilityEffectData.AbilityById[_m.data.attack.ID].Invoke(_m.data.attack, _m);
            }

            if (!(_timer >= _timerMax)) return;
            
            if (!AtMeleeRange || !_m.IsPlayerAlive)
            {
                OnExitEvent(null, null);
                OnNeedsReplan?.Invoke();
            }
            else
            {
                _attackTriggered = false;
                _effectTriggered = false;
                
                _waitTimer = Random.Range(_waitTimerMin, _waitTimerMax);
                _timer = 0;
            }
        }
    }
    
    private void OnExitEvent(IState from, IState to)
    {
        Debug.Log("Exiting Attack Melee");
    }

    public override IState ProcessInput()
    {
        return this;
    }

    private void TriggerAttack()
    {
        _attackTriggered = true;
        _v.BeginAttack(_m.data.attack.animationSpeedMultiplier);
    }
}
