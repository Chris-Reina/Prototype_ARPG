using System.Collections;
using System.Collections.Generic;
using DoaT.AI;
using UnityEngine;

public class ArcherIdle : State
{
    private readonly ArcherController _zc;
    private readonly ArcherModel _model;

    public ArcherIdle(StateManager stateManager, ArcherController controller) : base(stateManager)
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
            _stateManager.SetState<ArcherDeath>();
            return;
        }
        
        var visible = _zc.IsTargetVisible();
        if (!visible) return;
        
        
        _model.lastKnownPosition = _model.targetData.Position;
            
        if (Vector3.Distance(_zc.Position, _model.targetData.Position) < _model.data.arrowAttack.range)
        {
            _stateManager.SetState<ArcherAttack>();
        }
        else
        {
            _stateManager.SetState<ArcherMovement>();
        }
    }

    public override void Sleep()
    {
        if (_zc.DebugMe) Debug.Log($"Exiting {GetType()}");
        _zc.currentState = "";
    }
}
