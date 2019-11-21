using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Sets up listeners for "New Match" button.
/// Attached to "HostButton" in Main Menu.
/// </summary>
public class SoloListenerSetUp : MonoBehaviour
{
    void Awake()
    {
        GetComponent<Button>().onClick.RemoveAllListeners();
        GetComponent<Button>().onClick.AddListener(GameObject.Find("NetworkManagerV2").GetComponent<NetworkManagerExtension>().StartUpSolo);
    }
}
