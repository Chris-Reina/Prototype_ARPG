using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Manages Updates from subscribed objects.
/// </summary>
[DisallowMultipleComponent]
public class UpdateManager : MonoBehaviour
{
    public static UpdateManager Instance { get; private set; }

    private readonly List<IUpdate> _allUpdates = new List<IUpdate>();

    private readonly List<IFixedUpdate> _allFixedUpdates = new List<IFixedUpdate>();

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
        {
            Destroy(this);
        }
    }

    private void Update()
    {
        for (int i = 0; i < _allUpdates.Count; i++)
        {
            if (_allUpdates[i] == null)
            {
                RemoveUpdateFromManager(_allUpdates[i]);
                continue;
            }
            
            _allUpdates[i].OnUpdate();
        }
    }

    private void FixedUpdate()
    {
        for (int i = 0; i < _allFixedUpdates.Count; i++)
        {
            if (_allFixedUpdates[i] == null) 
            {
                RemoveFixedUpdateFromManager(_allFixedUpdates[i]);
                continue;
            }

            _allFixedUpdates[i].OnFixedUpdate();
        }
    }

    public void AddUpdateToManager(IUpdate update)
    {
        if (!_allUpdates.Contains(update))
            _allUpdates.Add(update);
    }
    public void RemoveUpdateFromManager(IUpdate update)
    {
        if (_allUpdates.Contains(update))
            _allUpdates.Remove(update);
    }
    public void AddFixedUpdateToManager(IFixedUpdate fixedUpdate)
    {
        if (!_allFixedUpdates.Contains(fixedUpdate))
            _allFixedUpdates.Add(fixedUpdate);
    }
    public void RemoveFixedUpdateFromManager(IFixedUpdate fixedUpdate)
    {
        if (_allFixedUpdates.Contains(fixedUpdate))
            _allFixedUpdates.Remove(fixedUpdate);
    }
    

    public void ClearUpdates()
    {
        _allUpdates.Clear();
    }
    public void ClearFixedUpdates()
    {
        _allFixedUpdates.Clear();
    }


    public static void AddUpdate(IUpdate update)
    {
        Instance.AddUpdateToManager(update);
    }
    public static void RemoveUpdate(IUpdate update)
    {
        Instance.RemoveUpdateFromManager(update);
    }
    public static void AddFixedUpdate(IFixedUpdate fixedUpdate)
    {
        Instance.AddFixedUpdateToManager(fixedUpdate);
    }
    public static void RemoveFixedUpdate(IFixedUpdate fixedUpdate)
    {
        Instance.RemoveFixedUpdateFromManager(fixedUpdate);
    }
}
