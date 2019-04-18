using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

/// <summary>
/// Updates elements in the final scoreboard when the match ends.
/// Attached to the GameOverScreen prefab.
/// Assumes that there are exactly 2 players in the match.
/// </summary>
public class GameOverScreen : NetworkBehaviour
{
    // UI elements
    private Button mainMenu;
    private TextMeshProUGUI player1Score;
    private TextMeshProUGUI player2Score;
    private TextMeshProUGUI playerWin;
    private GameObject[] players;


    private int score1;
    private int score2;

    //used to keep track of whether to play victory, or lose music
    // 1 = player 1, 2= player 2, 0 = tie
    private int winner =-1;     

    void Start()
    {
        // Find UI elements
        mainMenu = GameObject.Find("ReturnMainMenuButton").GetComponent<Button>();
        player1Score = GameObject.Find("Player1Score").GetComponent<TextMeshProUGUI>();
        player2Score = GameObject.Find("Player2Score").GetComponent<TextMeshProUGUI>();
        playerWin = GameObject.Find("PlayerWins").GetComponent<TextMeshProUGUI>();
        players = GameObject.FindGameObjectsWithTag("Player");

        mainMenu.onClick.AddListener(ReturnToMainMenu);

        score1 = -1;
        score2 = -1;

        SetPlayerScores();
        SetPlayerWin();
        SetPlayerWinMusic();
    }

    /// <summary>
    /// Sets player score text.
    /// </summary>
    private void SetPlayerScores()
    {
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
            winner = 1;
        }
        else if (score2 > score1)
        {
            playerWin.text = "Player 2 Wins!";
            winner = 2;
        }
        else
        {
            playerWin.text = "It's a Tie!";
            winner = 0;
        }
    }

    /// <summary>
    /// Set win or lose music
    /// </summary>
    private void SetPlayerWinMusic()
    {
        foreach (GameObject player in players)
        {
            player.GetComponent<PlayerSoundController>().SetWinLoseMusic(winner);
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
