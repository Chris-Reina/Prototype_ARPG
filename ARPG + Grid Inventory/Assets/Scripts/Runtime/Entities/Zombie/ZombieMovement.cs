using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DoaT.AI;
using UnityEngine;

public class ZombieMovement : State
{
    private readonly ZombieController _zc;
    private readonly ZombieModel _model;

    private float _distanceRecalculatePoint = 1f;
    private bool _lastFrameTargetVisible;


    public ZombieMovement(StateManager stateManager, ZombieController controller) : base(stateManager)
    {
        _zc = controller;
        _model = controller.Model;
        
    }

    public override void Awake()
    {
        if (_zc.DebugMe) Debug.Log($"Entering {GetType()}");
        _zc.currentState = GetType().ToString();

        _model.IsMoving = true;
        _zc.Path = GetPath(_model.targetData.Position);
    }

    public override void Execute()
    {
        if (_model.IsDead)
        {
            _stateManager.SetState<ZombieDeath>();
            return;
        }


        if (_zc.IsTargetVisible()) // SI LO VEO
        {
            if (!_lastFrameTargetVisible || Vector3.Distance(_zc.Path[_zc.Path.Count - 1], _model.targetData.Position) >
                _distanceRecalculatePoint) // Si se mueve mucho de a donde voy O recien empiezo a verlo.
            {
                _zc.Path = GetPath(_model.targetData.Position);
            }

            if (Vector3.Distance(_zc.Position, _model.targetData.Position) < _model.data.attack.range)//  -- Estoy a distancia de atacar del target
            {
                _stateManager.SetState<ZombieUseAbility>();
                return;
            }
            
            _model.lastKnownPosition = _model.targetData.Position;

            if(Vector3.Distance(_zc.Path[_zc.CurrentIndex], _zc.Position) < _model.data.nodeDetection)//NODE DETECTION
                _zc.CurrentIndex++;

            #region MOVEMENT

            var dir = Vector3.zero;
                
            foreach (var behaviour in _zc._steeringBehaviours)
            {
                var temp = behaviour.GetDirection(_zc.Neighbours);
                dir += temp;
            }

            dir = dir.normalized;

            _zc.transform.position += dir * (_model.MovementSpeed * Time.deltaTime);
            _model.RotationPoint = _zc.Position + dir;

            #endregion

            if(_zc.CurrentIndex < _zc.Path.Count) return;
            
            _zc.Path = GetPath(_model.targetData.Position); // Si llegue al final del camino y lo sigo viendo.
        }
        else
        {
            if (_lastFrameTargetVisible || Vector3.Distance(_zc.Path[_zc.Path.Count - 1], _model.lastKnownPosition) >
                _distanceRecalculatePoint) // Si dejo de verlo o mi end node esta muy lejos de lastKnownPos
            {
                _zc.Path = GetPath(_model.lastKnownPosition);
            }

            

            if(Vector3.Distance(_zc.Path[_zc.CurrentIndex], _zc.Position) < _model.data.nodeDetection)//NODE DETECTION
                _zc.CurrentIndex++;

            #region MOVEMENT

            var dir = Vector3.zero;
                
            foreach (var behaviour in _zc._steeringBehaviours)
            {
                dir += behaviour.GetDirection(_zc.Neighbours);
            }

            dir = dir.normalized;

            _zc.transform.position += dir * (_model.MovementSpeed * Time.deltaTime);
            _model.RotationPoint = _zc.Position + dir;

            #endregion
            
            if(_zc.CurrentIndex < _zc.Path.Count) return;

            _stateManager.SetState<ZombieIdle>(); //Si llegue al final y no lo veo paso a Idle
        }
    }

    public override void Sleep()
    {
        if (_zc.DebugMe) Debug.Log($"Exiting {GetType()}");
        _zc.currentState = "";
        
        _model.IsMoving = false;
    }
    
    private Path GetPath(Vector3 position)
    {
        _zc.CurrentIndex = 0;

        return _zc.Model.pathfinder.GetPathStruct(_zc.Position, position);
    }
}
