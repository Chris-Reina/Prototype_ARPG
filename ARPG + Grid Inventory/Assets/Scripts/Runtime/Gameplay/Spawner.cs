using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class Spawner : MonoBehaviour
{
    public ZombieController prefab;
    public EnergyDoor door;
    public PlayerStreamedData playerData;
    public Queue<EnergyType> spawnQueue = new Queue<EnergyType>();
    public List<GameObject> spawnLocations = new List<GameObject>();
    public List<ZombieController> zombiesInRange = new List<ZombieController>();
    public float spawnTimeMax = 5f;
    public float checkTimeMax = 5f;
    private float _spawnTime;
    private float _checkTime;

    public List<EnergyType> types;

    private void Awake()
    {
        _spawnTime = spawnTimeMax;
        _checkTime = checkTimeMax;
    }
    
    private void Update()
    {
        if (door.IsDead)
        {
            Destroy(gameObject);
            return;
        }
        
        if(spawnQueue.Count == 0)
            CheckDeadZombies();
        else
        {
            SpawnEnemy();
        }
    }

    void CheckDeadZombies()
    {
        if (_checkTime > 0)
        {
            _checkTime -= Time.deltaTime;
            return;
        }
        _checkTime = checkTimeMax;
        
        var zombieEnergies = zombiesInRange
                                                        .Where(x => !x.IsDead)
                                                        .Select(x => x.Model.EnergyOrb)
                                                        .ToList();

        var listNum = types
            .Select(e => zombieEnergies
                .Aggregate(0, (acum, curr) => curr.energyType.Equals(e) ? acum + 1 : acum))
            .ToList();

        var result = listNum.Zip(types, (i, e) => Tuple.Create(e, i))
                                                   .ToDictionary(x => x.Item1, x => x.Item2);
        
        
        var energy = door.EnergyTypeToOpen;
        foreach (var e in energy)
        {
            if (!result.ContainsKey(e)) continue;
            
            result[e] = result[e] - 1;
        }
        
        var enemiesToSpawn = result.Where(x => x.Value < 0)
                                                          .ToDictionary(x => x.Key, y => y.Value);

        if (!enemiesToSpawn.Any()) return;

        var playerE = playerData.EnergyStored;
        if (playerE.Length > 0)
        {
            foreach (var e in playerE)
            {
                if(enemiesToSpawn.ContainsKey(e))
                    enemiesToSpawn[e] = enemiesToSpawn[e] + 1;
            }
        }

        foreach (var enemy in enemiesToSpawn)
        {
            for (var i = 0; i < Mathf.Abs(enemy.Value); i++)
            {
                spawnQueue.Enqueue(enemy.Key);
            }
        }
    }

    void SpawnEnemy()
    {
        if (_spawnTime > 0)
        {
            _spawnTime -= Time.deltaTime;
            return;
        }
        _spawnTime = spawnTimeMax;
        
        var typeToSpawn = spawnQueue.Dequeue();
        var spawnGO = spawnLocations[Random.Range(0, spawnLocations.Count)].transform;
        
        var spawned = Instantiate(prefab, spawnGO.position, spawnGO.rotation);
        spawned.Model.EnergyOrb.energyType = typeToSpawn;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayersUtility.EntityMaskIndex)
        {
            var entity = other.GetComponent<IEntity>();
            if (entity is ZombieController zombie)
            {
                if (!zombiesInRange.Contains(zombie))
                {
                    zombiesInRange.Add(zombie);
                }
            }
        }
    }
}
