using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

/// <summary>
/// Contains information about the game's current status.
/// Ie. number of players in the match, time left in match.
/// </summary>
public class MatchManager : NetworkBehaviour
{
    public readonly int maxPlayers = 1;         // currently only accepting 2 players maximum
    [SyncVar] private int currentNumOfPlayers;  // the current number of players in the match
    private SyncListInt connections;            // list of all connections in the match
    private int playerId;                       // player ID of the current player
    private readonly int totalMatchTime = 900;  // total match time (default to 15 minutes)
    [SyncVar] private float timeLeftMatch;      // time left in the current match
    private bool matchEnded = false;

    /// <summary>
    /// Initialize variables.
    /// </summary>
    void Start()
    {
        if (!isServer) return;          // only the server may initialize MatchManager
        DontDestroyOnLoad(transform);
        timeLeftMatch = totalMatchTime;
    }

    private void Update()
    {
        //end game if match is over
        if((timeLeftMatch == 0)&(!matchEnded))
        {
            matchEnd();
            matchEnded = !matchEnded;
        }
    }

    /// <summary>
    /// If max number of players is not reached, increments the current number of players by 1.
    /// </summary>
    /// <returns>Returns true if a player has been successfully added to the match, else returns false.</returns>
    public bool AddPlayerToMatch(NetworkConnection conn)
    {
        if (!isServer) return false;    // only the server may alter variables

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

    /// <summary>
    /// Decrement the match timer by 1 per second.
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

    public void matchEnd()
    {
        GameObject gameOver = Instantiate(Resources.Load("GameOverScreen")) as GameObject;
    }
}
