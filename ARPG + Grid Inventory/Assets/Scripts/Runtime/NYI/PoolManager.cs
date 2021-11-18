using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using UnityEngine;
using Object = System.Object;

//MyA1-P1
//Decidimos hacer un Pool manager que tenga diferentes PoolAdapters para los diferentes pool ya que originalmente
//teniamos diferentes pools especificos y un PoolData que tenia referencias estaticas a dichos pools, es decir, 
//refactorizamos el PoolData en este script para representar el sistema que teniamos originalmente y el PoolAdapter 
//contiene los cambios especificos de la consigna.


[DisallowMultipleComponent]
public class PoolManager : MonoBehaviour
{
    public static PoolAdapter PlasmaBullet { get { return _plasmaBulletPool; } }
    public static PoolAdapter PlasmaBomb{ get { return _plasmaBombPool; } }
    public static PoolAdapter AsteroidPool { get { return _asteroidPool; } }
    public static PoolAdapter AudioSourcePool { get { return _audioSourcePool; } }
    public static PoolAdapter ParticlePool { get { return _particlePool; } }

    private static PoolAdapter _plasmaBulletPool = default;
    private static PoolAdapter _plasmaBombPool = default;
    private static PoolAdapter _asteroidPool = default;
    private static PoolAdapter _audioSourcePool = default;
    private static PoolAdapter _particlePool = default;
    
    
    // private PlasmaBulletFactory _plasmaBulletFactory = new PlasmaBulletFactory();
    // [SerializeField] private PlasmaBullet _bulletPrefab = default;
    //
    // private PlasmaBombFactory _plasmaBombFactory = new PlasmaBombFactory();
    // [SerializeField] private BombHandler _bombPrefab = default;
    //
    // private AsteroidFactory _asteroidFactory = new AsteroidFactory();
    // [SerializeField] private Asteroid _asteroidPrefab = default;
    //
    // private AudioSourceFactory _audioSourceFactory = new AudioSourceFactory();
    // [SerializeField] private GameObject _audioSourceParent = default;
    //
    // private ParticleFactory _particleFactory = new ParticleFactory();
    // [SerializeField] private ParticleDurationTracker _particlePrefab = default;


    /*private void Update()
    {
        Debug.Log($"PlasmaBullet Active: {_plasmaBulletFactory != null}");
        Debug.Log($"Asteroid Active: {_asteroidPool != null}");
        Debug.Log($"AudioTracker Active: {_audioSourcePool != null}");
        Debug.Log($"ParticleTracker Active: {_particlePool != null}");
    }*/

    // private PlasmaBullet CreatePlasmaBullet()
    // {
    //     return _plasmaBulletFactory.Create(_bulletPrefab);
    // }
    //
    // private BombHandler CreatePlasmaBomb()
    // {
    //     return _plasmaBombFactory.Create(_bombPrefab);
    // }
    //
    // private Asteroid CreateAsteroid()
    // {
    //     return _asteroidFactory.Create(_asteroidPrefab);
    // }
    //
    // private AudioDurationTracker CreateAudioSource()
    // {
    //     return _audioSourceFactory.Create(_audioSourceParent);
    // }
    //
    // private ParticleDurationTracker CreateParticle()
    // {
    //     return _particleFactory.Create(_particlePrefab);
    // }
    //
    // private void DisablePoolObject(IPoolObject poolObject)
    // {
    //     poolObject.OnDispose();
    // } 
    //
    //
    // #region Utility
    // public static List<T> CurrentActiveObjectsInPool<T>() where T : MonoBehaviour
    // {
    //     var objects = FindObjectsOfType<T>();
    //     var activeObjects = new List<T>();
    //
    //     foreach (var obj in objects)
    //     {
    //         if (obj.gameObject.activeSelf)
    //         {
    //             activeObjects.Add(obj);
    //         }
    //     }
    //     return activeObjects;
    // }
    //
    // public static void RecallAllAssets()
    // {
    //     RecallAssetsOfType<Asteroid>();
    //     RecallAssetsOfType<PlasmaBullet>();
    //     RecallAssetsOfType<AudioDurationTracker>();
    //     RecallAssetsOfType<ParticleDurationTracker>();
    // }
    //
    // private static void RecallAssetsOfType<T>() where T : UnityEngine.Object
    // {
    //     var assets = FindObjectsOfType<T>().OfType<IPoolObject>();
    //
    //     foreach (var asset in assets)
    //     {
    //         asset.OnDispose();
    //     }
    // }
    // #endregion
}

