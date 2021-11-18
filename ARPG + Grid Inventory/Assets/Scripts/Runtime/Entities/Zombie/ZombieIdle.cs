using System.Collections;
using System.Collections.Generic;
using DoaT.AI;
using UnityEngine;

public class ZombieIdle : State
{
    private readonly ZombieController _zc;
    private readonly ZombieModel _model;

    public ZombieIdle(StateManager stateManager, ZombieController controller) : base(stateManager)
    {
        _zc = controller;
        _model = controller.Model;
    }

    public override void Awake()
    {
        if (_zc.DebugMe) Debug.Log($"Entering {GetType()}");
        _zc.currentState = GetType().ToString();
    }

    public override void Execute()
    {
        if (_model.IsDead)
        {
            _stateManager.SetState<ZombieDeath>();
            return;
        }
        
        var isIt = _zc.IsTargetVisible();
        
        if (isIt)
        {
            _model.lastKnownPosition = _model.targetData.Position;
            
            if (Vector3.Distance(_zc.Position, _model.targetData.Position) < _model.data.attack.range)
            {
                _stateManager.SetState<ZombieUseAbility>();
            }
            else
            {
                _stateManager.SetState<ZombieMovement>();
            }
        }
    }

    public override void Sleep()
    {
        if (_zc.DebugMe) Debug.Log($"Exiting {GetType()}");
        _zc.currentState = "";
    }
}
