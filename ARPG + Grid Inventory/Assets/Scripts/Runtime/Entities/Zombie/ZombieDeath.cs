
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DoaT;
using DoaT.AI;
using UnityEngine;

public class ZombieDeath : State
{
    private readonly ZombieController _zc;
    private readonly ZombieModel _model;

    private List<Item> _items;
    private float _timer = 0;
    private float _timerMax = 0.2f;

    public ZombieDeath(StateManager stateManager, ZombieController controller) : base(stateManager)
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

    /*public class Algo<T> : IEnumerable
    {
        private IEnumerable<T> _internal;
        
        public Algo()
        {
            _internal = Enumerable.Empty<T>();
        }

        public Algo(IEnumerable<T> collection)
        {
            _internal = collection;
        }

        public void Enqueue(T item)
        {
            var collection = new T[] {item};
            _internal = collection.Concat(_internal);
        }

        public T Dequeue()
        {
            if (!_internal.Any()) return default;
            var element = _internal.First();
            _internal = _internal.Skip(1);
            return element;
        }

        public T Peek()
        {
            return _internal.FirstOrDefault();
        }

        public int Count => _internal.Count();

        public IEnumerator<T> GetEnumerator()
        { 
            return _internal.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }*/
}
