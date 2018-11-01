using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerManager : NetworkBehaviour
{
    // TODO -> Learn Synch Variables

    // Enum for denoting player type
    public enum PlayerType { Attacker, Defender };
    public bool islocal;
    private GameObject gameManager;
    private Canvas initialGui;

    public GameObject AttackerObject;
    public GameObject DefenderObject;


    public PlayerType myType;
    public string playerName = "Anonymous";

    // Use this for initialization
    void Start()
    {
        islocal = isLocalPlayer;
        if (isLocalPlayer == false) return;
        if (!hasAuthority) return;
        //Set player type based on who is the server. As there can only be one defender this restricts it.
        if (isServer)
        { 
            myType = PlayerType.Defender;
            Debug.Log("This is my server so I must defend it!");
        }
        else {
            myType = PlayerType.Attacker;
            Debug.Log("This is not my server so I must claim it!");
        }
        CmdSpawnPlayerUnit(myType);
    }

    // Update is called once per frame
    void Update()
    {
        //This method goes through server via: client -> Server -> client
        //if (!isLocalPlayer) return;
        //if (Input.GetKeyDown(KeyCode.Space)) CmdMoveUnit();
    }

    private GameObject defender;
    // Commands - Function to be performed on the server by the server.

    private GameObject myAvatar;
    [Command]
    void CmdSpawnPlayerUnit(PlayerType play)
    {
        GameObject clone;
        if (play == PlayerType.Attacker)
        {
            clone = Instantiate(AttackerObject);
            Debug.Log("Spawing an attacker avatar!");
            myAvatar = clone;
            NetworkServer.SpawnWithClientAuthority(clone, connectionToClient);
        }
        else if (play == PlayerType.Defender) {
            Debug.Log("Spawning for Defender");
            clone = Instantiate(DefenderObject);
            NetworkServer.SpawnWithClientAuthority(clone, connectionToClient);
        }
    }
}
