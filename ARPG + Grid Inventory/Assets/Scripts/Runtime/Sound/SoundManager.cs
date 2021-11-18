using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class SoundManager : MonoBehaviour
{
    private static SoundManager Instance { get; set; }
    
    [SerializeField] private AudioSource _prefab;
    private static AudioSource Prefab => Instance._prefab;
    

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else if (Instance != this) Destroy(this);
    }
    

    /// <summary>
    /// Instantiates an AudioSource with the given Clip. [volume = 1f]
    /// </summary>
    /// <param name="clip">AudioClip To Play.</param>
    public static void PlaySound(AudioClip clip)
    {
        var aSrc = Instantiate(Prefab, new Vector3(0, 0, 0), Quaternion.identity);
        aSrc.clip = clip;
        aSrc.loop = false;
        aSrc.volume = 1f;
        aSrc.Play();
    }

    /// <summary>
    ///  Instantiates an AudioSource with the given Clip and Volume.
    /// </summary>
    /// <param name="clip">AudioClip To Play.</param>
    /// <param name="volume">The Volume of said AudioClip</param>
    public static void PlaySound(AudioClip clip, float volume)
    {
        var aSrc = Instantiate(Prefab, new Vector3(0, 0, 0), Quaternion.identity);
        aSrc.clip = clip;
        aSrc.loop = false;
        aSrc.volume = volume;
        aSrc.Play();
    }

    /// <summary>
    /// Instantiates an AudioSource with the given Clip and Volume.
    /// </summary>
    /// <param name="position">Instantiated Position in World Space.</param>
    /// <param name="clip">AudioClip To Play.</param>
    /// <param name="volume">The Volume of said AudioClip.</param>
    public static void PlaySound(AudioClip clip, Vector3 position, float volume)
    {
        var aSrc = Instantiate(Prefab, position, Quaternion.identity);
        aSrc.clip = clip;
        aSrc.loop = false;
        aSrc.volume = volume;
        aSrc.Play();
    }

    /// <summary>
    /// Instantiates an AudioSource with the given Clip, Volume and Pitch.
    /// </summary>
    /// <param name="position">Instantiated Position in World Space.</param>
    /// <param name="clip">AudioClip To Play.</param>
    /// <param name="volume">The Volume of said AudioClip.</param>
    /// <param name="pitch">The Pitch assigned to the AudioSource.</param>
    public static void PlaySound(AudioClip clip, Vector3 position, float volume, float pitch)
    {
        var aSrc = Instantiate(Prefab, position, Quaternion.identity);
        aSrc.clip = clip;
        aSrc.loop = false;
        aSrc.pitch = pitch;
        aSrc.volume = volume;
        aSrc.Play();
    }

    /// <summary>
    /// Instantiates an AudioSource with the given Clip, Volume and Pitch. Pitch is Randomized
    /// </summary>
    /// <param name="position">Instantiated Position in World Space.</param>
    /// <param name="clip">AudioClip To Play.</param>
    /// <param name="volume">The Volume of said AudioClip.</param>
    /// <param name="pitchRange">The Pitch assigned to the AudioSource.</param>
    public static void PlaySound(AudioClip clip, Vector3 position, float volume, Tuple<float,float> pitchRange)
    {
        var (pitchMin, pitchMax) = pitchRange;
        var aSrc = Instantiate(Prefab, position, Quaternion.identity);
        aSrc.clip = clip;
        aSrc.loop = false;
        aSrc.pitch = Random.Range(pitchMin, pitchMax);
        aSrc.volume = volume;
        aSrc.Play();
    }

    /// <summary>
    /// Instantiates an AudioSource with the given Clip, Volume and Pitch. Pitch is Randomized
    /// </summary>
    /// <param name="position">Instantiated Position in World Space.</param>
    /// <param name="clip">AudioClip To Play.</param>
    /// <param name="volume">The Volume of said AudioClip.</param>
    /// <param name="pitchRange">The Pitch assigned to the AudioSource.</param>
    /// <param name="loop">Should it Loop?</param>
    public static void PlaySound(AudioClip clip, Vector3 position, float volume, Tuple<float,float> pitchRange, bool loop)
    {
        var (pitchMin, pitchMax) = pitchRange;
        var aSrc = Instantiate(Prefab, position, Quaternion.identity);
        aSrc.clip = clip;
        aSrc.loop = loop;
        aSrc.pitch = Random.Range(pitchMin, pitchMax);
        aSrc.volume = volume;
        aSrc.Play();
    }
    
    

    /// <summary>
    /// Instantiates an AudioSource with the given Clip. SpatialBlended. [volume = 1f]
    /// </summary>
    /// <param name="spatialBlend">Sets how much this AudioSource is affected by 3D Space calculations.</param>
    /// <param name="clip">AudioClip To Play.</param>
    public static void PlaySound(float spatialBlend, AudioClip clip)
    {
        var aSrc = Instantiate(Prefab, new Vector3(0, 0, 0), Quaternion.identity);
        aSrc.clip = clip;
        aSrc.loop = false;
        aSrc.volume = 1f;
        aSrc.spatialBlend = spatialBlend;
        aSrc.Play();
    }

    /// <summary>
    ///  Instantiates an AudioSource with the given Clip and Volume. SpatialBlended.
    /// </summary>
    /// <param name="spatialBlend">Sets how much this AudioSource is affected by 3D Space calculations.</param>
    /// <param name="clip">AudioClip To Play.</param>
    /// <param name="volume">The Volume of said AudioClip</param>
    public static void PlaySound(float spatialBlend, AudioClip clip, float volume)
    {
        var aSrc = Instantiate(Prefab, new Vector3(0, 0, 0), Quaternion.identity);
        aSrc.clip = clip;
        aSrc.loop = false;
        aSrc.volume = volume;
        aSrc.spatialBlend = spatialBlend;
        aSrc.Play();
    }

    /// <summary>
    /// Instantiates an AudioSource with the given Clip and Volume. SpatialBlended.
    /// </summary>
    /// <param name="spatialBlend">Sets how much this AudioSource is affected by 3D Space calculations.</param>
    /// <param name="position">Instantiated Position in World Space.</param>
    /// <param name="clip">AudioClip To Play.</param>
    /// <param name="volume">The Volume of said AudioClip.</param>
    public static void PlaySound(float spatialBlend, AudioClip clip, Vector3 position, float volume)
    {
        var aSrc = Instantiate(Prefab, position, Quaternion.identity);
        aSrc.clip = clip;
        aSrc.loop = false;
        aSrc.volume = volume;
        aSrc.spatialBlend = spatialBlend;
        aSrc.Play();
    }

    /// <summary>
    /// Instantiates an AudioSource with the given Clip, Volume and Pitch. SpatialBlended.
    /// </summary>
    /// <param name="spatialBlend">Sets how much this AudioSource is affected by 3D Space calculations.</param>
    /// <param name="position">Instantiated Position in World Space.</param>
    /// <param name="clip">AudioClip To Play.</param>
    /// <param name="volume">The Volume of said AudioClip.</param>
    /// <param name="pitch">The Pitch assigned to the AudioSource.</param>
    public static void PlaySound(float spatialBlend, AudioClip clip, Vector3 position, float volume, float pitch)
    {
        var aSrc = Instantiate(Prefab, position, Quaternion.identity);
        aSrc.clip = clip;
        aSrc.loop = false;
        aSrc.pitch = pitch;
        aSrc.volume = volume;
        aSrc.spatialBlend = spatialBlend;
        aSrc.Play();
    }

    /// <summary>
    /// Instantiates an AudioSource with the given Clip, Volume and Pitch. Pitch is Randomized. SpatialBlended.
    /// </summary>
    /// <param name="spatialBlend">Sets how much this AudioSource is affected by 3D Space calculations.</param>
    /// <param name="position">Instantiated Position in World Space.</param>
    /// <param name="clip">AudioClip To Play.</param>
    /// <param name="volume">The Volume of said AudioClip.</param>
    /// <param name="pitchRange">The Pitch assigned to the AudioSource.</param>
    public static void PlaySound(float spatialBlend, AudioClip clip, Vector3 position, float volume, Tuple<float,float> pitchRange)
    {
        var (pitchMin, pitchMax) = pitchRange;
        var aSrc = Instantiate(Prefab, position, Quaternion.identity);
        aSrc.clip = clip;
        aSrc.loop = false;
        aSrc.pitch = Random.Range(pitchMin, pitchMax);
        aSrc.volume = volume;
        aSrc.spatialBlend = spatialBlend;
        aSrc.Play();
    }
    
    /// <summary>
    /// Instantiates an AudioSource with the given Clip, Volume and Pitch. Pitch is Randomized. SpatialBlended.
    /// </summary>
    /// <param name="spatialBlend">Sets how much this AudioSource is affected by 3D Space calculations.</param>
    /// <param name="position">Instantiated Position in World Space.</param>
    /// <param name="clip">AudioClip To Play.</param>
    /// <param name="volume">The Volume of said AudioClip.</param>
    /// <param name="pitchRange">The Pitch assigned to the AudioSource.</param>
    /// <param name="loop">Should it Loop?</param>
    public static void PlaySound(float spatialBlend, AudioClip clip, Vector3 position, float volume, Tuple<float,float> pitchRange, bool loop)
    {
        var (pitchMin, pitchMax) = pitchRange;
        var aSrc = Instantiate(Prefab, position, Quaternion.identity);
        aSrc.clip = clip;
        aSrc.loop = loop;
        aSrc.volume = volume;
        aSrc.pitch = Random.Range(pitchMin, pitchMax);
        aSrc.spatialBlend = spatialBlend;
        aSrc.Play();
    }
    
}
