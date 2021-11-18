using System.Collections;
using System.Collections.Generic;
using DoaT.AI;
using UnityEngine;

public class ZombieUseAbility : State
{
    private readonly ZombieController _zc;
    private readonly ZombieModel _model;

    private float _waitTimer;
    private float _waitTimerMin = 0.05f;
    private float _waitTimerMax = 0.3f;
        
    private float _timer, _timerMax, _animationDelay;
    private bool _effectTriggered, _attackTriggered;

    public ZombieUseAbility(StateManager stateManager, ZombieController controller) : base(stateManager)
    {
        _zc = controller;
        _model = controller.Model;
    }

    public override void Awake()
    {
        if (_zc.DebugMe) Debug.Log($"Entering {GetType()}");
        _zc.currentState = GetType().ToString();

        _attackTriggered = false;
        _effectTriggered = false;
        
        _waitTimer = Random.Range(_waitTimerMin, _waitTimerMax);
        _timer = 0;
        _timerMax = _model.data.attack.AnimationDuration;
        _animationDelay = _timerMax * _model.data.attack.animationEffectDelay;
    }

    public override void Execute()
    {
        if (_model.IsDead)
        {
            _stateManager.SetState<ZombieDeath>();
            return;
        }

        _model.RotationPoint = _model.targetData.Position;
        
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
                AbilityEffectData.AbilityById[_model.data.attack.ID].Invoke(_model.data.attack, _model);
            }

            if (!(_timer >= _timerMax)) return;
            
            _stateManager.SetState<ZombieIdle>();
        }
    }

    public override void Sleep()
    {
        if (_zc.DebugMe) Debug.Log($"Exiting {GetType()}");
        _zc.currentState = "";
    }

    private void TriggerAttack()
    {
        _attackTriggered = true;
        _model.TriggerAttackCallback?.Invoke(_model.data.attack.animationSpeedMultiplier);
        if(_zc.DebugMe) Debug.Log("Ability Effect Triggered");
    }
}
