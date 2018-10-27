using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class InGameMenuController : MonoBehaviour {

    public GameObject dcButton;
    private GameObject networkManager;
	// Use this for initialization
	void Awake () {
        networkManager = GameObject.Find("NetworkManagerV2");
        if (networkManager != null) setupListeners();
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    private void setupListeners() {
        dcButton.GetComponent<Button>().onClick.RemoveAllListeners();
        //dcButton.GetComponent<Button>().onClick.AddListener(networkManager.GetComponent<NetworkManagerExtension>().);
    }
}