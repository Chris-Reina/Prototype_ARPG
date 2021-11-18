using System;
using System.Collections;
using System.Collections.Generic;
using DoaT.AI;
using FSM;
using UnityEngine;

public class MiniBossChase : MonoBaseState
{
    public override event Action OnNeedsReplan;
    private MiniBossModel _m;
    private MiniBossView _v;

    public float distanceRecalculatePoint = 1f;
    public float nodeDetection = 0.4f;
    
    private bool AmDead => _m.Health <= 0;
    private bool CanUseRangeAttack => _m.RangeAttackCooldown <= 0 && _m.Mana > _m.rangeAttackManaCost;
    private bool CanRechargeEnergy => _m.RechargeManaCooldown <= 0;
    private bool AtMeleeRange => _m.DistanceToTarget <= _m.meleeDistance;
    private bool KnowsPlayerPosition => _m.PlayerLocation == PlayerLocation.Known || _m.PlayerLocation == PlayerLocation.Visible;

    public int CurrentIndex
    {
        get => _m.CurrentIndex;
        set => _m.CurrentIndex = value;
    }
    public Path Path
    {
        get => _m.Path;
        set => _m.Path = value;
    }

    private void Awake() 
    {
        _m = GetComponent<MiniBossModel>();
        _v = GetComponent<MiniBossView>();
        
        OnEnter += OnEnterEvent;
        OnExit += OnExitEvent;
    }
    
    private void OnEnterEvent(IState from, IState to)
    {
        Debug.Log("Entering Chase");
        
        _v.isMoving = true;
        Path = GetPath(_m.targetData.Position);
    }
    public override void UpdateLoop()
    {
        Debug.Log(GetType() + " Update");
    
        if (_m.Health <= 0
            || _m.PlayerLocation == PlayerLocation.Unknown
            || (_m.PlayerLocation == PlayerLocation.Visible 
                && _m.RangeAttackCooldown <= 0 
                && _m.Mana >= _m.rangeAttackManaCost)
            || (_m.Mana < _m.rangeAttackManaCost 
                && _m.RechargeManaCooldown <= 0 
                && _m.RangeAttackCooldown <= 0))
        {
            OnNeedsReplan?.Invoke();
            return;
        }

        if (_v.isMoving == false) _v.isMoving = true;

        var pos = _m.targetData.Position;
        var tPos = transform.position;
        
        if (_m.IsTargetVisible()) // SI LO VEO
        {
            if (Vector3.Distance(Path[Path.Count - 1], pos) > distanceRecalculatePoint) // Si se mueve mucho de a donde voy
            {
                Path = GetPath(pos);
            }
            
            _m.targetLastKnownPosition = _m.targetData.Position;

            if(Vector3.Distance(Path[CurrentIndex], tPos) < _m.data.nodeDetection)//NODE DETECTION
                CurrentIndex++;

            #region MOVEMENT

            var dir = Vector3.zero;
                
            foreach (var behaviour in _m.steeringBehaviours)
            {
                var temp = behaviour.GetDirection(_m.Neighbours);
                dir += temp;
            }

            dir = dir.normalized;

            transform.position += dir * (_m.data.movementSpeed * Time.deltaTime);
            _m.RotationPoint = tPos + dir;

            #endregion

            if(CurrentIndex < Path.Count) return;
            
            Path = GetPath(_m.targetData.Position); // Si llegue al final del camino y lo sigo viendo.
        }
        else // Si no lo veo
        {
            if (Vector3.Distance(Path[Path.Count - 1], _m.targetLastKnownPosition) >
                distanceRecalculatePoint) // Si dejo de verlo o mi end node esta muy lejos de lastKnownPos
            {
                Path = GetPath(_m.targetLastKnownPosition);
            }

            if (Path.Count == CurrentIndex)
            {
                Path = GetPath(_m.targetData.Position);
                return;
            }
            
            if(Vector3.Distance(Path[CurrentIndex], tPos) < _m.data.nodeDetection)//NODE DETECTION
                CurrentIndex++;

            #region MOVEMENT

            var dir = Vector3.zero;
                
            foreach (var behaviour in _m.steeringBehaviours)
            {
                dir += behaviour.GetDirection(_m.Neighbours);
            }

            dir = dir.normalized;

            transform.position += dir * (_m.data.movementSpeed * Time.deltaTime);
            _m.RotationPoint = tPos + dir;

            #endregion
        }
        
    }
    
    private void OnExitEvent(IState from, IState to)
    {
        Debug.Log("Exiting Chase");
        _v.isMoving = false;
    }

    public override IState ProcessInput()
    {
        if (KnowsPlayerPosition && Transitions.ContainsKey(MiniBossController.AttackRangeState))
            return Transitions[MiniBossController.AttackRangeState];

        if (KnowsPlayerPosition && AtMeleeRange && Transitions.ContainsKey(MiniBossController.AttackMeleeState))
            return Transitions[MiniBossController.AttackMeleeState];

        return this;
    }

    private Path GetPath(Vector3 position)
    {
        CurrentIndex = 0;
        return _m.pathfinder.GetPathStruct(transform.position, position);
    }
    
}
