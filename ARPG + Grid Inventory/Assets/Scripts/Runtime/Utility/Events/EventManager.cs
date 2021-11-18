using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DoaT
{
    /// <summary>
    /// Hanldes the Subscription and Call of custom events.
    /// </summary>
    public class EventManager : MonoBehaviour
    {
        private static readonly Dictionary<string, Action<object[]>> EventsData = new Dictionary<string, Action<object[]>>();

        public static void Subscribe(string eventName, Action<object[]> action)
        {
            if (EventsData.ContainsKey(eventName))
                EventsData[eventName] += action;
            else
                EventsData.Add(eventName, action);
        }

        public static void Unsubscribe(string eventName, Action<object[]> action)
        {
            if (EventsData.ContainsKey(eventName))
                EventsData[eventName] -= action;
        }

        public static void Raise(string eventName, params object[] parameters)
        {
            if(EventsData.ContainsKey(eventName))
                EventsData[eventName]?.Invoke(parameters);
        }
        
        public static void Clear()
        {
            EventsData.Clear();
        }
    }
}
