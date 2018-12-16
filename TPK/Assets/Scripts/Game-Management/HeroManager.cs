using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Locally keeps track of overall all player's hero status and spawn locations.
/// </summary>
public class HeroManager : MonoBehaviour
{
    private List<Vector3> spawnLocations;   // stores all spawn locations of heroes

    /// <summary>
    /// Initialize variables.
    /// </summary>
    void Start()
    {
        // set initial spawn locations
        spawnLocations = new List<Vector3>();
        AddSpawnLocation(new Vector3(0, 0, 0));         // spawn location of Player 1
        AddSpawnLocation(new Vector3(117f, 0, -42f));   // spawn location of Player 2
    }

    /// <summary>
    /// Returns the spawn location of the requested player.
    /// </summary>
    /// <param name="playerId">Id of the player.</param>
    /// <returns>Returns the spawn location of the player.</returns>
    public Vector3 GetSpawnLocationOfPlayer(int playerId)
    {
        return spawnLocations[playerId - 1];
    }

    /// <summary>
    /// Finds the hero object of the player with the given id.
    /// </summary>
    /// <param name="id">The player's id.</param>
    /// <returns>Returns the hero object associated with the player id.</returns>
    public GameObject GetHeroObject(int id)
    {
        GameObject heroObject = null;
        GameObject[] heroObjects = GameObject.FindGameObjectsWithTag("Player");
        MatchManager matchManager = GameObject.FindGameObjectWithTag("MatchManager").GetComponent<MatchManager>();

        foreach (GameObject hero in heroObjects)
        {
            if (id == matchManager.GetPlayerId())
            {
                heroObject = hero;
                break;
            }
        }

        return heroObject;
    }

    /// <summary>
    /// Adds a new spawn location.
    /// </summary>
    /// <param name="spawnLocation">Spawn location to be added.</param>
    private void AddSpawnLocation(Vector3 spawnLocation)
    {
        spawnLocations.Add(spawnLocation);
    }
}
