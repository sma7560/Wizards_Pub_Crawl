using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class MatchManager : NetworkBehaviour
{
    private readonly int maxPlayers = 2;    // currently only accepting 2 players maximum
    private int currentNumOfPlayers;        // the current number of players in the match

    // Use this for initialization
    void Start()
    {
        currentNumOfPlayers = 0;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public bool AddPlayerToMatch()
    {
        if (currentNumOfPlayers < maxPlayers)
        {
            currentNumOfPlayers++;
            return true;
        }
        return false;
    }

    public int GetNumOfPlayers()
    {
        return currentNumOfPlayers;
    }
}
