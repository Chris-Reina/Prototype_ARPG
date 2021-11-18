using DoaT.AI;
using UnityEngine;

public class PlayerUseAbility : State
{
    private readonly PlayerController _pc;

    private float _timerMax;
    private float _timer = 0;
    private float _animationDelay;

    private bool _effectTriggered;
    private bool _attackTriggered;
    private bool _targetReached;

    private AbilityCommand CurrentComm => (AbilityCommand) _pc.CurrentCommand;
    private Ability _abilityToUse;
    private bool IsMovement => CurrentComm.IsMovement;
    
    private Vector3 _targetPosition;

    public PlayerUseAbility(StateManager stateManager, PlayerController controller) : base(stateManager)
    {
        _pc = controller;
    }

    public override void Awake()
    {
        _pc.currentState = GetType().ToString();
        if (_pc.DebugMe)
            Debug.Log(
                $"Entering {GetType()} with CommandID: {(_pc.CurrentCommand == null ? "Null" : _pc.CurrentCommand.HashID.ToString())}");

        Initialize();
    }

    public override void Execute()
    {
        if (IsMovement)
        {
            MovementAction();
        }
        else
        {
            if(CurrentComm.ForceExecution || _targetReached)
            {
                _pc.Model.IsInAbility = true;
                AbilityInRange();
            }
            else if (!_targetReached)
            {
                _pc.Model.IsInAbility = false;
                AbilityOutOfRange();
            }
        }
    }

    public override void Sleep()
    {
        if (_pc.DebugMe)
            Debug.Log(
                $"Exiting {GetType()} with CommandID: {(_pc.CurrentCommand == null ? "Null" : _pc.CurrentCommand.HashID.ToString())}");
        _pc.Model.AnimationOverrider.ResetController();
        _pc.path = null;
        _pc.Model.IsInAbility = false;
        _pc.Model.IsMoving = false;
    }


    private void Initialize()
    {
        if (IsMovement)
        {
            _abilityToUse = CurrentComm.MovementAbility;
            
            _pc.path = GetPath();
            _pc.Model.AnimationOverrider.ChangeRunAnimation(_abilityToUse.animationClip);
            _pc.Model.IsMoving = true;
            _pc.Model.IsInAbility = false;
        }
        else
        {
            if (CurrentComm.Ability is Attack attack)
            {
                _abilityToUse = _pc.Model.Mana.CanSpend(attack.manaCost) ? attack : CurrentComm.DefaultAbility;
                
                _pc.Model.AnimationAttackSpeed = _abilityToUse.animationSpeedMultiplier;
            }
            
            _timer = 0;
            _timerMax = _abilityToUse.AnimationDuration;
            _animationDelay = _abilityToUse.animationEffectDelay * _abilityToUse.AnimationDuration;
            _effectTriggered = false;
            _attackTriggered = false;
            _pc.path = GetPath();
            
            _targetReached = Vector3.Distance(_pc.Position, CurrentComm.Point) <= _abilityToUse.range;

            if (!CurrentComm.ForceExecution)
                _pc.Model.IsMoving = !_targetReached;
            else
            {
                _pc.Model.IsInAbility = true;
                _pc.Model.IsMoving = false;
            }
            
            _pc.Model.AnimationOverrider.ChangeAttackAnimation(_abilityToUse.animationClip);
            
            _pc.Model.RotationPoint = CurrentComm.Point;
        }
        
        _pc.Model.AnimationMovementSpeed = CurrentComm.MovementAbility.animationSpeedMultiplier;
    }

    private void MovementAction()
    {
        if (_pc.QueuedCommand != null)
        {
            if (_pc.QueuedCommand is InteractCommand)
            {
                _pc.CurrentCommand.Finish();
                _pc.UpdateQueue();
                _stateManager.SetState<PlayerIdle>();
                return;
            }
            
            _pc.CurrentCommand.Finish();
            //if(_pc.DebugMe) Debug.Log("Called UpdateQueue from UseAbility->MovementAction");
            _pc.UpdateQueue();
            _pc.CurrentCommand.Initialize();
            Initialize();
            _pc.path = GetPath();

            return;
        }

        AbilityEffectData.AbilityById[CurrentComm.MovementAbility.ID].Invoke(_abilityToUse, _pc.Model);
            
        if(_pc.currentIndex < _pc.path.Count) return;
            
        _pc.CurrentCommand.Finish();
        //if(_pc.DebugMe) Debug.Log("Called UpdateQueue from UseAbility->MovementAction->Finished");
        _pc.UpdateQueue();
        _stateManager.SetState<PlayerIdle>();
    }
    private void AbilityInRange()
    {
        // if (_pc.QueuedCommand is AbilityCommand abC)
        // {
        //     if (abC.IsMovement)
        //     {
        //         _pc.CurrentCommand.Finish();
        //         _pc.UpdateQueue();
        //         _pc.CurrentCommand.Initialize();
        //         _pc.path = GetPath();
        //     }
        // }
        
        if (!_attackTriggered)
            TriggerAttack();

        _timer += Time.deltaTime;

        if (_timer >= _animationDelay && !_effectTriggered)
        {
            _effectTriggered = true;
            AbilityEffectData.AbilityById[_abilityToUse.ID].Invoke(_abilityToUse, _pc.Model);
            if (_abilityToUse.sound != null) SoundManager.PlaySound(_abilityToUse.sound, _pc.Position, 1f);
            
            if (_abilityToUse is Attack attack)
            {
                _pc.InstantiateParticleSystem(attack.effectSystem);
                _pc.Model.Mana.AddValue(-attack.manaCost);
            }
        }

        if (_timer >= _timerMax)
        {
            _pc.CurrentCommand.Finish();
            //if(_pc.DebugMe) Debug.Log("Called UpdateQueue from UseAbility->AbilityInRange->Finished");
            _pc.UpdateQueue();
            _stateManager.SetState<PlayerIdle>();
            return;
        } 
    }
    private void AbilityOutOfRange()
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
            //if(_pc.DebugMe) Debug.Log("Called UpdateQueue from UseAbility->AbilityOutOfRange->Finished");
            _pc.UpdateQueue();
            _stateManager.SetState<PlayerIdle>();
            return;
        }
                
        if(CurrentComm.Ability)
            _targetReached = Vector3.Distance(_pc.Position, CurrentComm.Point) <= _abilityToUse.range;

        if (_targetReached)
        {
            _pc.Model.IsMoving = false;
            TriggerAttack();
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
    private void TriggerAttack()
    {
        _attackTriggered = true;
        _pc.Model.TriggerAttack?.Invoke();
        if(_pc.DebugMe) Debug.Log("Ability Effect Triggered");
    }
    
}