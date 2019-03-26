using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Controls the audio of the game.
/// </summary>
public static class AudioManager
{
    private static float volume = 1.0f;
    private static bool mute = false;

    /// <returns>
    /// Returns the current volume of the system.
    /// </returns>
    public static float GetVolume()
    {
        if (mute)
        {
            return 0;
        }

        return volume;
    }

    /// <returns>
    /// Returns the set volume ignoring mute.
    /// </returns>
    public static float GetVolumeIgnoreMute()
    {
        return volume;
    }

    /// <summary>
    /// Sets the volume to the desired value. Assumes that AudioSource is always contained in "EventSystem" GameObject.
    /// </summary>
    /// <param name="vol">Desired volume setting.</param>
    public static void SetVolume(float vol)
    {
        volume = vol;

        if (!mute)
        {
            SetSystemVolume(volume);
        }
    }

    /// <summary>
    /// Sets whether or not the audio is muted.
    /// </summary>
    /// <param name="shouldMute">If audio should be muted or not.</param>
    public static void SetMute(bool shouldMute)
    {
        mute = shouldMute;
        if (mute)
        {
            SetSystemVolume(0f);
        }
        else
        {
            SetSystemVolume(volume);
        }
    }

    /// <returns>
    /// Returns whether or not the audio is muted.
    /// </returns>
    public static bool GetMute()
    {
        return mute;
    }

    /// <summary>
    /// Set actual game volume.
    /// Assume AudioSource is in "EventSystem" GameObject.
    /// </summary>
    /// <param name="volume">Volume to set the system audio to.</param>
    public static void SetSystemVolume(float volume)
    {
        if (GameObject.Find("EventSystem").GetComponent<AudioSource>() != null)
        {
            if (mute)
            {
                GameObject.Find("EventSystem").GetComponent<AudioSource>().volume = 0f;
            }
            else
            {
                GameObject.Find("EventSystem").GetComponent<AudioSource>().volume = volume;
            }
        }
    }
}
