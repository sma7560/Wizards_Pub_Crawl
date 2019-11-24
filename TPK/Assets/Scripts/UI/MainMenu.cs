using UnityEngine;

/// <summary>
/// Script for Main Menu functionality.
/// Attached to the Main Menu.
/// </summary>
public class MainMenu : MonoBehaviour
{
    /// <summary>
    /// Initialization.
    /// </summary>
    void Start()
    {

		if (!PlayerPrefs.HasKey ("BgVolume"))
			PlayerPrefs.SetFloat ("BgVolume", 1.0f);

		if (!PlayerPrefs.HasKey ("MasterVolume"))
			PlayerPrefs.SetFloat ("MasterVolume", 1.0f);

		if (PlayerPrefs.HasKey ("BgMute"))
			AudioManager.SetBgMute (PlayerPrefs.GetInt ("BgMute") == 1);

		if (PlayerPrefs.HasKey ("MasterMute"))
			AudioManager.SetMasterMute (PlayerPrefs.GetInt ("MasterMute") == 1);



		AudioManager.SetSystemBgVolume(PlayerPrefs.GetFloat ("BgVolume"));
		AudioManager.SetSystemMasterVolume(PlayerPrefs.GetFloat ("MasterVolume"));
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
