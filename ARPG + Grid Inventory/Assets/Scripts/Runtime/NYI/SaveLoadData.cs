using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class SaveLoadData
{
    public SerializableDataDictionary<PowerUpData, string> powerups = new SerializableDataDictionary<PowerUpData, string>();
    public List<AsteroidData> asteroids = new List<AsteroidData>();
    public List<BulletData> bullets = new List<BulletData>();
    public GameStateData gameState;
    public PlayerData player;
}

[Serializable]
public class GameStateData
{
    public int playerLives;
    public bool levelManagerWaiting;
    public int level;
    public int score;

    // public GameStateData(ScoreManager scoreM, LevelManager levelM, PlayerLivesManager livesM)
    // {
    //     playerLives = livesM.CurrentLives;
    //     level = levelM.CurrentLevel;
    //     score = scoreM.CurrentScore;
    // }
}

[Serializable]
public class AsteroidData
{
    public int asteroidType;
    public SerializableVector3 velocity;
    public SerializableVector3 position;
    public SerializableQuaternion rotation;

    // public AsteroidData(Asteroid asteroid)
    // {
    //     asteroidType = (int)asteroid.CurrentType;
    //     velocity = new SerializableVector3(asteroid.Velocity);
    //     position = new SerializableVector3(asteroid.transform.position);
    //     rotation = new SerializableQuaternion(asteroid.transform.rotation);
    // }
}

[Serializable]
public class PlayerData
{
    public int currentWeaponIndex;
    public SerializableVector3 velocity;
    public SerializableVector3 position;
    public SerializableQuaternion rotation;

    public bool hasTimeWarp;
    public bool hasShield;
    public int shieldLives;

    public List<PlayerRewindData> rewindPlayerData = new List<PlayerRewindData>();
    

    // public PlayerData(PlayerModel player)
    // {
    //     currentWeaponIndex = player.CurrentWeaponIndex;
    //     velocity = new SerializableVector3(player.Velocity);
    //     position = new SerializableVector3(player.transform.position);
    //     rotation = new SerializableQuaternion(player.transform.rotation);
    //
    //     hasTimeWarp = player.hasTimeWarp;
    //     hasShield = player.hasShield;
    //     shieldLives = player.shieldLives;
    //
    //     rewindPlayerData = player.rewindData;
    // }
}

[Serializable]
public class PlayerRewindData
{
    public SerializableVector3 velocity;
    public SerializableVector3 position;
    public SerializableQuaternion rotation;


    // public PlayerRewindData(PlayerModel player)
    // {
    //     velocity = new SerializableVector3(player.Velocity);
    //     position = new SerializableVector3(player.transform.position);
    //     rotation = new SerializableQuaternion(player.transform.rotation);
    // }
    //
    // public void GetDataFromPlayer(PlayerModel player)
    // {        
    //     velocity = new SerializableVector3(player.Velocity);
    //     position = new SerializableVector3(player.transform.position);
    //     rotation = new SerializableQuaternion(player.transform.rotation);
    // }
    //
    // public void SetDataToPlayer(PlayerModel player)
    // {
    //     player.Velocity = velocity.ToVector3();
    //     player.transform.position = position.ToVector3();
    //     player.transform.rotation = rotation.ToQuaternion();
    // }
}

[Serializable]
public class PowerUpData
{
    public int powerupType;
    public float timer;
    public SerializableVector3 position;

    // public PowerUpData(IPickUp powerup)
    // {
    //     timer = powerup.Timer;
    //     powerupType = (int)powerup.CurrentPowerUpType;
    //     position = new SerializableVector3(powerup.Position);
    // }
}

[Serializable]
public class BulletData
{
    public SerializableVector3 velocity;
    public SerializableVector3 position;
    public SerializableQuaternion rotation;

    // public BulletData(PlasmaBullet bullet)
    // {
    //     velocity = new SerializableVector3(bullet.Velocity);
    //     position = new SerializableVector3(bullet.transform.position);
    //     rotation = new SerializableQuaternion(bullet.transform.rotation);
    // }
}

[Serializable]
public class SerializableDataDictionary<Tkey, Tvalue>
{
    public List<Tkey> keys = new List<Tkey>();
    public List<Tvalue> values =  new List<Tvalue>();
}
