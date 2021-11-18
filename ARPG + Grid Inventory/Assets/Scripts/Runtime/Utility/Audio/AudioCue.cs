using System;
using UnityEngine;

namespace DoaT
{
    [CreateAssetMenu(fileName = "AudioCue", menuName = "Audio/AudioCue"), Serializable]
    public class AudioCue : ScriptableObject
    {
        public AudioClip clip;
        public bool loop = false;
        [Range(0f, 1f)] public float volume = 1f;
        public FloatRange pitch = new FloatRange(1, 1, -3, 3);
        [Range(-1f, 1f)] public float stereoPan = 0;
        [Range(0f, 1f)] public float spatialBlend = 0f;
        [Range(0f, 1.1f)] public float reverbZoneMix = 1f;
    }
}
