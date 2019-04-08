using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Updates elements in the final scoreboard when the match ends.
/// Attached to the GameOverScreen prefab.
/// Assumes that there are exactly 2 players in the match.
/// </summary>
public class GameOverScreen : MonoBehaviour
{
    // UI elements
    private Button mainMenu;
    private TextMeshProUGUI player1Score;
    private TextMeshProUGUI player2Score;
    private TextMeshProUGUI playerWin;
    
    private int score1;
    private int score2;

    void Start()
    {
        // Find UI elements
        mainMenu = GameObject.Find("ReturnMainMenuButton").GetComponent<Button>();
        player1Score = GameObject.Find("Player1Score").GetComponent<TextMeshProUGUI>();
        player2Score = GameObject.Find("Player2Score").GetComponent<TextMeshProUGUI>();
        playerWin = GameObject.Find("PlayerWins").GetComponent<TextMeshProUGUI>();

        mainMenu.onClick.AddListener(ReturnToMainMenu);

        score1 = -1;
        score2 = -1;

        SetPlayerScores();
        SetPlayerWin();
    }

    /// <summary>
    /// Sets player score text.
    /// </summary>
    private void SetPlayerScores()
    {
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");

        if (players.Length != 2)
        {
            Debug.Log("ERROR: More/less than two player objects were found!");
        }

        foreach (GameObject player in players)
        {
            if (player.GetComponent<HeroModel>().GetPlayerId() == 1)
            {
                score1 = player.GetComponent<HeroModel>().GetScore();
            }
            else if (player.GetComponent<HeroModel>().GetPlayerId() == 2)
            {
                score2 = player.GetComponent<HeroModel>().GetScore();
            }
        }

        player1Score.SetText(score1.ToString());
        player2Score.SetText(score2.ToString());
    }

    /// <summary>
    /// Displays which player has won the match.
    /// </summary>
    private void SetPlayerWin()
    {
        if (score1 > score2)
        {
            playerWin.text = "Player 1 Wins!";
        }
        else if (score2 > score1)
        {
            playerWin.text = "Player 2 Wins!";
        }
        else
        {
            playerWin.text = "It's a Tie!";
        }
    }

    /// <summary>
    /// Behaviour for the "Main Menu" button.
    /// Disconnects from the match and returns the player to the Main Menu scene.
    /// </summary>
    private void ReturnToMainMenu()
    {
        GameObject.FindGameObjectWithTag("EventSystem").GetComponent<DungeonController>().QuitMatch();
    }
}
