using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Behaviour for the audio button to mute/unmute the audio.
/// Attached to the audio button.
/// </summary>
public class AudioButton : MonoBehaviour
{
    private Sprite audioOn;     // resource image of audio on
    private Sprite audioOff;    // resource image of audio off

    private Image image;        // image attached to this button

    /// <summary>
    /// Initialization.
    /// </summary>
    void Awake()
    {
        audioOn = Resources.Load<Sprite>("UI Resources/audio");
        audioOff = Resources.Load<Sprite>("UI Resources/audioOff");
        image = GetComponent<Image>();

        // Set sprite based on mute status
        if (AudioManager.GetMute())
        {
            image.sprite = audioOff;
        }
        else
        {
            image.sprite = audioOn;
        }
    }

    /// <summary>
    /// Behaviour when the button is clicked.
    /// Toggles between muting and unmuting the music.
    /// </summary>
    public void ToggleMute()
    {
        // Toggle mute
        AudioManager.SetMute(!AudioManager.GetMute());

        if (AudioManager.GetMute())
        {
            // Set muted audio sprite
            image.sprite = audioOff;
        }
        else
        {
            // Set unmuted audio sprite
            image.sprite = audioOn;
        }
    }
}
