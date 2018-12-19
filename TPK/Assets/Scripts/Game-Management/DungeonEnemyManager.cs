using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

//keeps track of enemies
public class DungeonEnemyManager : NetworkBehaviour
{
    public IUnityService unityService;
    public GameObject monster;
    private Vector3[] spawnLocation;
    private int currentNumMonsters = 0;

    // Use this for initialization
    void Start()
    {
        if (unityService == null)
        {
            unityService = new UnityService();
        }

        //set initial spawn locations
        setSpawnPoints();
    }

    public void startSpawn()
    {
        InvokeRepeating("dungeonSpawnMonster", 0f, 10);
    }

    void dungeonSpawnMonster()
    {
        if (currentNumMonsters > 10)
        {
            return;
        }

        Random random = new Random();
        int randLocation = Random.Range(1, 22);

        CmdSpawnMonster(getSpawnLocationOfMonster(randLocation));
    }

    // Commands for communicating to the server.
    private void CmdSpawnMonster(Vector3 location)
    {
        if (!isServer)
        {
            return;
        }
        Debug.Log("Monster spawning");
        Quaternion rotate = Quaternion.Euler(0, 0, 0);
        GameObject temp;
        temp = unityService.Instantiate(monster, location, rotate);
        NetworkServer.Spawn(temp);
        Debug.Log("Monster spawned");
        currentNumMonsters = currentNumMonsters + 1;
    }

    //set spawn location. Takes in number of spawn location, and the spawn
    //location as a n array of vector
    public void setSpawnLocations(int numSpawnLocations, Vector3[] locations)
    {
        spawnLocation = new Vector3[numSpawnLocations];
        for (int i = 0; i < numSpawnLocations; i++)
        {
            spawnLocation[i] = locations[i];
        }
    }

    //change specific spawn location
    public void setSpawnLocation(int playerNum, Vector3 location)
    {
         spawnLocation[playerNum] = location;
    }

    //sets predetermined spawn points
    public void setSpawnPoints()
    {
        spawnLocation = new Vector3[] {
            new Vector3(28, 0.5f, 11),
            new Vector3(52, 0.5f, 11),
            new Vector3(52, 0.5f, -9),
            new Vector3(27, 0.5f, -9),
            new Vector3(81, 0.5f, -25),
            new Vector3(93, 0.5f, -1),
            new Vector3(102, 0.5f, -1),
            new Vector3(2, 0.5f, -37),
            new Vector3(2, 0.5f, 39),
            new Vector3(55, 0.5f, 26),
            new Vector3(70, 0.5f, 26),
            new Vector3(70, 0.5f, 55),
            new Vector3(55, 0.5f, 55),
            new Vector3(95, 0.5f, 41),
            new Vector3(132, 0.5f, 12),
            new Vector3(132, 0.5f, -10),
            new Vector3(160, 0.5f, -10),
            new Vector3(160, 0.5f, 12),
            new Vector3(184, 0.5f, 12),
            new Vector3(184, 0.5f, -10),
            new Vector3(118, 0.5f, -42),
            new Vector3(94, 0.5f, -60),
            new Vector3(55, 0.5f, -54)
        };
    }

    //returns spawn location of specified spawn location 
    public Vector3 getSpawnLocationOfMonster(int spawnLocationAt)
    {
        return spawnLocation[spawnLocationAt];
    }
}
