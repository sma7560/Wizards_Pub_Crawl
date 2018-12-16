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
        //currentNumOfPlayers = 0;
        DontDestroyOnLoad(transform);
    }

    /// <summary>
    /// If max number of players is not reached, increments the current number of players by 1.
    /// </summary>
    /// <returns>Returns true if a player has been successfully added to the match, else returns false.</returns>
    public bool AddPlayerToMatch()
    {
        Debug.Log("AddPlayerToMatch called");
        Debug.Log("currentNumOfPlayers beginning is " + currentNumOfPlayers);

        if (currentNumOfPlayers < maxPlayers)
        {
            currentNumOfPlayers++;
            Debug.Log("currentNumOfPlayers incremented to " + currentNumOfPlayers);

            if (!isServer)
            {
                CmdAddPlayerToMatch(currentNumOfPlayers);
            }

            // Update pre-phase UI if it is active
            if (GameObject.FindGameObjectWithTag("PrephaseUI") != null)
            {
                GameObject.FindGameObjectWithTag("PrephaseUI").GetComponent<PrephaseUI>().UpdateNumOfPlayers();
            }

            Debug.Log("returning true");
            return true;
        }

        Debug.Log("returning false");
        return false;
    }

    /// <summary>
    /// Run command on client to update value of currentNumOfPlayers on server.
    /// </summary>
    [Command]
    private void CmdAddPlayerToMatch(int numOfPlayers)
    {
        Debug.Log("CmdAddPlayerToMatch() called");
        currentNumOfPlayers = numOfPlayers;
        //RpcAddPlayerToMatch();
    }

    /// <summary>
    /// Run command on server to update value of currentNumOfPlayers on clients.
    /// </summary>
    [ClientRpc]
    private void RpcAddPlayerToMatch(int numOfPlayers)
    {
        Debug.Log("RpcAddPlayerToMatch() called");
        currentNumOfPlayers = numOfPlayers;
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
        Debug.Log("CmdRemovePlayerFromMatch() called");
        currentNumOfPlayers--;
    }
    
    /// <returns>Returns the current number of players in the match.</returns>
    public int GetNumOfPlayers()
    {
        return currentNumOfPlayers;
    }
}
