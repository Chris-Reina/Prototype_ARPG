using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace DoaT
{
    public class TimerManager : MonoBehaviour
    {
        public static TimerManager Instance { get; private set; }

        private Dictionary<TimerHandler, Action> _timers = new Dictionary<TimerHandler, Action>();

        private void Awake()
        {
            if (Instance == null) Instance = this;
            else if (Instance != this) Destroy(this);
        }

        private void LateUpdate()
        {
            if (_timers.Count == 0) return;

            foreach (var timer in from timer in _timers
                let isDone = timer.Key.ElapseTime(Time.deltaTime)
                where isDone
                select timer)
            {
                timer.Key.IsActive = false;
                timer.Value?.Invoke();
            }

            _timers = _timers.Where(x => x.Key.IsActive)
                .ToDictionary(x => x.Key, x => x.Value);
        }

        private void InternalSetTimer(TimerHandler handler, Action callback, float duration, float delay)
        {
            if (_timers.ContainsKey(handler))
                _timers[handler] = callback;
            else
                _timers.Add(handler, callback);


            handler.Setup(duration, delay);
        }

        /// <summary>
        /// Sets a timer that will execute the given Action when done.
        /// </summary>
        /// <param name="handler">Handler to take track of the timer.</param>
        /// <param name="callback">Action to be executed when timer's done.</param>
        /// <param name="duration">Duration of the timer.</param>
        /// <param name="delay">Optional parameter. Wait time before starting the Timer.</param>
        public static void SetTimer(TimerHandler handler, Action callback, float duration, float delay)
        {
            if (handler == null || callback == null) return;
            if (Instance == null)
            {
                Debug.LogWarning("Missing an Instance of TimerManager.");
                return;
            }

            Instance.InternalSetTimer(handler, callback, duration, delay);
        }
    }
}
