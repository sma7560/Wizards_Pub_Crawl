using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Sets up the listener for joining the game.
/// Attached to the "Join Match" button in Main Menu.
/// </summary>
public class JoinListenerSetup : MonoBehaviour
{
    private GameObject networkManager;

    void Awake()
    {
        networkManager = GameObject.Find("NetworkManagerV2");

        GetComponent<Button>().onClick.RemoveAllListeners();
        GetComponent<Button>().onClick.AddListener(networkManager.GetComponent<NetworkManagerExtension>().JoinGame);
    }

    void Update()
    {
        // Listens for enter key press to join match
        if (GetComponent<Button>().interactable &&
            (Input.GetKeyDown(KeyCode.KeypadEnter) ||
             Input.GetKeyDown(KeyCode.Return)))
        {
            networkManager.GetComponent<NetworkManagerExtension>().JoinGame();
        }
    }
}
