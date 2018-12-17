using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameEndController : MonoBehaviour {

    public Button mainMenu;
    public TextMeshProUGUI player1score;
    public TextMeshProUGUI player2score;
    public TextMeshProUGUI playerWin;

    private DungeonController dungeonController;

	// Use this for initialization
	void Start () {
        //winLoseScreen.enabled = false;
        mainMenu.onClick.AddListener(taskReturnMainMenu);
        //get player 1 and 2 score
        //temp
        int score1 = 50;
        int score2 = 40;
        //*********************

        player1score.SetText(score1.ToString());
        player2score.SetText(score2.ToString());

        //print which player wins
        if(score1 > score2)
        {
            playerWin.SetText("Player 1 wins");
        }
        else
        {
            playerWin.SetText("Player 1 wins");
        }
    }
	
	public void taskReturnMainMenu()
    {
        dungeonController = GameObject.FindGameObjectWithTag("EventSystem").GetComponent<DungeonController>();
        dungeonController.QuitMatch();
    }
}
