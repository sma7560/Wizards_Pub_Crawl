using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;

/// <summary>
/// Holds functions which instantiate announcements from the server on player UI during dungeon play.
/// </summary>
public class AnnouncementManager : NetworkBehaviour
{
    private readonly int secondsToDisplayAnnouncement = 5;  // number of seconds that announcement is displayed for
    private GameObject announcement;                        // GameObject holding everything related to announcements
    private TextMeshProUGUI announcementText;               // holds the text element of the current announcement

    /// <summary>
    /// Initialize variables.
    /// </summary>
    void Start()
    {
        // Instantiate the announcement GameObject
        announcement = Instantiate(Resources.Load("Menu&UI Prefabs/Announcement")) as GameObject;
        announcementText = GameObject.Find("AnnouncementText").GetComponent<TextMeshProUGUI>();
        announcement.SetActive(false);
        DontDestroyOnLoad(announcement);    // Do not destroy announcement object when scene is changed
    }

    /// <summary>
    /// Called every frame.
    /// </summary>
    void Update()
    {
        CheckForPlayerDeaths();
    }

    /// <summary>
    /// Broadcasts that a player has acquired ale.
    /// </summary>
    /// <param name="playerId">Id of the player who has acquired the ale.</param>
    public void BroadcastAnnouncementAleAcquired(int playerId)
    {
        if (!isServer) return;

        LocalBroadcastAnnouncementAleAcquired(playerId);
        RpcBroadcastAnnouncementAleAcquired(playerId);
    }
    private void LocalBroadcastAnnouncementAleAcquired(int playerId)
    {
        announcementText.text = "<color=#" + GetPlayerColourHexCode(playerId) + ">Player " + playerId + "</color> acquired ale!";
        StartCoroutine(DisplayAnnouncement());
    }
    [ClientRpc]
    private void RpcBroadcastAnnouncementAleAcquired(int playerId)
    {
        if (isLocalPlayer) return;
        LocalBroadcastAnnouncementAleAcquired(playerId);
    }

    /// <summary>
    /// Broadcasts that a player has scored ale.
    /// </summary>
    /// <param name="playerId">Id the player who has scored the ale.</param>
    public void BroadcastAnnouncementAleScored(int playerId)
    {
        if (!isServer) return;

        LocalBroadcastAnnouncementAleScored(playerId);
        RpcBroadcastAnnouncementAleScored(playerId);
    }
    private void LocalBroadcastAnnouncementAleScored(int playerId)
    {
        announcementText.text = "<color=#" + GetPlayerColourHexCode(playerId) + ">Player " + playerId + "</color> scored the ale!";
        StartCoroutine(DisplayAnnouncement());
    }
    [ClientRpc]
    private void RpcBroadcastAnnouncementAleScored(int playerId)
    {
        if (isLocalPlayer) return;
        LocalBroadcastAnnouncementAleScored(playerId);
    }

    /// <summary>
    /// Broadcasts that a player has died and dropped the ale.
    /// </summary>
    /// <param name="playerId">Id of the player who has died.</param>
    public void BroadcastAnnouncementAleDropped(int playerId)
    {
        if (!isServer) return;

        LocalBroadcastAnnouncementAleDropped(playerId);
        RpcBroadcastAnnouncementAleDropped(playerId);
    }
    private void LocalBroadcastAnnouncementAleDropped(int playerId)
    {
        announcementText.text = "<color=#" + GetPlayerColourHexCode(playerId) + ">Player " + playerId + "</color> has died and dropped the ale!";
        StartCoroutine(DisplayAnnouncement());
    }
    [ClientRpc]
    private void RpcBroadcastAnnouncementAleDropped(int playerId)
    {
        if (isLocalPlayer) return;
        LocalBroadcastAnnouncementAleDropped(playerId);
    }

    /// <summary>
    /// Broadcasts that a player has died without dropping an ale.
    /// </summary>
    /// <param name="playerId">Id of the player who has died.</param>
    private void BroadcastAnnouncementPlayerDeath(int playerId)
    {
        if (!isServer) return;

        LocalBroadcastAnnouncementPlayerDeath(playerId);
        RpcBroadcastAnnouncementPlayerDeath(playerId);
    }
    private void LocalBroadcastAnnouncementPlayerDeath(int playerId)
    {
        announcementText.text = "<color=#" + GetPlayerColourHexCode(playerId) + ">Player " + playerId + "</color> has died!";
        StartCoroutine(DisplayAnnouncement());
    }
    [ClientRpc]
    private void RpcBroadcastAnnouncementPlayerDeath(int playerId)
    {
        if (isLocalPlayer) return;
        LocalBroadcastAnnouncementPlayerDeath(playerId);
    }

    /// <summary>
    /// Broadcasts that a boss enemy has appeared in the kitchen.
    /// </summary>
    public void BroadcastAnnouncementKitchenBossAppearance()
    {
        if (!isServer) return;

        LocalBroadcastAnnouncementKitchenBossAppearance();
        RpcBroadcastAnnouncementKitchenBossAppearance();
    }
    private void LocalBroadcastAnnouncementKitchenBossAppearance()
    {
        announcementText.text = "A boss monster has appeared in the kitchen!";
        StartCoroutine(DisplayAnnouncement());
    }
    [ClientRpc]
    private void RpcBroadcastAnnouncementKitchenBossAppearance()
    {
        if (isLocalPlayer) return;
        LocalBroadcastAnnouncementKitchenBossAppearance();
    }

    /// <summary>
    /// Coroutine that displays the announcement for a set period of time.
    /// </summary>
    private IEnumerator DisplayAnnouncement()
    {
        announcement.SetActive(true);
        yield return new WaitForSeconds(secondsToDisplayAnnouncement);
        announcement.SetActive(false);
    }

    /// <returns>
    /// Returns the colour of the player in hex code format.
    /// </returns>
    /// <param name="playerId">Id of the player whose colour we are trying to get.</param>
    private string GetPlayerColourHexCode(int playerId)
    {
        Color color = GetComponent<HeroManager>().GetPlayerColour(playerId);
        return ColorUtility.ToHtmlStringRGB(color);
    }

    /// <summary>
    /// Checks if players have died. If so, calls broadcasts the player deaths.
    /// NOTE: WILL BREAK IF deathTimer (in HeroController) > secondsToDisplayAnnouncement !!!
    /// TODO: Figure out better logic for this.
    /// </summary>
    private void CheckForPlayerDeaths()
    {
        GameObject[] playerObjects = GameObject.FindGameObjectsWithTag("Player");

        foreach (GameObject player in playerObjects)
        {
            HeroModel playerModel = player.GetComponent<HeroModel>();
            if (playerModel.IsKnockedOut())
            {
                BroadcastAnnouncementPlayerDeath(playerModel.GetPlayerId());
            }
        }
    }
}
