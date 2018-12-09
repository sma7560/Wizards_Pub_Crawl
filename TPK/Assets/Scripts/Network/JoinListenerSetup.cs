using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// This script is to setting up listeners for the button for Joining game.
public class JoinListenerSetup : MonoBehaviour {

    private GameObject networkManager;
	// Use this for initialization
	void Awake () {
        networkManager = GameObject.Find("NetworkManagerV2");
        // To remove all listeners
        this.GetComponent<Button>().onClick.RemoveAllListeners();

        // To re-add all required listeners.
        this.GetComponent<Button>().onClick.AddListener(networkManager.GetComponent<NetworkManagerExtension>().JoinGame);
    }
}
