﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using FSM;
using UnityEngine;

public class GoapPlanner
{
    private const int _WATCHDOG_MAX = 200;

    private int _watchdog;
    public event Action<IEnumerable<GOAPAction>> OnRunComplete;

    public void Run(GOAPState from, GOAPState to, IEnumerable<GOAPAction> actions, Func<IEnumerator, Coroutine> startCoroutine) {
        _watchdog = _WATCHDOG_MAX;

        var aStar = new AStar<GOAPState>(1/2000f);

        aStar.OnPathCompleted += ReturnPath;

        startCoroutine?.Invoke(aStar.Run(from,
            state => Satisfies(state, to),
            node => Explode(node, actions, ref _watchdog),
            state => GetHeuristic(state, to)));
    }
    
    private void ReturnPath(IEnumerable<GOAPState> collection)
    {
        OnRunComplete?.Invoke(CalculateGoap(collection));
    }

    public static FiniteStateMachine ConfigureFSM(IEnumerable<GOAPAction> plan, Func<IEnumerator, Coroutine> startCoroutine){
        var prevState = plan.First().linkedState;
            
        var fsm = new FiniteStateMachine(prevState, startCoroutine);

        foreach (var action in plan.Skip(1)){
            if (prevState == action.linkedState) continue;
            fsm.AddTransition("On" + action.linkedState.Name, prevState, action.linkedState);
                
            prevState = action.linkedState;
        }

        return fsm;
    }
    
    public static FiniteStateMachine ConfigureFSM(IEnumerable<GOAPAction> plan, Func<IEnumerator, Coroutine> startCoroutine, FiniteStateMachine existingFsm)
    {
        var prevState = plan.First().linkedState;

        //existingFsm.ConfigureExistingFSM(prevState);

        foreach (var action in plan.Skip(1)){
            if (prevState == action.linkedState) continue;
            existingFsm.AddTransition("On" + action.linkedState.Name, prevState, action.linkedState);
                
            prevState = action.linkedState;
        }

        return existingFsm;
    }

    private IEnumerable<GOAPAction> CalculateGoap(IEnumerable<GOAPState> sequence) {
        foreach (var act in sequence.Skip(1)) {
            Debug.Log(act);
        }

        Debug.Log("WATCHDOG " + _watchdog);

        return sequence.Skip(1).Select(x => x.generatingAction);
    }

    private static float GetHeuristic(GOAPState from,  GOAPState goal) => goal.values.Count(kv => !kv.In(from.values));
    private static bool  Satisfies(GOAPState    state, GOAPState to)   => to.values.All(kv => kv.In(state.values));

    private static IEnumerable<WeightedNode<GOAPState>> Explode(GOAPState node, IEnumerable<GOAPAction> actions,
                                                                ref int   watchdog) {
        if (watchdog == 0) return Enumerable.Empty<WeightedNode<GOAPState>>();
        watchdog--;

        return actions.Where(action => action.preconditions.All(kv => kv.In(node.values)))
                      .Aggregate(new List<WeightedNode<GOAPState>>(), (possibleList, action) => {
                           var newState = new GOAPState(node);
                           newState.values.UpdateWith(action.effects);
                           newState.generatingAction = action;
                           newState.step             = node.step + 1;

                           possibleList.Add(new WeightedNode<GOAPState>(newState, action.cost));
                           return possibleList;
                       });
    }

    
}
