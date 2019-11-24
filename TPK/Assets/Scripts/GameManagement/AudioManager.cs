using UnityEngine;

/// <summary>
/// Controls the audio of the game.
/// </summary>
public static class AudioManager
{
    // Background volume level
    private static float bgVolume = 1.0f;
    private static bool bgMute = false;

    // Sound effect volume level
    private static float masterVolume = 1.0f;
    private static bool masterMute = false;

    /// <returns>
    /// Returns the current background volume of the system.
    /// </returns>
    public static float GetBgVolume()
    {
        if (bgMute)
        {
            return 0;
        }

        return bgVolume;
    }

    /// <returns>
    /// Returns the current sound effects volume of the system.
    /// </returns>
    public static float GetMasterVolume()
    {
        if (masterMute)
        {
            return 0;
        }

        return masterVolume;
    }

    /// <returns>
    /// Returns the current background volume ignoring mute.
    /// </returns>
    public static float GetBgVolumeIgnoreMute()
    {
        return bgVolume;
    }

    /// <returns>
    /// Returns the current sound effects volume ignoring mute.
    /// </returns>
    public static float GetMasterVolumeIgnoreMute()
    {
        return masterVolume;
    }

    /// <summary>
    /// Sets the background volume to the desired value.
    /// Assumes that AudioSource is always contained in "EventSystem" GameObject.
    /// </summary>
    /// <param name="vol">Desired volume setting.</param>
    public static void SetBgVolume(float vol)
    {
        bgVolume = vol;
		PlayerPrefs.SetFloat ("BgVolume", vol);

        if (!bgMute)
        {
            SetSystemBgVolume(bgVolume);
        }
    }

    /// <summary>
    /// Sets the sound effects volume to the desired value.
    /// Assumes that AudioSource is always contained in this player's GameObject.
    /// </summary>
    /// <param name="vol">Desired volume setting.</param>
    public static void SetMasterVolume(float vol)
    {
        masterVolume = vol;
		PlayerPrefs.SetFloat ("MasterVolume", vol);

        if (!masterMute)
        {
            SetSystemMasterVolume(masterVolume);
        }
    }

    /// <summary>
    /// Sets whether or not the background audio is muted.
    /// </summary>
    /// <param name="shouldMute">If background audio should be muted or not.</param>
    public static void SetBgMute(bool shouldMute)
    {
        bgMute = shouldMute;
        if (bgMute)
        {
            SetSystemBgVolume(0f);
			PlayerPrefs.SetInt ("BgMute", 1);
        }
        else
        {
			SetSystemBgVolume(PlayerPrefs.GetFloat ("BgVolume"));
			PlayerPrefs.SetInt ("BgMute", 0);
        }
    }

    /// <summary>
    /// Sets whether or not the Master Volume is muted.
    /// </summary>
    /// <param name="shouldMute">If sound effects audio should be muted or not.</param>
    public static void SetMasterMute(bool shouldMute)
    {
        masterMute = shouldMute;
        if (masterMute)
        {
            SetSystemMasterVolume(0f);
			PlayerPrefs.SetInt ("MasterMute", 1);
        }
        else
        {
			SetSystemMasterVolume(PlayerPrefs.GetFloat ("MasterVolume"));
			PlayerPrefs.SetInt ("MasterMute", 0);
        }
    }

    /// <returns>
    /// Returns whether or not the background audio is muted.
    /// </returns>
    public static bool IsBgMuted()
    {
		return bgMute;
    }

    /// <returns>
    /// Returns whether or not the sound effects audio is muted.
    /// </returns>
    public static bool IsMasterMuted()
    {
		return masterMute;
    }

    /// <summary>
    /// Set actual game background volume.
    /// Assume AudioSource is in "EventSystem" GameObject.
    /// </summary>
    /// <param name="volume">Volume to set the system audio to.</param>
    public static void SetSystemBgVolume(float volume)
    {
        if (GameObject.Find("EventSystem").GetComponent<AudioSource>() != null)
        {
            if (bgMute)
            {
                GameObject.Find("EventSystem").GetComponent<AudioSource>().volume = 0f;
            }
            else
            {
                GameObject.Find("EventSystem").GetComponent<AudioSource>().volume = volume;
            }
				

        }
    }

    /// <summary>
    /// Set game Master volume.
    /// </summary>
    /// <param name="volume">Volume to set the system audio to.</param>
    public static void SetSystemMasterVolume(float volume)
    {
		/*
        // For main menu sound effects
        if (GameObject.Find("SfxAudio") != null)
        {
            if (masterMute)
            {
                GameObject.Find("SfxAudio").GetComponent<AudioSource>().volume = 0f;
            }
            else
            {
                GameObject.Find("SfxAudio").GetComponent<AudioSource>().volume = volume;
            }
        }

        // For sound effects attached to players
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        foreach (GameObject player in players)
        {
            if (masterMute)
            {
                player.GetComponent<AudioSource>().volume = 0f;
            }
            else
            {
                player.GetComponent<AudioSource>().volume = volume;
            }
        }
		*/

		if (masterMute)
		{
			AudioListener.volume = 0f;
		}
		else
		{
			AudioListener.volume = volume;
		}
    }
}
