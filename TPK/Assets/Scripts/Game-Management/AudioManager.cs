using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Controls the audio of the game.
/// </summary>
public static class AudioManager
{

    public enum Sound
    {
        PlayerAttack,
        move,
        Shooting,

    }
    private static float volume = 1.0f;

    /// <returns>
    /// Returns the volume set by the player.
    /// </returns>
    public static float GetVolume()
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

        // Set actual game volume; assume AudioSource is in "EventSystem" GameObject
        if (GameObject.Find("EventSystem").GetComponent<AudioSource>() != null)
        {
            GameObject.Find("EventSystem").GetComponent<AudioSource>().volume = volume;
        }
    }

    public static void PlaySound(Sound sound)
    {
        GameObject soundGameObject = new GameObject("Sound");
        AudioSource audioSource = soundGameObject.AddComponent<AudioSource>();
        audioSource.PlayOneShot(GetAudioClip(sound));
    }

    private static AudioClip GetAudioClip(Sound sound)
    {
        foreach (SoundAssets.SoundAudioClip soundAudioClip in SoundAssets.i.soundAudioClipArray)
        {
            if (soundAudioClip.sound == sound)
            {
                return soundAudioClip.audioClip;
            }
        }

        Debug.LogError("Sound" + sound + "dont exist");
        return null;
    }
}
