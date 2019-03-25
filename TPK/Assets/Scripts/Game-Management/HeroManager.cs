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
        AddSpawnLocation(new Vector3(-55f, 0.65f, 100f));         // spawn location of Player 1
        AddSpawnLocation(new Vector3(-55f, 0.65f, 20f));   // spawn location of Player 2
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

        // Loop through each hero on the scene and check if their id matches the given id
        foreach (GameObject hero in heroObjects)
        {
            int pid = hero.GetComponent<HeroModel>().GetPlayerId();
            if (id == pid)
            {
                heroObject = hero;
                break;
            }
        }

        return heroObject;
    }

    /// <returns>
    /// Returns a list all player transforms.
    /// </returns>
    public Transform[] GetAllPlayerTransforms()
    {
        GameObject[] playerObjects = GameObject.FindGameObjectsWithTag("Player");
        Transform[] targets = new Transform[GetComponent<MatchManager>().GetNumOfPlayers()];

        for (int i = 0; i < GetComponent<MatchManager>().GetNumOfPlayers(); i++)
        {
            targets[i] = playerObjects[i].GetComponent<Transform>();
        }

        return targets;
    }

    /// <returns>
    /// Returns the colour of the player depending on their player id.
    /// </returns>
    public Color GetPlayerColour(int playerId)
    {
        Color heroColour;

        switch (playerId)
        {
            case 1:
                heroColour = Color.blue;
                break;
            case 2:
                heroColour = Color.red;
                break;
            case 3:
                heroColour = Color.green;
                break;
            case 4:
                heroColour = Color.magenta;
                break;
            default:
                heroColour = Color.black;
                break;
        }

        return heroColour;
    }

    /// <returns>
    /// Returns the colour of the player in hex code format.
    /// </returns>
    /// <param name="playerId">Id of the player whose colour we are trying to get.</param>
    public string GetPlayerColourHexCode(int playerId)
    {
        Color color = GetComponent<HeroManager>().GetPlayerColour(playerId);
        return ColorUtility.ToHtmlStringRGB(color);
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
