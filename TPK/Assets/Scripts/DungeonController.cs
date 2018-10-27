using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

// DungeonController: used to update dungeon status during dungeon level gameplay

public class DungeonController : MonoBehaviour {

    public GameObject inGameMenuObject;

    // Use this for initialization
    void Start () {
        Debug.Log("DungeonController script started.");
    }
	
	// Update is called once per frame
	void Update () {

        // Enables in-game menu when Esc key pressed
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            inGameMenuObject.SetActive(true);
        }
    }

    public void QuitMatch()
    {
        Debug.Log("MATCH QUIT");
        SceneManager.LoadScene(0);
    }
}
