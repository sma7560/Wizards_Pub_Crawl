using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class MatchManager : MonoBehaviour
{
    public readonly int maxPlayers = 2;    // currently only accepting 2 players maximum
    private int currentNumOfPlayers;        // the current number of players in the match

    /// <summary>
    /// Initialize variables
    /// </summary>
    void Start()
    {
        currentNumOfPlayers = 0;
    }

    /// <summary>
    /// If max number of players is not reached, increments the current number of players by 1.
    /// </summary>
    /// <returns>Returns true if a player has been successfully added to the match, else returns false.</returns>
    public bool AddPlayerToMatch()
    {
        if (currentNumOfPlayers < maxPlayers)
        {
            currentNumOfPlayers++;
            return true;
        }
        return false;
    }

    /// <summary>
    /// If the current number of players is 1 or more, decrements the current number of players by 1.
    /// </summary>
    /// <returns>Returns true if a player has been successfully removed from the match, else returns false.</returns>
    public bool RemovePlayerFromMatch()
    {
        if (currentNumOfPlayers > 1)
        {
            currentNumOfPlayers--;
            return true;
        }
        return false;
    }
    
    /// <returns>Returns the current number of players in the match.</returns>
    public int GetNumOfPlayers()
    {
        return currentNumOfPlayers;
    }
}
