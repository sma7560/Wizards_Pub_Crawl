using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class MatchManager : NetworkBehaviour
{
    public readonly int maxPlayers = 2; // currently only accepting 2 players maximum
    [SyncVar]
    public int currentNumOfPlayers;     // the current number of players in the match

    /// <summary>
    /// Initialize variables.
    /// </summary>
    void Start()
    {
        DontDestroyOnLoad(transform);
    }

    /// <summary>
    /// If max number of players is not reached, increments the current number of players by 1.
    /// </summary>
    /// <returns>Returns true if a player has been successfully added to the match, else returns false.</returns>
    public bool AddPlayerToMatch()
    {
        if (currentNumOfPlayers < maxPlayers)
        {
            CmdAddPlayerToMatch();

            // Update pre-phase UI if it is active
            if (GameObject.FindGameObjectWithTag("PrephaseUI") != null)
            {
                GameObject.FindGameObjectWithTag("PrephaseUI").GetComponent<PrephaseUI>().UpdateNumOfPlayers();
            }

            return true;
        }

        return false;
    }

    /// <summary>
    /// Send command to server to update value of currentNumOfPlayers on server.
    /// </summary>
    [Command]
    private void CmdAddPlayerToMatch()
    {
        currentNumOfPlayers++;
    }

    /// <summary>
    /// If the current number of players is 1 or more, decrements the current number of players by 1.
    /// </summary>
    /// <returns>Returns true if a player has been successfully removed from the match, else returns false.</returns>
    public bool RemovePlayerFromMatch()
    {
        if (currentNumOfPlayers > 1)
        {
            CmdRemovePlayerFromMatch();
            return true;
        }

        return false;
    }

    /// <summary>
    /// Send command to server to update value of currentNumOfPlayers on server.
    /// </summary>
    [Command]
    private void CmdRemovePlayerFromMatch()
    {
        currentNumOfPlayers--;
    }
    
    /// <returns>Returns the current number of players in the match.</returns>
    public int GetNumOfPlayers()
    {
        return currentNumOfPlayers;
    }
}
