
using System;
using DoaT.AI;
using UnityEngine;

public class PlayerIdle : State
{
    private readonly PlayerController _pc;

    public PlayerIdle(StateManager stateManager, PlayerController controller) : base(stateManager)
    {
        _pc = controller;
    }

    public override void Awake()
    {
        if (_pc.DebugMe)
            Debug.Log(
                $"Entering {GetType()} with CommandID: {(_pc.CurrentCommand == null ? "Null" : _pc.CurrentCommand.HashID.ToString())}");
        
        _pc.currentState = GetType().ToString();
    }

    public override void Execute()
    {
        if (_pc.CurrentCommand != null)
        {
            RunCommand(_pc.CurrentCommand);
        }
        
        if (_pc.QueuedCommand != null)
        {
            //if(_pc.DebugMe) Debug.Log("Called UpdateQueue from Idle->Execute");
            _pc.UpdateQueue();
        }
    }

    public override void Sleep()
    {
        if (_pc.DebugMe)
            Debug.Log(
                $"Exiting {GetType()} with CommandID: {(_pc.CurrentCommand == null ? "Null" : _pc.CurrentCommand.HashID.ToString())}");
    }

    private void RunCommand(CharacterCommand command)
    {
        if (command is AbilityCommand abilityCommand)
        {
            if (abilityCommand.Ability == null && abilityCommand.MovementAbility == null)
            {
                command.Initialize();
                command.Finish();
                //if(_pc.DebugMe) Debug.Log("Called UpdateQueue from Idle->RunCommand");
                _pc.UpdateQueue();
                return;
            }
            
            command.Initialize();
            
            _stateManager.SetState<PlayerUseAbility>();
        }
        else if (command is InteractCommand iCommand)
        {
            command.Initialize();
            
            _stateManager.SetState<PlayerInteract>();
        }
    }
}
