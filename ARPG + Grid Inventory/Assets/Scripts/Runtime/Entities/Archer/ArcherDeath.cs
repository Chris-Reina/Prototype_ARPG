using System.Collections;
using System.Collections.Generic;
using DoaT;
using DoaT.AI;
using UnityEngine;

public class ArcherDeath : State
{
    private readonly ArcherController _zc;
    private readonly ArcherModel _model;

    private List<Item> _items;
    private float _timer = 0;
    private float _timerMax = 0.2f;

    public ArcherDeath(StateManager stateManager, ArcherController controller) : base(stateManager)
    {
        _zc = controller;
        _model = controller.Model;
    }

    public override void Awake()
    {
        if (_zc.DebugMe) Debug.Log($"Entering {GetType()}");
        _zc.currentState = GetType().ToString();
        EventManager.Trigger(EventsData.OnEntityKilled);
        _items = _model.lootTable.DropItems();
    }

    public override void Execute()
    {
        _model.OnDeath?.Invoke();
        if (_model.Dissolve)
        {
            _zc.Despawn();
            return;
        }
        
        if (_items.Count == 0)
        {
            _model.Dissolve = true;
            return;
        }

        if (_timer > 0)
        {
            _timer -= Time.deltaTime;
            return;
        }

        EventManager.Trigger(EventsData.OnWorldLootSpawn, _zc.Position, _items[0]);
        _items.RemoveAt(0);
        _timer = _timerMax;
    }

    public override void Sleep()
    {
        if (_zc.DebugMe) Debug.Log($"Exiting {GetType()}");
        _zc.currentState = "";
    }
}
