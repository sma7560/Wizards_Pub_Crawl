using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//keeps track of overall hero status + spawn locations
public class HeroState : MonoBehaviour {

    private Vector3[] spawnLocation;

    // Use this for initialization
    void Start () {
        //set initial spawn locations
        Vector3[] initialSpawn = new Vector3[] { new Vector3(0, 0, 0), new Vector3(30, 0, 0) };
        setSpawnLocations(2, initialSpawn);
    }
	
	// Update is called once per frame
	void Update () {
		
	}


    //set spawn location. Takes in number of players, and the spawn
    //location as a vector array of same size as number of players
    public void setSpawnLocations(int numPlayer, Vector3[] locations)
    {
        spawnLocation = new Vector3[numPlayer];
        for (int i = 0; i < numPlayer; i++)
        {
            spawnLocation[i] = locations[i];
        }
    }

    //returns spawn location of player requested
    public Vector3 getSpawnLocationOfPlayer(int playerNum)
    {
        return spawnLocation[playerNum - 1]; // since player starts at 1, not 0
    }
}
