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

    // USed to in the start of the in game player object.
    void Start()
    {
        islocal = isLocalPlayer;
        if (isLocalPlayer == false) return;
        if (!hasAuthority) return;
    
        // For now spawn where
        CmdSpawnPlayerUnit(this.transform.position);
    }

    // Update is called once per frame
    void Update()
    {
        //This method goes through server via: client -> Server -> client
        //if (!isLocalPlayer) return;
        //if (Input.GetKeyDown(KeyCode.Space)) CmdMoveUnit();
    }

    // external facing function for spawning the player unit.
    public void spawnOnNetwork(Transform position){
        CmdSpawnPlayerUnit(position.position);
    }
    // Commands - Function to be performed on the server by the server.

    private GameObject myAvatar;
    [Command]
    void CmdSpawnPlayerUnit(Vector3 position)
    {
        GameObject clone;
        clone = Instantiate(AttackerObject);
        // This should set the position.
        clone.transform.position = position;
        Debug.Log("Spawing an attacker avatar!");
        myAvatar = clone;
        NetworkServer.Spawn(clone);
    }

    //[Command]
    //void CmdKillPlayerUnity() {

    //}
}
