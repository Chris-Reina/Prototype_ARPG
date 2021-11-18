using System;
using System.Collections;
using System.Collections.Generic;
using DoaT;
using FSM;
using UnityEngine;

public class MiniBossDead : MonoBaseState
{   
    public override event Action OnNeedsReplan;
    private MiniBossModel _m;
    private MiniBossView _v;

    private bool PorLasDudasBool = false;
    
    private List<Item> _items;
    private float _timer = 0;
    private float _timerMax = 0.2f;

    private void Awake() 
    {
        _m = GetComponent<MiniBossModel>();
        _v = GetComponent<MiniBossView>();
        
        OnEnter += OnEnterEvent;
        
        OnNeedsReplan += () => OnEnterEvent(null, null);
    }

    private void OnEnterEvent(IState from, IState to)
    {
        Debug.Log("Entering RechargeMana");
        EventManager.Trigger(EventsData.OnEntityKilled);
        _items = _m.lootTable.DropItems();
        _v.dead = true;
    }
    
    public override void UpdateLoop()
    {
        Debug.Log(GetType() + " Update");
    
        if (PorLasDudasBool) return;
        
        _m.OnDeath?.Invoke();
        if (_m.Dissolve)
        {
            _m.Despawn();
            return;
        }
        
        if (_items.Count == 0)
        {
            _m.Dissolve = true;
            return;
        }

        if (_timer > 0)
        {
            _timer -= Time.deltaTime;
            return;
        }

        EventManager.Trigger(EventsData.OnWorldLootSpawn, _m.Position, _items[0]);
        _items.RemoveAt(0);
        _timer = _timerMax;
        PorLasDudasBool = true;
        FSM.Active = false;
    }

    public override IState ProcessInput()
    {
        return this;
    }
}
