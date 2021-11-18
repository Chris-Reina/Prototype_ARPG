
using System.Collections;
using System.Collections.Generic;
using DoaT.AI;
using UnityEngine;

public class PlayerStandUp : State
{
    private readonly PlayerController _pc;
    private float _duration;
    private static readonly int StandUpSpeedMultiplier = Animator.StringToHash("StandUpSpeedMultiplier");

    public PlayerStandUp(StateManager stateManager, PlayerController controller) : base(stateManager)
    {
        _pc = controller;
    }

    public override void Awake()
    {
        if (_pc.DebugMe)
            Debug.Log(
                $"Entering {GetType()} with CommandID: {(_pc.CurrentCommand == null ? "Null" : _pc.CurrentCommand.HashID.ToString())}");
        
        _pc.currentState = GetType().ToString();

        _duration = _pc.Model.AnimationOverrider.baseStandUp.length / _pc.Model.Animator.GetFloat(StandUpSpeedMultiplier);
    }

    public override void Execute()
    {
        if (!_pc.Model.ShouldStandUp)
        {
            _stateManager.SetState<PlayerIdle>();
            return;
        }
        
        if (_duration > 0)
        {
            _duration -= Time.deltaTime;
            return;
        }
        
        _stateManager.SetState<PlayerIdle>();
    }

    public override void Sleep()
    {
        if (_pc.DebugMe)
            Debug.Log(
                $"Exiting {GetType()} with CommandID: {(_pc.CurrentCommand == null ? "Null" : _pc.CurrentCommand.HashID.ToString())}");

        _pc.CleanCommands();
    }
}
