
using System;
using Unity.Collections.LowLevel.Unsafe;

namespace DoaT.Scheduler
{
    public class SchedulerTask
    {
        private float _time;
        private Action _action;

        public event Action<SchedulerTask> OnTaskCompleted;

        public float RemainingTime => _time;

        public SchedulerTask(float time, Action action)
        {
            _time = time;
            _action = action;
        }

        private void OnElapseTime(float deltaTime)
        {
            _time -= deltaTime;
            
            if(_time <= 0)
                ExecuteTask();
        }

        private void ExecuteTask()
        {
            _action?.Invoke();
            OnTaskCompleted?.Invoke(this);
        }

    }
}

