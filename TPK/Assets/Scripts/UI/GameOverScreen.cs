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
	private MatchManager MM;

	private bool solo;

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
		MM = GameObject.FindGameObjectWithTag ("MatchManager").GetComponent<MatchManager> ();

        mainMenu.onClick.AddListener(ReturnToMainMenu);

        score1 = -1;
        score2 = -1;

		if (MM.GetMaxPlayers() == 1) {
			solo = true;
			score2 = MM.GetScoreLimitSolo () - 1;	//to compare to the player's score when calculating victory or defeat

			GameObject.Find("Player1").GetComponent<TextMeshProUGUI>().text = "Your Score:";

			GameObject.Find("Player2").SetActive(false);
			GameObject.Find("Player2Score").SetActive(false);
		}

        SetPlayerScores();
        SetPlayerWin();
        SetPlayerWinMusic();
    }

    /// <summary>
    /// Sets player score text.
    /// </summary>
    private void SetPlayerScores()
    {
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
			if (!solo)
				playerWin.text = "Player 1 Wins!";
			else
				playerWin.text = "You Succeeded!";
			
			winner = 1;
        }
        else if (score2 > score1)
        {
			if (!solo) {
				playerWin.text = "Player 2 Wins!";
				winner = 2;
			} else {
				playerWin.text = "You failed to get 5 ales!";
				winner = 0;
			}
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
