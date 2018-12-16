using UnityEngine;
using UnityEngine.Networking;


// This script disables all uneeded components from the local player.
public class HeroSetup : NetworkBehaviour
{

    public int id;  // ID of the hero to identify the player
    [SerializeField] Behaviour[] compsToDisable;

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

        // Set hero's ID to the current num of players connected
        id = GameObject.FindGameObjectWithTag("MatchManager").GetComponent<MatchManager>().GetNumOfPlayers();
    }
}
