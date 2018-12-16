using UnityEngine;
using UnityEngine.Networking;


// This script disables all uneeded components from the local player.
public class HeroSetup : NetworkBehaviour
{

    [SerializeField]
    Behaviour[] compsToDisable;
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
        else
        {
            if (!isServer)
            {
                CmdAddPlayerToMatch();  // Add player to MatchManager
            }
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    /// <summary>
    /// Runs this command from the client to the server to update number of players in the match.
    /// </summary>
    [Command]
    private void CmdAddPlayerToMatch()
    {
        MatchManager matchManager = GameObject.Find("MatchManager(Clone)").GetComponent<MatchManager>();
        matchManager.AddPlayerToMatch();
    }
}
