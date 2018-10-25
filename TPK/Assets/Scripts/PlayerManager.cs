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
        initialGui = GameObject.Find("InitialGui").GetComponent<Canvas>();
        initialGui.enabled = true;
        // Need to figure out how to set the type up externally
        //myType = PlayerType.Attacker;

        //Instantiate(AttackerObject);
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void SetToAttacker() {
        
        myType = PlayerType.Attacker;
        gameManager = GameObject.Find("GameManager");
        Debug.Log(gameManager.name);

        gameManager.GetComponent<MatchManager>().AddAttacker();
        Debug.Log("Added an Attacker");
        Instantiate(AttackerObject);


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
            Instantiate(DefenderObject);
        }
        Debug.Log("Could not a Defender");

        initialGui = GameObject.Find("InitialGui").GetComponent<Canvas>();
        initialGui.enabled = false;

    }
}
