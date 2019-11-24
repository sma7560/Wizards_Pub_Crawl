using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Behaviour for the audio button to mute/unmute the audio.
/// Attached to the audio button in volume settings menu.
/// </summary>
[RequireComponent(typeof(Button))]
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
        if (transform.parent.name == "BackgroundMusic")
        {
            if (AudioManager.IsBgMuted())
            {
                image.sprite = audioOff;
            }
            else
            {
                image.sprite = audioOn;
            }
        }
        else if (transform.parent.name == "Master")
        {
            if (AudioManager.IsMasterMuted())
            {
                image.sprite = audioOff;
            }
            else
            {
                image.sprite = audioOn;
            }
        }
        else
        {
            Debug.Log("Error; could not recognize audio type of this slider!");
        }
    }

    /// <summary>
    /// Behaviour when the button is clicked.
    /// Toggles between muting and unmuting the background music.
    /// </summary>
    public void ToggleBgMute()
    {
        // Toggle mute
        AudioManager.SetBgMute(!AudioManager.IsBgMuted());

        // Set sprite based on mute status
        if (AudioManager.IsBgMuted())
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
    /// Toggles between muting and unmuting the sound effects.
    /// </summary>
    public void ToggleMasterMute()
    {
        // Toggle mute
        AudioManager.SetMasterMute(!AudioManager.IsMasterMuted());

        // Set sprite based on mute status
        if (AudioManager.IsMasterMuted())
        {
            image.sprite = audioOff;
        }
        else
        {
            image.sprite = audioOn;
        }
    }
}
