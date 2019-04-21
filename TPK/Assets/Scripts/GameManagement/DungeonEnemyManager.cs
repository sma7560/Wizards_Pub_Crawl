using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

/// <summary>
/// Manages all enemies in the dungeon.
/// </summary>
public class DungeonEnemyManager : NetworkBehaviour
{
    private MatchManager matchManager;

    [SerializeField] private List<GameObject> monsterList;
    private readonly int maxNumMonsters = 14;           //max number of regular monsters
    private readonly int maxNumSpawnGroups = 9;        //max groups of monster swarms
	private readonly int SwarmSquadSize = 4;
	private readonly float spawnTimerRegular = 5;
	private readonly float spawnTimerSwarm = 5;
    private List<Vector3> spawnLocations;
    [SerializeField] private GameObject SwarmMonster;

    void Awake()
    {
        spawnLocations = new List<Vector3>();
    }

    void Start()
    {
        matchManager = GetComponent<MatchManager>();
    }

    private void Update()
    {
        // if all spawn locations have been spawned to, repopulate spawn points
        if (spawnLocations.Count <= 0)
        {
            SetupSpawnPoints();
        }
    }

    /// <summary>
    /// Starts spawning monsters periodically.
    /// </summary>
    public void StartSpawn()
    {
        if (!isServer) return;

        // Spawn an initial 5 monsters
        for (int i = 1; i <= 5; i++)
        {
            DungeonSpawnMonster();
        }

        // Call summon monster every 4 seconds
		InvokeRepeating("DungeonSpawnMonster", 0f, spawnTimerRegular);
		InvokeRepeating("DungeonSpawnSwarm", 0f, spawnTimerSwarm);
    }

    /// <summary>
    /// Spawns a random monster at a random location if all conditions are met.
    /// </summary>
    private void DungeonSpawnMonster()
    {
        if (!isServer || matchManager.HasMatchEnded()) return;
        if (getNumMonstersSpawned()[0] >= maxNumMonsters || spawnLocations.Count <= 0) return;

        // Spawn a monster at a randomly selected location
        int randLocation = Random.Range(0, spawnLocations.Count);
        List<GameObject> spawnableEnemies = GetCurrentSpawnableMonsterList();
        GameObject randMonster = spawnableEnemies[Random.Range(0, spawnableEnemies.Count)];
        SpawnMonster(GetSpawnLocationOfMonster(randLocation), randMonster);
    }

    /// <summary>
    /// Spawns a swarm of 5 SwarmMonster at a random location if all conditions are met.
    /// </summary>
    private void DungeonSpawnSwarm()
    {
        if (!isServer || matchManager.HasMatchEnded()) return;
        if (getNumMonstersSpawned()[1] >= maxNumSpawnGroups || spawnLocations.Count <= 0) return;

        // Spawn a monster at a randomly selected location
        int randLocation = Random.Range(0, spawnLocations.Count);
        Vector3 locationAt = GetSpawnLocationOfMonster(randLocation);
        List<Vector3> spawnOffset = new List<Vector3>(5);
        spawnOffset.Add(new Vector3(0, 0, 0));
        spawnOffset.Add(new Vector3(1, 0, 1));
        spawnOffset.Add(new Vector3(1, 0, -1));
        spawnOffset.Add(new Vector3(-1, 0, -1));
        spawnOffset.Add(new Vector3(-1, 0, 1));
        
		for (int i=0; i< SwarmSquadSize; i++)
        {
            SpawnMonster(locationAt - spawnOffset[i], SwarmMonster);
        }
    }

    /// <summary>
    /// Return how many regular monsters, and how many groups of monster swarms in an array,
    /// where [0] = regular, [1] = groups of swarms
    /// </summary>
    private int[] getNumMonstersSpawned()
    {
        GameObject[] spawnedMonsters = GameObject.FindGameObjectsWithTag("Enemy");
        int[] count = { 0, 0 };
        foreach(GameObject monster in spawnedMonsters)
        {
            if(monster.name.Contains("SwarmGoblin"))
            {
                count[1] = count[1] + 1;
            }
            else
            {
                count[0] = count[0] + 1;
            }
        }
        return count;
    }


    /// <summary>
    /// Spawns a monster at the specified location.
    /// </summary>
    private void SpawnMonster(Vector3 location, GameObject monsterType)
    {
        if (!isServer || matchManager.HasMatchEnded()) return;

        Quaternion rotate = Quaternion.Euler(0, 0, 0);
        GameObject monsterToSpawn = Instantiate(monsterType, location, rotate);
        NetworkServer.Spawn(monsterToSpawn);
    }

    /// <summary>
    /// populate enemy spawn point list.
    /// </summary>
    private void SetupSpawnPoints()
    {
        GameObject[] spawnPoints = GameObject.FindGameObjectsWithTag("enemySpawnPoint");

        for (int i = 0; i < spawnPoints.Length; i++)
        {
            spawnLocations.Add(spawnPoints[i].transform.position);
        }
    }

    /// <returns>
    /// Returns spawn location based on index.
    /// </returns>
    /// <param name="spawnLocationIndex">Index of spawn location.</param>
    public Vector3 GetSpawnLocationOfMonster(int spawnLocationIndex)
    {
        Vector3 spawnLocation = spawnLocations[spawnLocationIndex];
        spawnLocations.RemoveAt(spawnLocationIndex);
        return spawnLocation;
    }

    /// <returns>
    /// Returns all spawnable enemy types if less than 4 heavy monsters; otherwise, returns only non-heavy enemy types.
    /// </returns>
    private List<GameObject> GetCurrentSpawnableMonsterList()
    {
        int numHeavyMonsters = 0;

        //count all heavy monsters
        List<GameObject> monsterListWithoutHeavy = new List<GameObject>();
        foreach (GameObject enemy in GameObject.FindGameObjectsWithTag("Enemy"))
        {
            if (enemy.name.Contains("Heavy"))
            {
                numHeavyMonsters++;
            }
        }

        //return list without heavy monsters
        if (numHeavyMonsters >= 4)
        {
            foreach (GameObject enemy in monsterList)
            {
                if (!enemy.name.Contains("Heavy"))
                {
                    monsterListWithoutHeavy.Add(enemy);
                }
            }
            return monsterListWithoutHeavy;
        }
        //return list with all monsters
        else
        {
            return monsterList;
        }
    }
}
