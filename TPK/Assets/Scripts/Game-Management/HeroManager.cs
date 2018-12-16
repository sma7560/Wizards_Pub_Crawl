using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Keeps track of overall all player's hero status and spawn locations.
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
    /// Adds a new spawn location.
    /// </summary>
    /// <param name="spawnLocation">Spawn location to be added.</param>
    public void AddSpawnLocation(Vector3 spawnLocation)
    {
        spawnLocations.Add(spawnLocation);
    }

    /// <summary>
    /// Returns the spawn location of the requested player.
    /// </summary>
    /// <param name="heroId">Id of the hero/player.</param>
    /// <returns>Returns the spawn location of the player.</returns>
    public Vector3 GetSpawnLocationOfPlayer(int heroId)
    {
        return spawnLocations[heroId - 1];
    }
}
