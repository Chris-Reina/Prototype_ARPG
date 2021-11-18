using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class SaveLoadManager : MonoBehaviour
{
    // public bool CanLoadGame { get { return File.Exists($"{Application.dataPath}{savePath}") && CanSaveGame; } }
    // public bool CanSaveGame { get { return !ReferenceManager.PlayerRespawnManager.IsRespawning && !TimeWarpPowerUp.isRemembering; } }
    // public bool ShouldLoadGame { get { return GameControl.ShouldLoadGame; } }
    // public bool ShouldPause { get { return GameControl.ShouldLoadGame && _shouldPause; } }

    public bool ShouldPause
    {
        get { return _shouldPause; }
    }

    //[HideInInspector] 
    public SaveLoadData data;
    public string savePath = "/Resources/SaveFile.dat";

    private GameStateSaveLoad _gameStateSaveLoad;
    private PlayerSaveLoad _playerSaveLoad;
    private AsteroidSaveLoad _asteroidSaveLoad;
    private BulletSaveLoad _bulletSaveLoad;
    private PowerUpSaveLoad _powerUpSaveLoad;
    private bool _shouldPause = false;

    private void Awake()
    {
        ClearSaveData();

        // if (GameControl.ShouldLoadGame)
        //     _shouldPause = true;
    }

    private void Start()
    {

        // if (ShouldLoadGame)
        // {
        //     ReferenceManager.CountDownActivator.Subscribe(this);
        //     LoadGameFromStart();
        // }
    }

    public void SaveGame()
    {
        ClearSaveData();
        /*_gameStateSaveLoad.Save(ReferenceManager.ScoreManager, ReferenceManager.LevelManager, ReferenceManager.PlayerLivesManager);
        _playerSaveLoad.Save(ReferenceManager.PlayerModel);

        var bulletsActive = PoolManager.CurrentActiveObjectsInPool<PlasmaBullet>();

        if(bulletsActive.Count > 0)
        {
            foreach (var bul in bulletsActive)
            {
                _bulletSaveLoad.Save(bul);
            }
        }        

        var asteroidsActive = PoolManager.CurrentActiveObjectsInPool<Asteroid>();
        foreach (var ast in asteroidsActive)
        {
            _asteroidSaveLoad.Save(ast);            
        }

        var powerupsActive = ReferenceManager.PowerUpSpawner.activeSpawns;
        foreach (var pus in powerupsActive)
        {
            _powerUpSaveLoad.Save(pus);
        }*/

        data.SaveBinary($"{Application.dataPath}{savePath}");
    }

    public void LoadGame()
    {
        // if (!CanLoadGame) return;

        ClearSaveData();

        //EventManager.Trigger(EventsData.OnGameLoadBegin, new object[] { });

        //PoolManager.RecallAllAssets();
        //ReferenceManager.PowerUpSpawner.ReturnAllActivePowerUps();

        data = BinarySerializer.LoadBinary<SaveLoadData>($"{Application.dataPath}{savePath}");

        // _gameStateSaveLoad.Load(ReferenceManager.ScoreManager, ReferenceManager.LevelManager, 
        //                         ReferenceManager.PlayerLivesManager, data.gameState);
        // _playerSaveLoad.Load(ReferenceManager.PlayerModel, data.player);
        //
        // if (data.bullets.Count > 0)
        // {
        //     foreach (var bulData in data.bullets)
        //     {
        //         var bullet = PoolManager.PlasmaBullet.GetObject<PlasmaBullet>();
        //
        //         _bulletSaveLoad.Load(bullet, bulData);
        //     }
        // }
        //
        // foreach (var astData in data.asteroids)
        // {
        //     var asteroid = PoolManager.AsteroidPool.GetObject<Asteroid>();
        //
        //     _asteroidSaveLoad.Load(asteroid, astData);
        // }
        //
        // if (data.powerups.keys.Count > 0)
        // {
        //     for (int i = 0; i < data.powerups.keys.Count; i++)
        //     {
        //         if (data.powerups.values[i] == typeof(ShieldPickUp).ToString())
        //         {
        //             var pwrUp = ReferenceManager.PowerUpSpawner.GetShieldPowerUp();
        //             _powerUpSaveLoad.Load(pwrUp, data.powerups.keys[i]);
        //         }
        //         else
        //         {
        //             var pwrUp = ReferenceManager.PowerUpSpawner.GetTimewarpPowerUp();
        //             _powerUpSaveLoad.Load(pwrUp, data.powerups.keys[i]);
        //         }
        //     }
        // }

        //EventManager.Trigger(EventsData.OnGameLoadSuccessful, new object[] { });
    }

    public void LoadGameFromStart()
    {
        //if (!CanLoadGame) return;

        ClearSaveData();

        //EventManager.Trigger(EventsData.OnGameLoadBegin, new object[] { });

        //PoolManager.RecallAllAssets();

        data = BinarySerializer.LoadBinary<SaveLoadData>($"{Application.dataPath}{savePath}");

        // _gameStateSaveLoad.Load(ReferenceManager.ScoreManager, ReferenceManager.LevelManager, ReferenceManager.PlayerLivesManager, data.gameState);
        // _playerSaveLoad.Load(ReferenceManager.PlayerModel, data.player);

        // if (data.bullets.Count > 0)
        // {
        //     foreach (var bulData in data.bullets)
        //     {
        //         var bullet = PoolManager.PlasmaBullet.GetObject<PlasmaBullet>();
        //
        //         _bulletSaveLoad.Load(bullet, bulData);
        //     }
        // }
        //
        // foreach (var astData in data.asteroids)
        // {
        //     var asteroid = PoolManager.AsteroidPool.GetObject<Asteroid>();
        //
        //     _asteroidSaveLoad.Load(asteroid, astData);
        // }
        //
        // if(data.powerups.keys.Count > 0)
        // {
        //     for (int i = 0; i < data.powerups.keys.Count; i++)
        //     {
        //         if (data.powerups.values[i] == typeof(ShieldPickUp).ToString())
        //         {
        //             var pwrUp = ReferenceManager.PowerUpSpawner.GetShieldPowerUp();
        //             _powerUpSaveLoad.Load(pwrUp, data.powerups.keys[i]);
        //         }
        //         else
        //         {
        //             var pwrUp = ReferenceManager.PowerUpSpawner.GetTimewarpPowerUp();
        //             _powerUpSaveLoad.Load(pwrUp, data.powerups.keys[i]);
        //         }
        //     }
        // }

        ClearSaveData();
        
        // ReferenceManager.CountDownActivator.ActivateCountDown(ReferenceManager.LevelManager.TimeBetweenLevels, true);
    }

    // public void AddDataPlayer(PlayerData pData)
    // {
    //     data.player = pData;
    // }
    // public void AddDataAsteroid(AsteroidData aData)
    // {
    //     data.asteroids.Add(aData);
    // }
    // public void AddDataGameState(GameStateData gData)
    // {
    //     data.gameState = gData;
    // }
    // public void AddDataBullet(BulletData bData)
    // {
    //     data.bullets.Add(bData);
    // }    
    // public void AddDataPowerUp(PowerUpData puData, Type puType)
    // {
    //     data.powerups.keys.Add(puData);
    //     data.powerups.values.Add(puType.ToString());
    // }

    public void ClearSaveData()
    {
        data = new SaveLoadData();
        //
        // _gameStateSaveLoad = new GameStateSaveLoad(this);
        // _playerSaveLoad = new PlayerSaveLoad(this);
        // _asteroidSaveLoad = new AsteroidSaveLoad(this);
        // _bulletSaveLoad = new BulletSaveLoad(this);
        // _powerUpSaveLoad = new PowerUpSaveLoad(this);
    }
}

public class PlayerSaveLoad
{
    SaveLoadManager _saveLoadManager;

    public PlayerSaveLoad(SaveLoadManager saveManager)
    {
        _saveLoadManager = saveManager;
    }

    public void Save(PlayerModel player)
    {
        //var data = new PlayerData(player);
        //_saveLoadManager.AddDataPlayer(data);
    }

    public void Load(PlayerModel player, PlayerData data)
    {
        // player.CurrentWeaponIndex = data.currentWeaponIndex;
        // player.transform.position =  data.position.ToVector3();
        // player.transform.rotation = data.rotation.ToQuaternion();
        // player.Velocity = data.velocity.ToVector3();
        //
        // player.hasTimeWarp = data.hasTimeWarp;
        // player.hasShield = data.hasShield;
        // player.shieldLives = data.shieldLives;
        //
        // player.rewindData = data.rewindPlayerData;
    }
}

public class AsteroidSaveLoad
{
    SaveLoadManager _saveLoadManager;

    public AsteroidSaveLoad(SaveLoadManager saveManager)
    {
        _saveLoadManager = saveManager;
    }

    // public void Save(Asteroid asteroid)
    // {
    //     var data = new AsteroidData(asteroid);
    //     _saveLoadManager.AddDataAsteroid(data);
    // }
    //
    // public void Load(Asteroid asteroid, AsteroidData data)
    // {
    //     asteroid.CurrentType = (AsteroidType)data.asteroidType;
    //     asteroid.Velocity = data.velocity.ToVector3();
    //     asteroid.transform.position = data.position.ToVector3();
    //     asteroid.transform.rotation = data.rotation.ToQuaternion();
    // }
}

public class GameStateSaveLoad
{
    SaveLoadManager _saveLoadManager;

    public GameStateSaveLoad(SaveLoadManager saveManager)
    {
        _saveLoadManager = saveManager;
    }

    // public void Save(ScoreManager scoreM, LevelManager levelM, PlayerLivesManager livesM)
    // {
    //     var data = new GameStateData(scoreM, levelM, livesM);
    //     _saveLoadManager.AddDataGameState(data);
    // }
    //
    // public void Load(ScoreManager scoreM, LevelManager levelM, PlayerLivesManager livesM, GameStateData data)
    // {
    //     scoreM.CurrentScore = data.score;
    //     levelM.CurrentLevel = data.level;
    //     levelM.IsWaiting = false;
    //     livesM.CurrentLives = data.playerLives;
    // }
}

public class PowerUpSaveLoad
{
    SaveLoadManager _saveLoadManager;

    public PowerUpSaveLoad(SaveLoadManager saveManager)
    {
        _saveLoadManager = saveManager;
    }

    // public void Save(IPickUp powerup)
    // {
    //     var data = new PowerUpData(powerup);
    //
    //     if(powerup.GetType() == typeof(TimeWarpPickUp))
    //         _saveLoadManager.AddDataPowerUp(data, typeof(TimeWarpPickUp));
    //     else if(powerup.GetType() == typeof(ShieldPickUp))
    //         _saveLoadManager.AddDataPowerUp(data, typeof(ShieldPickUp));
    // }
    //
    //
    //
    // public void Load(ShieldPickUp powerup, PowerUpData data)
    // {
    //     powerup.CurrentPowerUpType = (PowerUpType)data.powerupType;
    //     powerup.Timer = data.timer;
    //     powerup.Position = data.position.ToVector3();
    // }
    //
    // public void Load(TimeWarpPickUp powerup, PowerUpData data)
    // {
    //     powerup.CurrentPowerUpType = (PowerUpType)data.powerupType;
    //     powerup.Timer = data.timer;
    //     powerup.Position = data.position.ToVector3();
    // }
}

public class BulletSaveLoad
{
    SaveLoadManager _saveLoadManager;

    public BulletSaveLoad(SaveLoadManager saveManager)
    {
        _saveLoadManager = saveManager;
    }

    // public void Save(PlasmaBullet bullet)
    // {
    //     var data = new BulletData(bullet);
    //     _saveLoadManager.AddDataBullet(data);
    // }
    //
    // public void Load(PlasmaBullet bullet, BulletData data)
    // {
    //     bullet.Velocity = data.velocity.ToVector3();
    //     bullet.transform.position = data.position.ToVector3();
    //     bullet.transform.rotation = data.rotation.ToQuaternion();
    // }
}