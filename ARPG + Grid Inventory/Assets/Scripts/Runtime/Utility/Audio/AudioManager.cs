using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DoaT
{
    public class AudioManager : MonoBehaviour
    {
        //private static Pool<AudioDurationTracker> Pool => Instance._audioSourcePool;
        private static AudioManager Instance { get; set; }

        [SerializeField] private Pool<AudioDurationTracker> _audioSourcePool;
        [SerializeField] private AudioDurationTracker audioTrackerPrefab = default;

        private void Awake()
        {
            if (Instance == null) Instance = this;
            else if (Instance != this) Destroy(this);

            AudioDurationTracker Factory(object x) => Instantiate((AudioDurationTracker) x);
            _audioSourcePool = new Pool<AudioDurationTracker>(audioTrackerPrefab, 5, Factory, true);
        }

        public static void PlayCue(AudioCue cue)
        {
            var source = Instance._audioSourcePool.GetObjectFromPool();
            source.AudioSource.Setup(cue);
            source.Activate(Vector3.zero, Quaternion.identity);
        }

        public static void PlayCue(AudioCue cue, Vector3 position)
        {
            var source = Instance._audioSourcePool.GetObjectFromPool();
            source.AudioSource.Setup(cue);
            source.Activate(position, Quaternion.identity);
        }
    }
}