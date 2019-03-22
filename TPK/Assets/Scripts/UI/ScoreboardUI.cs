using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Attached to the scoreboard. Sets the values in the scoreboard.
/// This assumes that there are exactly 2 players - Player 1 and Player 2, otherwise the scoreboard will not be shown.
/// </summary>
public class ScoreboardUI : MonoBehaviour
{
    // Icons
    private Sprite king;
    private Sprite rogue;
    private Sprite wizard;

    // Managers
    private HeroManager heroManager;
    private MatchManager matchManager;

    // UI elements
    private TextMeshProUGUI player1Name;
    private TextMeshProUGUI player2Name;
    private TextMeshProUGUI player1Score;
    private TextMeshProUGUI player2Score;
    private Image player1Icon;
    private Image player2Icon;

    /// <summary>
    /// Initialize variables.
    /// Note: Awake->Enable->Start
    /// </summary>
    void Awake()
    {
        // Initialize managers
        matchManager = GameObject.FindGameObjectWithTag("MatchManager").GetComponent<MatchManager>();
        heroManager = GameObject.FindGameObjectWithTag("MatchManager").GetComponent<HeroManager>();

        // Get scoreboard UI elements
        player1Name = GameObject.Find("Player1Text").GetComponent<TextMeshProUGUI>();
        player2Name = GameObject.Find("Player2Text").GetComponent<TextMeshProUGUI>();
        player1Score = GameObject.Find("Player1ScoreText").GetComponent<TextMeshProUGUI>();
        player2Score = GameObject.Find("Player2ScoreText").GetComponent<TextMeshProUGUI>();
        player1Icon = GameObject.Find("Player1Icon").GetComponent<Image>();
        player2Icon = GameObject.Find("Player2Icon").GetComponent<Image>();

        // Get icon resources
        king = Resources.Load<Sprite>("UI Resources/king");
        rogue = Resources.Load<Sprite>("UI Resources/thief");
        wizard = Resources.Load<Sprite>("UI Resources/mage");
    }

    /// <summary>
    /// Called whenever scoreboard is opened by the player.
    /// Checks that there are exactly 2 players - Player 1 and Player 2; otherwise, does not open.
    /// Sets the values in the scoreboard.
    /// </summary>
    void OnEnable()
    {
        // First check that there are exactly 2 players in the match
        if (matchManager.GetMaxPlayers() != 2 || matchManager.GetNumOfPlayers() != matchManager.GetMaxPlayers())
        {
            gameObject.SetActive(false);
            return;
        }

        // Player objects
        GameObject[] playerObjects = GameObject.FindGameObjectsWithTag("Player");
        GameObject player1 = null, player2 = null;

        // Check that there are exactly 2 player objects
        if (playerObjects.Length != 2)
        {
            gameObject.SetActive(false);
            return;
        }

        // Get the player objects
        foreach (GameObject player in playerObjects)
        {
            if (player.GetComponent<HeroModel>().GetPlayerId() == 1)
            {
                player1 = player;
            }
            else if (player.GetComponent<HeroModel>().GetPlayerId() == 2)
            {
                player2 = player;
            }
        }

        // Check that both player objects are set properly
        if (player1 == null || player2 == null)
        {
            gameObject.SetActive(false);
            return;
        }

        // Set the player name text and colour
        player1Name.text = "<color=#" + heroManager.GetPlayerColourHexCode(1) + ">Player 1</color>";
        player2Name.text = "<color=#" + heroManager.GetPlayerColourHexCode(2) + ">Player 2</color>";

        // Set the player scores
        player1Score.text = player1.GetComponent<HeroModel>().GetScore().ToString();
        player2Score.text = player2.GetComponent<HeroModel>().GetScore().ToString();

        // Set the player icons
        switch (player1.GetComponent<HeroModel>().GetHeroIndex())
        {
            case 0:
                player1Icon.sprite = king;
                break;
            case 1:
                player1Icon.sprite = rogue;
                break;
            case 2:
                player1Icon.sprite = wizard;
                break;
            default:
                player1Icon.sprite = king;
                break;
        }

        switch (player2.GetComponent<HeroModel>().GetHeroIndex())
        {
            case 0:
                player2Icon.sprite = king;
                break;
            case 1:
                player2Icon.sprite = rogue;
                break;
            case 2:
                player2Icon.sprite = wizard;
                break;
            default:
                player2Icon.sprite = king;
                break;
        }
    }
}
