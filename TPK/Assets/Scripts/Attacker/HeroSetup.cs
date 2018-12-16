using UnityEngine;
using UnityEngine.Networking;


// This script disables all uneeded components from the local player.
public class HeroSetup : NetworkBehaviour
{
    [SerializeField] Behaviour[] compsToDisable;
    private MatchManager matchManager;

    // Use this for initialization
    void Start()
    {
        Debug.Log("Local Player: " + hasAuthority);

        if (!isLocalPlayer)
        {
            for (int i = 0; i < compsToDisable.Length; i++)
            {
                compsToDisable[i].enabled = false;
            }
        }

        // Set the player ID in MatchManager
        matchManager = GameObject.FindGameObjectWithTag("MatchManager").GetComponent<MatchManager>();
        if (hasAuthority)
        {
            if (isServer)
            {
                Debug.Log("connectionToClient = " + connectionToClient);
                matchManager.SetPlayerId(connectionToClient);
            }
            else
            {
                Debug.Log("connectionToServer = " + connectionToServer);
                matchManager.SetPlayerId(connectionToServer);
            }
        }
    }
}
