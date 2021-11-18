using System.Collections;
using System.Collections.Generic;
using DoaT.AI;
using UnityEngine;

public class PlayerInteract : State
{
    private readonly PlayerController _pc;

    private bool _targetReached;

    private InteractCommand CurrentComm { get; set; }
    
    private Vector3 _targetPosition;

    public PlayerInteract(StateManager stateManager, PlayerController controller) : base(stateManager)
    {
        _pc = controller;
    }

    public override void Awake()
    {
        _pc.currentState = GetType().ToString();
        CurrentComm = (InteractCommand) _pc.CurrentCommand;
        if (_pc.DebugMe)
            Debug.Log(
                $"Entering {GetType()} with CommandID: {(_pc.CurrentCommand == null ? "Null" : _pc.CurrentCommand.HashID.ToString())}");
        Initialize();
    }

    public override void Execute()
    {
        if(CurrentComm.ForceExecution || _targetReached)
            InteractableInRange();
        else if (!_targetReached)
            InteractableOutOfRange();
    }

    public override void Sleep()
    {
        if (_pc.DebugMe)
            Debug.Log(
                $"Exiting {GetType()} with CommandID: {(_pc.CurrentCommand == null ? "Null" : _pc.CurrentCommand.HashID.ToString())}");
        
        _pc.path = null;
        _pc.Model.IsMoving = false;
        CurrentComm = null;
    }


    private void Initialize()
    {
        _pc.path = GetPath();
            
        _targetReached = Vector3.Distance(_pc.Position, CurrentComm.Point) <= _pc.Model.interactRange;
            
        if(!CurrentComm.ForceExecution)
            _pc.Model.IsMoving = !_targetReached;
        else
        {
            _pc.Model.IsMoving = false;
        }
            
        _pc.Model.RotationPoint = CurrentComm.Point;
        
    }

    private void InteractableInRange()
    {
        CurrentComm.Interactable.Interact();
        
        _pc.CurrentCommand.Finish();
        _pc.UpdateQueue();
        _stateManager.SetState<PlayerIdle>();
    }
    private void InteractableOutOfRange()
    {
        if (_pc.QueuedCommand != null)
        {
            _pc.CurrentCommand.Finish();
            //if(_pc.DebugMe) Debug.Log("Called UpdateQueue from UseAbility->AbilityOutOfRange");
            _pc.UpdateQueue();
            _pc.CurrentCommand.Initialize();
            _pc.path = GetPath();
        }

        AbilityEffectData.AbilityById[CurrentComm.MovementAbility.ID].Invoke(CurrentComm.MovementAbility, _pc.Model);

        if (_pc.currentIndex >= _pc.path.Count)
        {
            _pc.CurrentCommand.Finish();
            _pc.UpdateQueue();
            _stateManager.SetState<PlayerIdle>();
            return;
        }

        _targetReached = Vector3.Distance(_pc.Position, CurrentComm.Point) <= _pc.Model.interactRange;

        if (_targetReached)
        {
            _pc.Model.IsMoving = false;
        }
    }

    
    private Path GetPath()
    {
        _pc.currentIndex = 0;
        _targetPosition = CurrentComm.Point;

        return CurrentComm.FullMovement
            ? _pc.Model.Pathfinder.GetPathStruct(_pc.Position, _targetPosition)
            : _pc.Model.Pathfinder.GetPathStruct(_pc.Position, _targetPosition, _pc.Model.shortPathStepAmount);
    }
}
