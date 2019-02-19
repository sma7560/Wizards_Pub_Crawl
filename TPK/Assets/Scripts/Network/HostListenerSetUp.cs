using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// This script is to setting up listeners for the button for hosting game.
public class HostListenerSetUp : MonoBehaviour
{
    private GameObject networkManager;

    void Awake()
    {
        networkManager = GameObject.Find("NetworkManagerV2");

        // To remove all listeners
        this.GetComponent<Button>().onClick.RemoveAllListeners();

        // To re-add all required listeners.
        this.GetComponent<Button>().onClick.AddListener(networkManager.GetComponent<NetworkManagerExtension>().StartUpHost);

    }
}
