using UnityEngine;

namespace DoaT
{
    [System.Serializable]
    public class TimerHandler
    {
        [SerializeField] private float _duration = 1f;
        [SerializeField] private float _elapsed = 0f;

        public bool IsActive { get; set; }
        public float Duration => _duration;
        public float Elapsed => _elapsed < 0f ? 0f : _elapsed;
        public float Remaining => _duration - Elapsed;
        public float Completion
        {
            get
            {
                var value = _elapsed / _duration;
                if (value < 0f)
                    value = 0f;
                else if (value > 1f)
                    value = 1f;

                return value;
            }
        }
        public bool IsDone => _elapsed >= _duration;

        public TimerHandler() { }

        public void Setup(float duration, float delay)
        {
            IsActive = true;
            _duration = duration;
            _elapsed = 0 - delay;
        }

        public void AddDelay(float delay)
        {
            _elapsed -= delay;
        }

        public bool ElapseTime(float deltaTime)
        {
            if (IsDone) return true;

            _elapsed += deltaTime;
            return IsDone;
        }
    }
}

