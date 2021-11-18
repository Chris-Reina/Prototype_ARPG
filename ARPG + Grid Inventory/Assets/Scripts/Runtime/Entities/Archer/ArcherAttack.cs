using System.Collections;
using System.Collections.Generic;
using DoaT.AI;
using UnityEngine;

public class ArcherAttack : State
{
    private readonly ArcherController _zc;
    private readonly ArcherModel _model;

    private float _waitTimer;
    private const float WaitTimerMin = 0.1f;
    private const float WaitTimerMax = 1.4f;

    private float _timer, _timerMax, _animationDelay;
    private bool _effectTriggered, _attackTriggered;

    public ArcherAttack(StateManager stateManager, ArcherController controller) : base(stateManager)
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
        
        _waitTimer = Random.Range(WaitTimerMin, WaitTimerMax);
        _timer = 0;
        Debug.Log("Animation Duration: "+_model.data.arrowAttack.AnimationDuration);
        Debug.Log("Animation Speed Multiplier: "+_model.data.arrowAttack.animationSpeedMultiplier);
        Debug.Log("Animation Duration * speed multiplier: "+_model.data.arrowAttack.AnimationDuration / _model.data.arrowAttack.animationSpeedMultiplier);
        Debug.Log("Animation Duration * speed multiplier * animation effectDelay: "+_model.data.arrowAttack.AnimationDuration / _model.data.arrowAttack.animationSpeedMultiplier * _model.data.arrowAttack.animationEffectDelay);
        
        _timerMax = _model.data.arrowAttack.AnimationDuration;
        _animationDelay = _model.data.arrowAttack.AnimationDuration * _model.data.arrowAttack.animationEffectDelay;
    }

    public override void Execute()
    {
        if (_model.IsDead)
        {
            _stateManager.SetState<ArcherDeath>();
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
                AbilityEffectData.AbilityById[_model.data.arrowAttack.ID].Invoke(_model.data.arrowAttack, _model);
            }

            if (!(_timer >= _timerMax)) return;
            
            _stateManager.SetState<ArcherIdle>();
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
        _model.TriggerAttackCallback?.Invoke(_model.data.arrowAttack.animationSpeedMultiplier);
        if(_zc.DebugMe) Debug.Log("Ability Effect Triggered");
    }
}
