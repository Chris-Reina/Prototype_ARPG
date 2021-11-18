using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

[RequireComponent(typeof(AudioSource))]
public class MusicManager : MonoBehaviour
{
    public static MusicManager Instance { get; private set; }

    public AudioSource MainSource { get; private set; }
    public AudioSource SideSource { get; private set; }

    [SerializeField] private AudioSource _sourceOne = default;
    [SerializeField] private AudioSource _sourceTwo = default;
    [Space]
    [Range(0f, 1f), SerializeField] private float _transitionTime = 0.5f;
    [Range(0f, 1f), SerializeField] private float _musicVolume = 1;
    [SerializeField] private AudioMixerGroup _mixerGroup = default;
    [Space]
    [SerializeField] private AudioClip _defaultMusic;

    private bool _shouldChange = false;
    
    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);

        if (_sourceOne == null || _sourceTwo == null)
        {
            var sources = GetComponents<AudioSource>();
            if(sources.Length >= 2)
            {
                _sourceOne = sources[0];
                _sourceTwo = sources[1];
            }
            else
            {
                if (_sourceOne == null)
                {
                    _sourceOne = gameObject.AddComponent<AudioSource>();
                    _sourceOne.playOnAwake = false;
                }

                if (_sourceTwo == null)
                {
                    _sourceTwo = gameObject.AddComponent<AudioSource>();
                    _sourceTwo.playOnAwake = false;
                }
            }
        }

        _sourceOne.outputAudioMixerGroup = _mixerGroup;
        _sourceTwo.outputAudioMixerGroup = _mixerGroup;

        MainSource = _sourceOne;
        SideSource = _sourceTwo;

        SideSource.volume = 0;

        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        if (!MainSource.isPlaying && !SideSource.isPlaying)
        {
            MainSource.clip = _defaultMusic;
            MainSource.Play();
        }
    }

    private void Update()
    {
        
        if (SideSource.volume < _musicVolume && _shouldChange)
        {

            MainSource.volume -= _musicVolume / _transitionTime * Time.deltaTime;
            SideSource.volume += _musicVolume / _transitionTime * Time.deltaTime;

            //Debug.Log($"MainSource Volume : {MainSource.volume}  ---  MainSource clip : {MainSource.clip}");
            //Debug.Log($"SideSource Volume : {SideSource.volume}  ---  SideSource clip : {SideSource.clip}");

            if (SideSource.volume >= _musicVolume)
            {
                SideSource.volume = _musicVolume;
                _shouldChange = false;
                ChangeMain();
            }
        }

    }

    public void ChangeMusicTo(AudioClip clip)
    {
        SideSource.clip = clip;
        SideSource.Play();

        _shouldChange = true;
    }

    private void ChangeMain()
    {
        if (MainSource == _sourceOne)
        {
            MainSource = _sourceTwo;
            SideSource = _sourceOne;
        }
        else
        {
            MainSource = _sourceOne;
            SideSource = _sourceTwo;
        }

        SideSource.Stop();
    }
}

