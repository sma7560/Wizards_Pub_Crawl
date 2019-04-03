using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

/// <summary>
/// Manages all enemies in the dungeon.
/// </summary>
public class DungeonEnemyManager : NetworkBehaviour
{
    public IUnityService unityService;  // for unit testing
    private MatchManager matchManager;

    //Monster types
    [SerializeField]
    private List<GameObject> monsterList;
    private int currentNumMonsters = 0;
    private int heavyMonsters = 0;

    private List<Vector3> spawnLocations = new List<Vector3>();

    /// <summary>
    /// Initialize variables.
    /// </summary>
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
        if(spawnLocations.Count <=0)
        {
            SetSpawnPoints();
        }
    }

    /// <summary>
    /// Starts spawning monsters periodically.
    /// </summary>
    public void StartSpawn()
    {
        if (!isServer) return;
        for(int i =1; i<=5; i++)
        {
            DungeonSpawnMonster();
        }
        //call summon monster every 4 seconds
        InvokeRepeating("DungeonSpawnMonster", 0f, 4);
    }

    /// <summary>
    /// Spawn monster
    /// </summary>
    private void DungeonSpawnMonster()
    {
        if (!isServer || matchManager.HasMatchEnded()) return;

        //only spawn monster if current spawned monsters number less than 16
        GameObject[] currentMonsterList = GameObject.FindGameObjectsWithTag("Enemy");
        currentNumMonsters = currentMonsterList.Length;
        if (currentNumMonsters > 16)
        {
            return;
        }


        //randomly select one of the spawn locations
        int randLocation = Random.Range(0, spawnLocations.Count);
        //get list of current spawnable enemy types
        List<GameObject> currentSpawnableEnemies = getCurrentSpawnableMonsterList(currentMonsterList);
        //randomly select one of the spawnable enemies
        GameObject randMonster = currentSpawnableEnemies[Random.Range(0, currentSpawnableEnemies.Count)];
        //call function to spawn monster on server
        SpawnMonster(GetSpawnLocationOfMonster(randLocation), randMonster);
    }


    /// <summary>
    /// Spawn monster on server
    /// </summary>
    private void SpawnMonster(Vector3 location, GameObject monsterType)
    {
        if (!isServer || matchManager.HasMatchEnded()) return;

        Quaternion rotate = Quaternion.Euler(0, 0, 0);
        GameObject monsterToSpawn = unityService.Instantiate(monsterType, location, rotate);
        NetworkServer.Spawn(monsterToSpawn);
        currentNumMonsters++;
    }

    /// <summary>
    /// Sets the enemy spawn points.
    /// </summary>
    public void SetSpawnPoints()
    {
        GameObject[] spawnGameObjects = GameObject.FindGameObjectsWithTag("enemySpawnPoint");

        for (int i = 0; i < spawnGameObjects.Length; i++)
        {
            spawnLocations.Add(spawnGameObjects[i].transform.position);
        }
    }

    /// <returns>
    /// Returns spawn location based on index.
    /// </returns>
    /// <param name="spawnLocationAt">Index of spawn location.</param>
    public Vector3 GetSpawnLocationOfMonster(int spawnLocationAt)
    {
        Vector3 spawnLocation = spawnLocations[spawnLocationAt];
        spawnLocations.RemoveAt(spawnLocationAt);
        return spawnLocation;
    }

    /// <summary>
    /// returns all spawnable enemy types if less than 4 heavy monsters, otherwise return only non-heavy enemy types
    /// </summary>
    private List<GameObject> getCurrentSpawnableMonsterList (GameObject[] currentMonsters)
    {
        int temp = 0;
        List<GameObject> monsterListWithoutHeavy = new List<GameObject>();
        //count all heavy monsters
        foreach (GameObject enemy in currentMonsters)
        {
            if (enemy.name.Contains("Heavy"))
            {
                temp++;
            }
            monsterListWithoutHeavy.Add(enemy);
        }
        
        if(temp>=4)
        {
            return monsterListWithoutHeavy;
        }
        else
        {
            return monsterList;
        }
    }
}
