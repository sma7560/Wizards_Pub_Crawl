using UnityEngine;
using UnityEngine.Networking;

/// <summary>
/// Performs setup upon a new player joining the game.
/// </summary>
public class HeroSetup : NetworkBehaviour
{
    [SerializeField] private Behaviour[] componentsToDisable;
    private MatchManager matchManager;
    private bool heroModelsUpdated;     // Used to call UpdateHeroModels() once

    /// <summary>
    /// Initialization.
    /// </summary>
    void Start()
    {
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
            for (int i = 0; i < componentsToDisable.Length; i++)
            {
                componentsToDisable[i].enabled = false;
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
                matchManager.SetPlayerId(connectionToClient);
            }
            else
            {
                matchManager.SetPlayerId(connectionToServer);
            }
        }
    }

    /// <summary>
    /// Locally updates the current hero model displayed to match the correct one saved in HeroModel.
    /// NOTE:
    ///   This is needed if Player 1 updates their model before Player 2 has loaded the DungeonLevel scene.
    ///   In this case, Player 2 would have default (king) model loaded for Player 1, as model updates only
    ///   happen upon players changing their selected hero. Therefore, if Player 1 doesn't change their selected hero,
    ///   Player 2 will have the incorrect model loaded for Player 1 (it will be default of king).
    ///   Calling this function will update Player 1's model correctly to their selected hero.
    /// </summary>
    private void UpdateHeroModels()
    {
        GameObject[] playerObjects = GameObject.FindGameObjectsWithTag("Player");
        PrephaseUI prephaseUI = GameObject.FindGameObjectWithTag("PrephaseUI").GetComponent<PrephaseUI>();

        foreach (GameObject player in playerObjects)
        {
            HeroModel heroModel = player.GetComponent<HeroModel>();
            int heroIndex = heroModel.GetHeroIndex();
            heroModel.LocalSetModel(player, prephaseUI.GetDefaultHero(heroIndex).childIndex, prephaseUI.GetDefaultHero(heroIndex).heroType, 0);
        }
    }
}
