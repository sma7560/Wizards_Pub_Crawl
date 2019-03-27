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

    // Monster types
    public GameObject[] regMonsterList;
    private int currentNumMonsters = 0;

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
        GameObject[] currentMonsterList = GameObject.FindGameObjectsWithTag("Enemy");
        currentNumMonsters = currentMonsterList.Length;
    }

    /// <summary>
    /// Starts spawning monsters periodically.
    /// </summary>
    public void StartSpawn()
    {
        if (!isServer) return;
        for(int i =5; i<=5; i++)
        {
            DungeonSpawnMonster();
        }
        InvokeRepeating("DungeonSpawnMonster", 0f, 4);
    }

    private void DungeonSpawnMonster()
    {
        if (!isServer || matchManager.HasMatchEnded()) return;

        if (currentNumMonsters > 16)
        {
            return;
        }

        int randLocation = Random.Range(0, spawnLocations.Count);
        int randMonster = Random.Range(0, 3);
        SpawnMonster(GetSpawnLocationOfMonster(randLocation), GetMonsterType());
    }

    // Commands for communicating to the server.
    private void SpawnMonster(Vector3 location, GameObject monsterType)
    {
        if (!isServer || matchManager.HasMatchEnded()) return;

        //Debug.Log("Monster spawning of type " + monsterType.name);
        Quaternion rotate = Quaternion.Euler(0, 0, 0);
        GameObject temp = unityService.Instantiate(monsterType, location, rotate);
        NetworkServer.Spawn(temp);
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
        Debug.Log(spawnLocations.Count);
        return spawnLocation;
    }

    /// <returns>
    /// Returns the monster type according to what int is passed in.
    /// </returns>
    /// <param name="monsterType">Integer corresponding to the monster type.</param>
    public GameObject GetMonsterType()
    {
        int randMonster = Random.Range(0, regMonsterList.Length);
        return regMonsterList[randMonster];
    }
}
