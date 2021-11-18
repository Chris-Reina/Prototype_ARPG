using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using FSM;
using UnityEngine;

public class MiniBossController : MonoBehaviour
{
    public MiniBossIdle idleState;
    public MiniBossChase chaseState;
    public MiniBossAttackMelee attackMeleeState;
    public MiniBossAttackRange attackRangeState;
    public MiniBossRechargeMana rechargeManaState;
    public MiniBossDead deadState;

    public MiniBossModel model;
    public bool isPlanning = false;
    
    private FiniteStateMachine _fsm;
                
    private float _lastReplanTime;
    private float _replanRate = .25f;

    private const string Idle = "Idle";
    private const string Chase = "Chase";
    private const string AttackMelee = "AttackMelee";
    private const string AttackRange = "AttackRange";
    private const string RechargeMana = "RechargeMana";
    private const string Dead = "Dead";
    
    public static string IdleState => "OnMiniBossIdle";
    public static string ChaseState => "OnMiniBossChase";
    public static string AttackMeleeState => "OnMiniBossAttackMelee";
    public static string AttackRangeState => "OnMiniBossAttackRange";
    public static string RechargeManaState => "OnMiniBossRechargeMana";
    public static string DeadState => "OnMiniBossDead";

    public static Action<string> DEBUG;
    
#region World Variables

    private const string DistanceToTarget = "DistanceToTarget";
    private const string Health = "Health";
    private const string RangeAttackCooldown = "RangeAttackCooldown";
    private const string RechargeManaCooldown = "RechargeManaCooldown";

    private const string IsPlayerAlive = "IsPlayerAlive";

    private const string Mana = "Mana";

    private const string PlayerVisibility = "PlayerVisibility";
    
#endregion
    
    private void Awake()
    {
        model = GetComponent<MiniBossModel>();
    }

    void Start()
    {
        idleState.OnNeedsReplan          += OnReplan;
        chaseState.OnNeedsReplan         += OnReplan;
        attackMeleeState.OnNeedsReplan   += OnReplan;
        attackRangeState.OnNeedsReplan   += OnReplan;
        rechargeManaState.OnNeedsReplan  += OnReplan;
        deadState.OnNeedsReplan          += OnReplan;

        PlanAndExecute();
        DEBUG += Debug.Log;
    }

    // private void Update()
    // {
    //     if (Input.GetKeyDown(KeyCode.N))
    //     {
    //         DEBUGReadPlan();
    //     }
    // }

    private void PlanAndExecute()
    {
        isPlanning = true;
        
        var actions = new List<GOAPAction>
        {
            new GOAPAction(Idle)
                .Effect(PlayerVisibility, x => x.Value = PlayerLocation.Known)
                .LinkedState(idleState),

            new GOAPAction(Chase)
                .Pre(PlayerVisibility, x =>
                {
                    var val = x.GetValue<PlayerLocation>();
                    return val == PlayerLocation.Visible || val == PlayerLocation.Known;
                })
                .Effect(DistanceToTarget,x=>x.Value = model.meleeDistance)
                .Effect(PlayerVisibility, x=> x.Value = PlayerLocation.Visible)
                .LinkedState(chaseState),
                                               
            new GOAPAction(RechargeMana)
                .Pre(RechargeManaCooldown, x => x.GetValue<float>() <= 0)
                .Pre(Mana,   x => x.GetValue<int>() < model.rangeAttackManaCost)
                .Effect(Mana, x => x.Value = model.maxMana)
                .LinkedState(rechargeManaState),
                
            new GOAPAction(AttackMelee)
                .Pre(PlayerVisibility, x => x.GetValue<PlayerLocation>() == PlayerLocation.Visible)
                .Pre(DistanceToTarget,   x => x.GetValue<float>() <= model.meleeDistance)
                .Effect(IsPlayerAlive, x => x.Value = false)
                .Cost(1 + 10/model.meleeAttackDamage)
                .LinkedState(attackMeleeState),
                                               
            new GOAPAction(AttackRange)
                .Pre(PlayerVisibility, x => x.GetValue<PlayerLocation>() == PlayerLocation.Visible)
                .Pre(RangeAttackCooldown,   x => x.GetValue<float>() <= 0)
                .Pre(Mana,   x => x.GetValue<int>() >= model.rangeAttackManaCost)
                .Effect(IsPlayerAlive, x => x.Value = false)
                .Cost(1 + 10/model.rangeAttackDamage)
                .LinkedState(attackRangeState),

            new GOAPAction(Dead)
                .Pre(Health, x => x.GetValue<float>() <= 0)
                .Effect(IsPlayerAlive, x => x.Value = false)
                .LinkedState(deadState)
        };
             
        var from = new GOAPState();
        from.values[Mana] = new IntWorldVariable(Mana, model.Mana);
        from.values[Health] = new FloatWorldVariable(Health, model.Health);
        from.values[PlayerVisibility] = new EnumWorldVariable<PlayerLocation>(PlayerVisibility, model.PlayerLocation);
        from.values[IsPlayerAlive] = new BoolWorldVariable(IsPlayerAlive, model.IsPlayerAlive);
        from.values[DistanceToTarget] = new FloatWorldVariable(DistanceToTarget, model.DistanceToTarget);
        from.values[RangeAttackCooldown] = new FloatWorldVariable(RangeAttackCooldown, model.RangeAttackCooldown);
        from.values[RechargeManaCooldown] = new FloatWorldVariable(RechargeManaCooldown, model.RechargeManaCooldown);
               
        var to = new GOAPState();
        to.values[IsPlayerAlive] = new BoolWorldVariable(IsPlayerAlive, false);
                
        var planner = new GoapPlanner();
                
        planner.OnRunComplete += ConfigureFsm;
        planner.Run(from, to, actions, StartCoroutine);
    }

    private void DEBUGReadPlan()
    {
        var actions = new List<GOAPAction>
        {
            new GOAPAction(Idle)
                .Effect(PlayerVisibility, x => x.Value = PlayerLocation.Known )
                .LinkedState(idleState),

            new GOAPAction(Chase)
                .Pre(PlayerVisibility, x =>
                {
                    Debug.Log($"value: {x.Value}, Cond: val == PlayerLocation.Visible || val == PlayerLocation.Known;");
                    
                    var val = x.GetValue<PlayerLocation>();
                    return val == PlayerLocation.Visible || val == PlayerLocation.Known;
                })
                .Effect(DistanceToTarget,x=>x.Value = model.meleeDistance)
                .Effect(PlayerVisibility, x=> x.Value = PlayerLocation.Visible)
                .LinkedState(chaseState),
                                               
            new GOAPAction(RechargeMana)
                .Pre(RechargeManaCooldown, x =>
                {
                    Debug.Log($"value: {x.Value}, Cond: x.GetValue<float>() <= 0;");
                    return x.GetValue<float>() <= 0;
                })
                .Pre(Mana,   x =>
                {
                    Debug.Log($"value: {x.Value}, Cond: x.GetValue<int>() < model.rangeAttackManaCost;");
                    return x.GetValue<int>() < model.rangeAttackManaCost;
                })
                .Effect(Mana, x => x.Value = model.maxMana)
                .LinkedState(rechargeManaState),
                
            new GOAPAction(AttackMelee)
                .Pre(PlayerVisibility, x =>
                {
                    Debug.Log($"value: {x.Value}, Cond: x.GetValue<PlayerLocation>() == PlayerLocation.Visible;"); //
                    return x.GetValue<PlayerLocation>() == PlayerLocation.Visible;
                })
                .Pre(DistanceToTarget,   x =>
                {
                    Debug.Log($"value: {x.Value}, Cond: x.GetValue<float>() <= model.meleeDistance;");
                    return x.GetValue<float>() <= model.meleeDistance;
                })
                .Effect(IsPlayerAlive, x => x.Value = false)
                .Cost(1 + 10/model.meleeAttackDamage)
                .LinkedState(attackMeleeState),
                                               
            new GOAPAction(AttackRange)
                .Pre(PlayerVisibility, x =>
                {
                    Debug.Log($"value: {x.Value}, Cond: x.GetValue<PlayerLocation>() == PlayerLocation.Visible;");
                    return x.GetValue<PlayerLocation>() == PlayerLocation.Visible;
                })
                .Pre(RangeAttackCooldown,   x =>
                {
                    Debug.Log($"value: {x.Value}, x.GetValue<float>() <= 0;");
                    return x.GetValue<float>() <= 0;
                })
                .Pre(Mana,   x =>
                {
                    Debug.Log($"value: {x.Value}, Cond: x.GetValue<int>() >= model.rangeAttackManaCost;");
                    return x.GetValue<int>() >= model.rangeAttackManaCost;
                })
                .Effect(IsPlayerAlive, x => x.Value = false)
                .Cost(1 + 10/model.rangeAttackDamage)
                .LinkedState(attackRangeState),

            new GOAPAction(Dead)
                .Pre(Health, x =>
                {
                    Debug.Log($"value: {x.Value}, Cond: x.GetValue<float>() <= 0;");
                    return x.GetValue<float>() <= 0;
                })
                .Effect(IsPlayerAlive, x => x.Value = false)
                .LinkedState(deadState)
        };
             
        var from = new GOAPState();
        from.values[Mana] = new IntWorldVariable(Mana, model.Mana);
        from.values[Health] = new FloatWorldVariable(Health, model.Health);
        from.values[PlayerVisibility] = new EnumWorldVariable<PlayerLocation>(PlayerVisibility, PlayerLocation.Unknown);
        from.values[IsPlayerAlive] = new BoolWorldVariable(IsPlayerAlive, model.IsPlayerAlive);
        from.values[DistanceToTarget] = new FloatWorldVariable(DistanceToTarget, model.DistanceToTarget);
        from.values[RangeAttackCooldown] = new FloatWorldVariable(RangeAttackCooldown, model.RangeAttackCooldown);
        from.values[RechargeManaCooldown] = new FloatWorldVariable(RechargeManaCooldown, model.RechargeManaCooldown);
               
        var to = new GOAPState();
        to.values[IsPlayerAlive] = new BoolWorldVariable(IsPlayerAlive, false);
                
        var planner = new GoapPlanner();
        planner.OnRunComplete += DEBUGPrintPlan;
        planner.Run(from, to, actions, StartCoroutine);
    }
    
    private void OnReplan()
    {
        if (Time.time > _lastReplanTime + _replanRate && !isPlanning)
        {
            Debug.Log("Replan Accepted");
            _lastReplanTime = Time.time;
            isPlanning = true;
        }
        else
        {
            if (!isPlanning)
            {
                Debug.Log("Replan Denied By Time");
                Debug.Log(Time.time - (_lastReplanTime + _replanRate));
            }
            return;
        }

        _fsm.Active = false;
        Debug.Log("Exiting state '" + _fsm.CurrentState.Name + "'.");
        _fsm.CurrentState.Exit(null);
        _fsm = null;
        
        Debug.Log("model.RechargeManaCooldown = " + model.RechargeManaCooldown);
        Debug.Log("model.RangeAttackCooldown= " + model.RangeAttackCooldown);
        
        var actions = new List<GOAPAction>
        {
            new GOAPAction(Idle)
                .Effect(PlayerVisibility, x => x.Value = PlayerLocation.Known)
                .LinkedState(idleState),

            new GOAPAction(Chase)
                .Pre(PlayerVisibility, x =>
                {
                    var val = x.GetValue<PlayerLocation>();
                    return val == PlayerLocation.Visible || val == PlayerLocation.Known;
                })
                .Effect(DistanceToTarget,x=>x.Value = model.meleeDistance)
                .Effect(PlayerVisibility, x=> x.Value = PlayerLocation.Visible)
                .LinkedState(chaseState),
                                               
            new GOAPAction(RechargeMana)
                .Pre(RechargeManaCooldown, x => x.GetValue<float>() <= 0)
                .Pre(Mana,   x => x.GetValue<int>() < model.rangeAttackManaCost)
                .Effect(Mana, x => x.Value = model.maxMana)
                .LinkedState(rechargeManaState),
                
            new GOAPAction(AttackMelee)
                .Pre(PlayerVisibility, x => x.GetValue<PlayerLocation>() == PlayerLocation.Visible)
                .Pre(DistanceToTarget,   x => x.GetValue<float>() <= model.meleeDistance)
                .Effect(IsPlayerAlive, x => x.Value = false)
                .Cost(1 + 10/model.meleeAttackDamage)
                .LinkedState(attackMeleeState),
                                               
            new GOAPAction(AttackRange)
                .Pre(PlayerVisibility, x => x.GetValue<PlayerLocation>() == PlayerLocation.Visible)
                .Pre(RangeAttackCooldown,   x => x.GetValue<float>() <= 0)
                .Pre(Mana,   x => x.GetValue<int>() >= model.rangeAttackManaCost)
                .Effect(IsPlayerAlive, x => x.Value = false)
                .Cost(1 + 10/model.rangeAttackDamage)
                .LinkedState(attackRangeState),

            new GOAPAction(Dead)
                .Pre(Health, x => x.GetValue<float>() <= 0)
                .Effect(IsPlayerAlive, x => x.Value = false)
                .LinkedState(deadState)
        };
             
        var from = new GOAPState();
        from.values[Mana] = new IntWorldVariable(Mana, model.Mana);
        from.values[Health] = new FloatWorldVariable(Health, model.Health);
        from.values[PlayerVisibility] = new EnumWorldVariable<PlayerLocation>(PlayerVisibility, model.PlayerLocation);
        from.values[IsPlayerAlive] = new BoolWorldVariable(IsPlayerAlive, model.IsPlayerAlive);
        from.values[DistanceToTarget] = new FloatWorldVariable(DistanceToTarget, model.DistanceToTarget);
        from.values[RangeAttackCooldown] = new FloatWorldVariable(RangeAttackCooldown, model.RangeAttackCooldown);
        from.values[RechargeManaCooldown] = new FloatWorldVariable(RechargeManaCooldown, model.RechargeManaCooldown);
               
        var to = new GOAPState();
        to.values[IsPlayerAlive] = new BoolWorldVariable(IsPlayerAlive, false);
                
        var planner = new GoapPlanner();
                
        planner.OnRunComplete += ConfigureFsm;
        planner.Run(from, to, actions, StartCoroutine);
    }

    private void DEBUGPrintPlan(IEnumerable<GOAPAction> plan)
    {
        Debug.LogWarning("plan Finished");
        
        var result = plan.Aggregate("",
            (current, item) => current + (item.linkedState.GetType().ToString() + " -> "));

        if (result.Length > 5)
            result = result.Remove(result.Length - 5, 4);

        Debug.LogWarning(result);
    }
    
    private void ConfigureFsm(IEnumerable<GOAPAction> plan)
    {
        isPlanning = false;
        _fsm = GoapPlanner.ConfigureFSM(plan, StartCoroutine);
        _fsm.Active = true;
    }
}
