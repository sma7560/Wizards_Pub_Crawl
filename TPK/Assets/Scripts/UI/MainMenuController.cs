using UnityEngine;

/// <summary>
/// Script for Main Menu functionality.
/// Attached to the Main Menu.
/// </summary>
public class MainMenuController : MonoBehaviour
{
    /// <summary>
    /// Initialization.
    /// </summary>
    void Start()
    {
        AudioManager.SetSystemBgVolume(AudioManager.GetBgVolume());
    }

    /// <summary>
    /// On-click functionality for the Quit button.
    /// Quits the game.
    /// </summary>
    public void QuitGame()
    {
        Application.Quit();
    }
}
