using System;
using System.Collections.Generic;
using System.Linq;
using DoaT;

public class Pool<T> : IPool<T> where T : IPoolSpawn
{
    private readonly T _objectToPool;
    private readonly bool _isDynamic;
    private readonly Func<object, T> _factory;
    private readonly List<PoolObject<T>> _internal;
    

    public List<T> CurrentActiveObjects => _internal.Where(x => !x.IsAvailable)
                                                    .Select(x => x.Object)
                                                    .ToList();
    
    public List<T> CurrentInactiveObjects => _internal.Where(x => x.IsAvailable)
                                                      .Select(x => x.Object)
                                                      .ToList();

    public Pool(T objectToPool, int initialStock, Func<object, T> factory, bool isDynamic)
    {
        _objectToPool = objectToPool;
        _factory = factory;
        _isDynamic = isDynamic;
        
        _internal = new List<PoolObject<T>>();

        if (initialStock <= 0) return;
        
        for (var i = 0; i < initialStock; i++)
        {
            var newPoolObj = new PoolObject<T>(_factory(_objectToPool)) { IsAvailable = true };
            newPoolObj.Object.SetParentPool(this);
            _internal.Add(newPoolObj);
        }
    }

    public T GetObjectFromPool()
    {
        if (_internal.Any(x => x.IsAvailable))
        {
            return _internal.First(x => x.IsAvailable).GetObject();
        }
        
        if (!_isDynamic) return default;
        
        var newPoolObj = new PoolObject<T>(_factory(_objectToPool)) { IsAvailable = false };
        newPoolObj.Object.SetParentPool(this);
        _internal.Add(newPoolObj);

        return newPoolObj.GetObject();
    }

    public void ReturnObjectToPool(T obj)
    {
        var temp = _internal.First(x => Equals(x.Object, obj));
        if(temp != null) temp.IsAvailable = true;
    }
}
