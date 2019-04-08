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
    private static float sfxVolume = 1.0f;
    private static bool sfxMute = false;

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
    public static float GetSfxVolume()
    {
        if (sfxMute)
        {
            return 0;
        }

        return sfxVolume;
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
    public static float GetSfxVolumeIgnoreMute()
    {
        return sfxVolume;
    }

    /// <summary>
    /// Sets the background volume to the desired value.
    /// Assumes that AudioSource is always contained in "EventSystem" GameObject.
    /// </summary>
    /// <param name="vol">Desired volume setting.</param>
    public static void SetBgVolume(float vol)
    {
        bgVolume = vol;

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
    public static void SetSfxVolume(float vol)
    {
        sfxVolume = vol;

        if (!sfxMute)
        {
            SetSystemSfxVolume(sfxVolume);
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
        }
        else
        {
            SetSystemBgVolume(bgVolume);
        }
    }

    /// <summary>
    /// Sets whether or not the sound effects audio is muted.
    /// </summary>
    /// <param name="shouldMute">If sound effects audio should be muted or not.</param>
    public static void SetSfxMute(bool shouldMute)
    {
        sfxMute = shouldMute;
        if (sfxMute)
        {
            SetSystemSfxVolume(0f);
        }
        else
        {
            SetSystemSfxVolume(sfxVolume);
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
    public static bool IsSfxMuted()
    {
        return sfxMute;
    }

    /// <summary>
    /// Set actual game background volume.
    /// Assume AudioSource is in "EventSystem" GameObject.
    /// </summary>
    /// <param name="volume">Volume to set the system audio to.</param>
    private static void SetSystemBgVolume(float volume)
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
    /// Set actual game sound effects volume.
    /// Assume AudioSource is in player's GameObject.
    /// </summary>
    /// <param name="volume">Volume to set the system audio to.</param>
    private static void SetSystemSfxVolume(float volume)
    {
        // For main menu sound effects
        if (GameObject.Find("SfxAudio") != null)
        {
            if (sfxMute)
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
            if (sfxMute)
            {
                player.GetComponent<AudioSource>().volume = 0f;
            }
            else
            {
                player.GetComponent<AudioSource>().volume = volume;
            }
        }
    }
}
