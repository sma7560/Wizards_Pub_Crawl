using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

//keeps track of enemies
public class DungeonEnemyManager : NetworkBehaviour
{
    public IUnityService unityService;
    public GameObject lightMonster;
    public GameObject mediumMonster;
    public GameObject heavyMonster;
    private Vector3[] spawnLocation;
    private int currentNumMonsters = 0;

    // Use this for initialization
    void Start()
    {
        if (unityService == null)
        {
            unityService = new UnityService();
        }
        lightMonster = (Resources.Load("Enemies/LightMonster") as GameObject);
        mediumMonster = (Resources.Load("Enemies/RegularMonster") as GameObject);
        heavyMonster = (Resources.Load("Enemies/HeavyMonster") as GameObject);
    }

    public void StartSpawn()
    {
        //set initial spawn locations
        SetSpawnPoints();
        InvokeRepeating("DungeonSpawnMonster", 0f, 5);
    }

    void DungeonSpawnMonster()
    {
        if (currentNumMonsters > 10)
        {
            return;
        }

        int randLocation = Random.Range(0, spawnLocation.Length);
        int randMonster = Random.Range(0, 3);
        //Debug.Log(randLocation);
        CmdSpawnMonster(GetSpawnLocationOfMonster(randLocation), getMonsterType(randMonster));
    }

    // Commands for communicating to the server.
    private void CmdSpawnMonster(Vector3 location, GameObject monsterType)
    {
        if (!isServer)
        {
            return;
        }
        //Debug.Log("Monster spawning");
        Quaternion rotate = Quaternion.Euler(0, 0, 0);
        GameObject temp;
        temp = unityService.Instantiate(monsterType, location, rotate);
        NetworkServer.Spawn(temp);
        //Debug.Log("Monster spawned");
        currentNumMonsters = currentNumMonsters + 1;
    }

    //grab spawn points from spawnlocations on map
    public void SetSpawnPoints()
    {
        GameObject[] spawnGameObjects;
        spawnGameObjects = GameObject.FindGameObjectsWithTag("enemySpawnPoint");
        spawnLocation = new Vector3[spawnGameObjects.Length];
        for(int i=0; i< spawnLocation.Length; i++)
        {
            spawnLocation[i] = spawnGameObjects[i].transform.position;
        }

    }

    //returns spawn location of specified spawn location 
    public Vector3 GetSpawnLocationOfMonster(int spawnLocationAt)
    {
        return spawnLocation[spawnLocationAt];
    }

    //return monster type according to what int is passed in
    public GameObject getMonsterType(int monsterType)
    {
        switch (monsterType)
        {
            case 0:
                {
                    return lightMonster;
                }
            case 1:
                {
                    return mediumMonster;
                }
            case 2:
                {
                    return heavyMonster;
                }
        }
        //should never reach here
        return lightMonster;
    }
}
