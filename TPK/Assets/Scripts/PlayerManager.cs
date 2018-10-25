using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerManager : NetworkBehaviour {
    // Enum for denoting player type
    public enum PlayerType { Attacker, Defender};

    private GameObject gameManager;
    private Canvas initialGui;

    public GameObject AttackerObject;
    public GameObject DefenderObject;
    public PlayerType myType;


	// Use this for initialization
	void Start () {
        if (isLocalPlayer == false) return;
        initialGui = GameObject.Find("InitialGui").GetComponent<Canvas>();
        initialGui.enabled = true;
        // Need to figure out how to set the type up externally
        //myType = PlayerType.Attacker;

        //Instantiate(AttackerObject);
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    // Commands - Function to be performed on the server.
    [Command]
    public void spawnPlayer() {
           
    }

    public void SetToAttacker() {
        
        myType = PlayerType.Attacker;
        gameManager = GameObject.Find("GameManager");
        Debug.Log(gameManager.name);

        gameManager.GetComponent<MatchManager>().AddAttacker();
        Debug.Log("Added an Attacker");
        GameObject avatar = Instantiate(AttackerObject) as GameObject;
        avatar.transform.SetParent(transform);


        initialGui = GameObject.Find("InitialGui").GetComponent<Canvas>();
        initialGui.enabled = false;

    }

    public void SetToDefender()
    {

        myType = PlayerType.Defender;
        gameManager = GameObject.Find("GameManager");
        Debug.Log(gameManager.name);


        if (gameManager.GetComponent<MatchManager>().AddDefender()) {
            Debug.Log("Added a Defender");
            GameObject avatar = Instantiate(DefenderObject) as GameObject;
            avatar.transform.SetParent(transform);
        }
        Debug.Log("Could not a Defender");

        initialGui = GameObject.Find("InitialGui").GetComponent<Canvas>();
        initialGui.enabled = false;

    }
}
