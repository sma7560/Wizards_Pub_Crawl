using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

/// <summary>
/// Holds functions which instantiate announcements from the server on player UI during dungeon play.
/// </summary>
public class AnnouncementManager : NetworkBehaviour
{
    // Sprite icons of the various hero types
    private Sprite king;
    private Sprite rogue;
    private Sprite wizard;
    private Sprite kitchenBoss;

    private enum AnnouncementType
    {
        AleAcquired,
        AleDropped,
        PlayerDeath,
        KitchenBossAppeared
    }

    private readonly int secondsToDisplayAnnouncement = 5;  // number of seconds that announcement is displayed for
    private GameObject announcement;                        // GameObject holding everything related to announcements
    private TextMeshProUGUI announcementText;               // text of the announcement
    private GameObject leftIcon;                            // icon to the left of the announcement
    private GameObject rightIcon;                           // icon to the right of announcement

    [SyncVar] private bool broadcastingAleDropped;      // whether or not a broadcast for ale dropped is currently being announced
    [SyncVar] private bool broadcastingPlayerDeath;     // whether or not a broadcast for player death is currently being announced

    /// <summary>
    /// Initialize variables.
    /// </summary>
    void Start()
    {
        // Instantiate the announcement GameObject on all players
        announcement = Instantiate(Resources.Load("Menu&UI Prefabs/Announcement")) as GameObject;
        DontDestroyOnLoad(announcement);    // Do not destroy announcement object when scene is changed
        announcementText = GameObject.Find("AnnouncementText").GetComponent<TextMeshProUGUI>();
        leftIcon = GameObject.Find("LeftIcon");
        rightIcon = GameObject.Find("RightIcon");
        announcement.SetActive(false);      // Disable all announcements initially

        // Get icon resources
        king = Resources.Load<Sprite>("UI Resources/king");
        rogue = Resources.Load<Sprite>("UI Resources/thief");
        wizard = Resources.Load<Sprite>("UI Resources/mage");
        kitchenBoss = Resources.Load<Sprite>("UI Resources/mug");

        // Initialize booleans
        if (isServer)
        {
            broadcastingAleDropped = false;
            broadcastingPlayerDeath = false;
        }
    }

    /// <summary>
    /// Called every frame.
    /// </summary>
    void Update()
    {
        if (!isServer) return;

        // Only check for player deaths to announce if a death announcement is not already being broadcasted
        if (!broadcastingAleDropped && !broadcastingPlayerDeath)
        {
            CheckForPlayerDeaths();
        }
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
        string colour = GetComponent<HeroManager>().GetPlayerColourHexCode(playerId);
        announcementText.text = "<color=#" + colour + ">Player " + playerId + "</color> acquired ale!";
        SetHeroIcon(playerId);
        leftIcon.GetComponent<RectTransform>().anchoredPosition = new Vector2(-200f, 0f);
        rightIcon.GetComponent<RectTransform>().anchoredPosition = new Vector2(200f, 0f);
        StartCoroutine(DisplayAnnouncement(AnnouncementType.AleAcquired));
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
        string colour = GetComponent<HeroManager>().GetPlayerColourHexCode(playerId);
        announcementText.text = "<color=#" + colour + ">Player " + playerId + "</color> scored the ale!";
        SetHeroIcon(playerId);
        leftIcon.GetComponent<RectTransform>().anchoredPosition = new Vector2(-210f, 0f);
        rightIcon.GetComponent<RectTransform>().anchoredPosition = new Vector2(210f, 0f);
        StartCoroutine(DisplayAnnouncement(AnnouncementType.AleAcquired));
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
        string colour = GetComponent<HeroManager>().GetPlayerColourHexCode(playerId);
        announcementText.text = "<color=#" + colour + ">Player " + playerId + "</color> has died and dropped the ale!";
        SetHeroIcon(playerId);
        leftIcon.GetComponent<RectTransform>().anchoredPosition = new Vector2(-330f, 0f);
        rightIcon.GetComponent<RectTransform>().anchoredPosition = new Vector2(330f, 0f);
        StartCoroutine(DisplayAnnouncement(AnnouncementType.AleDropped));
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
        string colour = GetComponent<HeroManager>().GetPlayerColourHexCode(playerId);
        announcementText.text = "<color=#" + colour + ">Player " + playerId + "</color> has died!";
        SetHeroIcon(playerId);
        leftIcon.GetComponent<RectTransform>().anchoredPosition = new Vector2(-180f, 0f);
        StartCoroutine(DisplayAnnouncement(AnnouncementType.PlayerDeath));
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
        leftIcon.GetComponent<RectTransform>().anchoredPosition = new Vector2(-370f, 0f);
        leftIcon.GetComponent<Image>().sprite = kitchenBoss;
        StartCoroutine(DisplayAnnouncement(AnnouncementType.KitchenBossAppeared));
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
    private IEnumerator DisplayAnnouncement(AnnouncementType type)
    {
        // Set appropriate bools
        if (type == AnnouncementType.AleDropped)
        {
            broadcastingAleDropped = true;
        }
        else if (type == AnnouncementType.PlayerDeath)
        {
            broadcastingPlayerDeath = true;
        }

        // Set active the appropriate icons depending on announcement type
        if (type == AnnouncementType.AleAcquired || type == AnnouncementType.AleDropped)
        {
            Debug.Log("left active, right active");
            leftIcon.SetActive(true);    // hero icon
            rightIcon.SetActive(true);   // ale icon
        }
        else if (type == AnnouncementType.KitchenBossAppeared || type == AnnouncementType.PlayerDeath)
        {
            Debug.Log("left active, right inactive");
            leftIcon.SetActive(true);    // hero/boss icon
            rightIcon.SetActive(false);
        }

        // Display announcement
        announcement.SetActive(true);
        yield return new WaitForSeconds(secondsToDisplayAnnouncement);
        announcement.SetActive(false);

        // Set appropriate bools
        if (type == AnnouncementType.AleDropped)
        {
            broadcastingAleDropped = false;
        }
        else if (type == AnnouncementType.PlayerDeath)
        {
            broadcastingPlayerDeath = false;
        }
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

    /// <summary>
    /// Sets the left icon to an image of the player's hero.
    /// </summary>
    /// <param name="playerId">Id of the player whom we setting the icon image to.</param>
    private void SetHeroIcon(int playerId)
    {
        GameObject player = GetComponent<HeroManager>().GetHeroObject(playerId);
        switch (player.GetComponent<HeroModel>().GetHeroIndex())
        {
            case 0:
                leftIcon.GetComponent<Image>().sprite = king;
                break;
            case 1:
                leftIcon.GetComponent<Image>().sprite = rogue;
                break;
            case 2:
                leftIcon.GetComponent<Image>().sprite = wizard;
                break;
            default:
                leftIcon.GetComponent<Image>().sprite = king;
                break;
        }
    }
}
