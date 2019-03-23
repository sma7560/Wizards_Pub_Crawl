using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/// <summary>
/// Script for Main Menu functionality.
/// </summary>
public class MainMenuController : MonoBehaviour
{
    void Start()
    {
        GameObject.Find("EventSystem").GetComponent<AudioSource>().volume = AudioManager.GetVolume();
    }

    /// <summary>
    /// Quits the game.
    /// </summary>
    public void QuitGame()
    {
        Debug.Log("QUIT");
        Application.Quit();
    }

    /// <summary>
    /// Disables all buttons while new match is being created.
    /// This reduces the chances of the user clicking the "New Match" button twice, which can cause glitches.
    /// </summary>
    public void CreatingNewMatch()
    {
        // Replace text with "CREATING MATCH..."
        TextMeshProUGUI joinMatchText = GameObject.Find("JoinMatchText").GetComponent<TextMeshProUGUI>();
        joinMatchText.text = "CREATING MATCH...";

        // Disable all other buttons
        GameObject newMatchButton = GameObject.Find("HostButton");
        Button joinMatchButton = GameObject.Find("JoinMatchButton").GetComponent<Button>();
        GameObject backButton = GameObject.Find("BackButton");
        newMatchButton.SetActive(false);
        joinMatchButton.interactable = false;
        EventSystem.current.SetSelectedGameObject(null);
        backButton.SetActive(false);
    }
}
