using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;

//MyA1-P1

public class PoolAdapter
{
    private Pool _pool;

    private int _currentActiveObjects;

    private List<int> _activeObjectsHash = new List<int>();

    public PoolAdapter(Func<IPoolObject> createFunction, Action<IPoolObject> disableFunction, int initialQuantity)
    {
        _pool = new Pool(createFunction, disableFunction, initialQuantity);
    }

    public T GetObject<T>()
    {
        _currentActiveObjects += 1;
        var poolObject = _pool.AcquireObject();
        
        _activeObjectsHash.Add(poolObject.GetHashCode());
        
        poolObject.OnAcquire();
        return (T) poolObject;
    }

    public void ReturnObject(object obj)
    {
        if (obj is IPoolObject poolObject)
        {
            _currentActiveObjects -= 1;

            if (_activeObjectsHash.Contains(poolObject.GetHashCode()))
            {
                _activeObjectsHash.Remove(poolObject.GetHashCode());
                _pool.ReleaseObject(poolObject);
            }
        }
    }

    public void CurrentActiveObjectsInPool(out int activeAmount)
    {
        activeAmount = _activeObjectsHash.Count;
    }
    
    public int CurrentActiveObjectsInPool()
    {
        return _activeObjectsHash.Count;
    }
}
