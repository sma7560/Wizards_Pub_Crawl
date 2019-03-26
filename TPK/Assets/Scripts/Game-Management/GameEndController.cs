using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameEndController : MonoBehaviour
{
    public Button mainMenu;
    public TextMeshProUGUI player1score;
    public TextMeshProUGUI player2score;
    public TextMeshProUGUI playerWin;

    private DungeonController dungeonController;

    // Use this for initialization
    void Start()
    {
        //winLoseScreen.enabled = false;
        mainMenu.onClick.AddListener(taskReturnMainMenu);

        //get score of all players
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        int score1 = players[0].GetComponent<HeroModel>().GetScore();
        int score2 = players[1].GetComponent<HeroModel>().GetScore();

        player1score.SetText(score1.ToString());
        player2score.SetText(score2.ToString());

        //print which player wins
        if (score1 > score2)
        {
            playerWin.SetText("Player 1 wins");
        }
        else if (score2 > score1)
        {
            playerWin.SetText("Player 2 wins");
        }
        else
        {
            playerWin.SetText("Tie: Neither player wins");
        }
    }

    public void taskReturnMainMenu()
    {
        dungeonController = GameObject.FindGameObjectWithTag("EventSystem").GetComponent<DungeonController>();
        dungeonController.QuitMatch();
    }
}
