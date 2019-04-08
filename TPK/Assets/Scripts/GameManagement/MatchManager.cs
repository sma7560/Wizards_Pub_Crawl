using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

/// <summary>
/// Contains information about the current match's status.
/// Ie. number of players in the match, time left in match.
/// </summary>
public class MatchManager : NetworkBehaviour
{
    // Constants
    private readonly int maxPlayers = 2;                // currently only accepting 2 players maximum
    private readonly int totalMatchTime = 30;          // total match time (default to 15 minutes)

    // SyncVars
    [SerializeField] private SyncListInt connections;   // list of all connections in the match
    [SyncVar] private int currentNumOfPlayers;          // the current number of players in the match
    [SyncVar] private float timeLeftMatch;              // time left in the current match

    // Local variables
    private int playerId;                               // player ID of the current player
    private bool matchEnded;                            // if match has ended; needed to ensure EndMatch() is only called once

    /// <summary>
    /// Initialize variables.
    /// </summary>
    void Start()
    {
        if (!isServer) return;          // only the server may initialize MatchManager

        // Initialize variables
        timeLeftMatch = totalMatchTime;
        matchEnded = false;
        currentNumOfPlayers = 1;        // start at 1 to account for the server itself
        DontDestroyOnLoad(transform);   // Do not destroy MatchManager when scene is changed
    }

    private void Update()
    {
        // Locally checks if the match has ended
        if ((timeLeftMatch <= 0) && !matchEnded)
        {
            matchEnded = true;
            EndMatch();
        }
    }

    /// <summary>
    /// If max number of players is not reached, increments the current number of players by 1.
    /// </summary>
    /// <returns>Returns true if a player has been successfully added to the match, else returns false.</returns>
    public bool AddPlayerToMatch(NetworkConnection conn)
    {
        if (!isServer) return false;

        // Add NetworkConnection to connections list
        if (connections == null)
        {
            connections = new SyncListInt();
        }
        connections.Add(conn.connectionId);

        // Increment currentNumOfPlayers counter
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
    public bool RemovePlayerFromMatch(NetworkConnection conn)
    {
        if (!isServer) return false;    // only the server may alter variables

        // Remove NetworkConnection from connections list
        if (connections != null)
        {
            connections.Remove(conn.connectionId);
        }

        // Decrement currentNumOfPlayers counter
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

    /// <summary>
    /// Sets the player's ID locally on each player's MatchManager object.
    /// </summary>
    /// <param name="conn">The player's network connection.</param>
    public void SetPlayerId(NetworkConnection conn)
    {
        if (connections.Contains(conn.connectionId))
        {
            // Set the index of connections as the player ID
            // Note: +1 so that IDs start at 1 instead of 0
            playerId = (connections.IndexOf(conn.connectionId) + 1);
        }
    }

    /// <returns>
    /// Returns the player's ID (where IDs start from 1).
    /// Returns 0 if the player's ID has not yet been set.
    /// </returns>
    public int GetPlayerId()
    {
        return playerId;
    }

    /// <returns>
    /// Returns the time left in the match.
    /// </returns>
    public float GetTimeLeftInMatch()
    {
        return timeLeftMatch;
    }

    /// <returns>
    /// Returns the maximum number of players that can join the match.
    /// </returns>
    public int GetMaxPlayers()
    {
        return maxPlayers;
    }

    /// <returns>
    /// Returns whether or not the match has ended. True if match has ended, false otherwise.
    /// </returns>
    public bool HasMatchEnded()
    {
        return matchEnded;
    }

    /// <summary>
    /// Coroutine to decrement the match timer by 1 per second.
    /// </summary>
    public IEnumerator DecrementMatchTime()
    {
        if (!isServer) yield return null;   // Only server can change value of a SyncVar

        while (timeLeftMatch > 0)
        {
            yield return new WaitForSeconds(1);
            timeLeftMatch--;
        }
    }

    /// <summary>
    /// Called on the server when the match has ended. Instantiates a local scoreboard on server.
    /// </summary>
    private void EndMatch()
    {
        if (!isServer) return;  // only allow server to instantiate the scoreboard

        if (GameObject.FindGameObjectWithTag("GameOver") == null)
        {
            Instantiate(Resources.Load("Menu&UI Prefabs/GameOverScreen"));
        }

        RpcEndMatch();
    }

    /// <summary>
    /// Server calls this method to end match on all clients. Instantiates local scoreboards on all clients.
    /// </summary>
    [ClientRpc]
    private void RpcEndMatch()
    {
        if (GameObject.FindGameObjectWithTag("GameOver") == null)
        {
            Instantiate(Resources.Load("Menu&UI Prefabs/GameOverScreen"));
        }
    }
}
