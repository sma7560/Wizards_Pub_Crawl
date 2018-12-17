using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameEndController : MonoBehaviour {

    public Canvas winLoseScreen;
    public Button mainMenu;

	// Use this for initialization
	void Start () {
        //winLoseScreen.enabled = false;
        mainMenu.onClick.AddListener(taskReturnMainMenu);
	}

    public void gameEnded()
    {
        winLoseScreen.enabled = true;
    }
	
	public void taskReturnMainMenu()
    {

    }
}
