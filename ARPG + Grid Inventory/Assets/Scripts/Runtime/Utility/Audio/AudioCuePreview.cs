using UnityEngine;

namespace DoaT
{
    [ExecuteInEditMode, RequireComponent(typeof(AudioSource))]
    public class AudioCuePreview : MonoBehaviour
    {
        public AudioCue cue;

        [SerializeField] private AudioSource _audioSource;

        public bool IsPlaying => _audioSource == null ? false : _audioSource.isPlaying;


        private void Awake()
        {
            if (_audioSource == null) _audioSource = GetComponent<AudioSource>();
        }

        private void OnEnable()
        {
            if (_audioSource == null) _audioSource = GetComponent<AudioSource>();
        }

        public void PreviewCue()
        {
            _audioSource.Stop();
            _audioSource.Setup(cue);
            _audioSource.Play();
        }

        public void PreviewCue(AudioCue c)
        {
            _audioSource.Stop();
            _audioSource.Setup(c);
            _audioSource.Play();
        }

        public void StopPreview()
        {
            _audioSource.Stop();
        }
    }
}
