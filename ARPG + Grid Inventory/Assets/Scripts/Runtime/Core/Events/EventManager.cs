using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class EventManager
{
    public delegate void EventCallback(params object[] parameters);
    
    private static Dictionary<string, EventCallback> _eventsData = new Dictionary<string, EventCallback>();


    public static void Subscribe(string eventName, EventCallback action)
    {
        if (_eventsData.ContainsKey(eventName))
        {
            _eventsData[eventName] += action;
        }
        else
        {
            _eventsData.Add(eventName, action);
        }
    }

    public static void Unsubscribe(string eventName, EventCallback action)
    {
        if (_eventsData.ContainsKey(eventName))
        {
            _eventsData[eventName] -= action;
        }
    }

    public static void Trigger(string eventName, params object[] parameters)
    {
        if(_eventsData.ContainsKey(eventName))
            _eventsData[eventName](parameters);
    }

    public static void Trigger(string eventName)
    {
        Trigger(eventName, new object[] { });
    }

    public static void ClearEvents()
    {
        _eventsData = new Dictionary<string, EventCallback>();
    }
}