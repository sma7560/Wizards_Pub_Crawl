using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

/// <summary>
/// Performs setup upon a new player joining the game.
/// </summary>
public class HeroSetup : NetworkBehaviour
{
    [SerializeField] Behaviour[] compsToDisable;
    private MatchManager matchManager;

    private bool heroModelsUpdated;     // Used to call UpdateHeroModels() once

    /// <summary>
    /// Use this for initialization.
    /// </summary>
    void Start()
    {
        Debug.Log("Local Player: " + hasAuthority);

        heroModelsUpdated = false;

        DisableComps();
        SetupPlayerId();
    }

    void Update()
    {
        // Call UpdateHeroModels() once PrephaseUI has finished initialization
        if (!heroModelsUpdated &&
            GameObject.FindGameObjectWithTag("PrephaseUI") != null &&
            GameObject.FindGameObjectWithTag("PrephaseUI").GetComponent<PrephaseUI>().GetDefaultHero(0) != null)
        {
            heroModelsUpdated = true;
            UpdateHeroModels();
        }
    }

    /// <summary>
    /// Disables the unneeded components on other hero objects which are not owned/controlled by the player.
    /// </summary>
    private void DisableComps()
    {
        if (!isLocalPlayer)
        {
            for (int i = 0; i < compsToDisable.Length; i++)
            {
                compsToDisable[i].enabled = false;
            }
        }
    }

    /// <summary>
    /// Sets the player ID in MatchManager.
    /// </summary>
    private void SetupPlayerId()
    {
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

    /// <summary>
    /// Locally updates the current hero model displayed to match the correct one saved in HeroModel.
    /// </summary>
    private void UpdateHeroModels()
    {
        GameObject[] playerObjects = GameObject.FindGameObjectsWithTag("Player");
        PrephaseUI prephaseUI = GameObject.FindGameObjectWithTag("PrephaseUI").GetComponent<PrephaseUI>();

        foreach(GameObject player in playerObjects)
        {
            HeroModel heroModel = player.GetComponent<HeroModel>();
            int heroIndex = heroModel.GetHeroIndex();
            heroModel.LocalSetModel(player, prephaseUI.GetDefaultHero(heroIndex).childIndex, prephaseUI.GetDefaultHero(heroIndex).heroType, 0);
        }
    }
}
