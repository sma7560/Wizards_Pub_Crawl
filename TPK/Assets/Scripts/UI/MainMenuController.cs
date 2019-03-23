using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/// <summary>
/// Script for Main Menu functionality.
/// </summary>
public class MainMenuController : MonoBehaviour
{
    void Start()
    {
        AudioManager.SetSystemVolume(AudioManager.GetVolume());
    }

    /// <summary>
    /// Quits the game.
    /// </summary>
    public void QuitGame()
    {
        Debug.Log("QUIT");
        Application.Quit();
    }
}
