using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Attached to the JoinMatch button.
/// Sets up the listener for joining the game.
/// </summary>
public class JoinListenerSetup : MonoBehaviour
{
    private GameObject networkManager;

    /// <summary>
    /// Initialization.
    /// </summary>
    void Awake()
    {
        networkManager = GameObject.Find("NetworkManagerV2");
        // To remove all listeners
        this.GetComponent<Button>().onClick.RemoveAllListeners();

        // To re-add all required listeners.
        this.GetComponent<Button>().onClick.AddListener(networkManager.GetComponent<NetworkManagerExtension>().JoinGame);
    }

    /// <summary>
    /// Called once per frame.
    /// </summary>
    void Update()
    {
        // Listens for enter key press to join match
        if (this.GetComponent<Button>().interactable && (Input.GetKeyDown(KeyCode.KeypadEnter) || Input.GetKeyDown(KeyCode.Return)))
        {
            networkManager.GetComponent<NetworkManagerExtension>().JoinGame();
        }
    }
}
