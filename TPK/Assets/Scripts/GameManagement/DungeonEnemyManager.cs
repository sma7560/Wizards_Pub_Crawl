using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

/// <summary>
/// Manages all enemies in the dungeon.
/// </summary>
public class DungeonEnemyManager : NetworkBehaviour
{
    public IUnityService unityService;
    private MatchManager matchManager;

    [SerializeField] private List<GameObject> monsterList;
    private readonly int maxNumMonsters = 16;
    private List<Vector3> spawnLocations;

    void Awake()
    {
        spawnLocations = new List<Vector3>();
    }
    
    void Start()
    {
        if (unityService == null)
        {
            unityService = new UnityService();
        }

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
        InvokeRepeating("DungeonSpawnMonster", 0f, 4f);
    }

    /// <summary>
    /// Spawns a random monster at a random location if all conditions are met.
    /// </summary>
    private void DungeonSpawnMonster()
    {
        if (!isServer || matchManager.HasMatchEnded()) return;
        if (GameObject.FindGameObjectsWithTag("Enemy").Length >= maxNumMonsters || spawnLocations.Count <= 0) return;

        // Spawn a monster at a randomly selected location
        int randLocation = Random.Range(0, spawnLocations.Count);
        List<GameObject> spawnableEnemies = GetCurrentSpawnableMonsterList();
        GameObject randMonster = spawnableEnemies[Random.Range(0, spawnableEnemies.Count)];
        SpawnMonster(GetSpawnLocationOfMonster(randLocation), randMonster);
    }


    /// <summary>
    /// Spawns a monster at the specified location.
    /// </summary>
    private void SpawnMonster(Vector3 location, GameObject monsterType)
    {
        if (!isServer || matchManager.HasMatchEnded()) return;

        Quaternion rotate = Quaternion.Euler(0, 0, 0);
        GameObject monsterToSpawn = unityService.Instantiate(monsterType, location, rotate);
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
            else
            {
                monsterListWithoutHeavy.Add(enemy);
            }
        }

        //return list without heavy monsters
        if (numHeavyMonsters >= 4)
        {
            return monsterListWithoutHeavy;
        }
        //return list with all monsters
        else
        {
            return monsterList;
        }
    }
}
