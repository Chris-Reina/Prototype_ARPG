using System;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;

namespace DoaT.Scheduler
{
    [DisallowMultipleComponent]
    public class Scheduler : MonoBehaviour, IUpdate
    {
        private static List<SchedulerTask> _tasks = new List<SchedulerTask>();

        public void OnUpdate()
        {
        }
        
        public static SchedulerTask AddTask(float time, Action action)
        {
            var tempTask = new SchedulerTask(time, action);
            _tasks.Add(tempTask);
            return tempTask;
        }

        public static void CancelTask(ref SchedulerTask task)
        {
            if (_tasks.Contains(task))
                _tasks.Remove(task);

            task = null;
        }

        private static void RemoveDoneTask(SchedulerTask task)
        {
            task.OnTaskCompleted -= RemoveDoneTask;
            _tasks.Remove(task);
        }
    }
}

