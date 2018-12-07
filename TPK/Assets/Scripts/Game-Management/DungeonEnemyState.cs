using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//keeps track of overall hero status + spawn locations
public class DungeonEnemyState : MonoBehaviour
{

    private Vector3[] spawnLocation;

    // Use this for initialization
    void Start()
    {
        //set initial spawn locations
        Vector3[] initialSpawn = new Vector3[] { new Vector3(0, 0, 0), new Vector3(30, 0, 0) };
        setSpawnLocations(2, initialSpawn);
    }

    // Update is called once per frame
    void Update()
    {

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

    //returns spawn location of specified spawn location 
    public Vector3 getSpawnLocationOfPlayer(int numSpawnLocations)
    {
        return spawnLocation[numSpawnLocations - 1]; // since player starts at 1, not 0
    }
}
