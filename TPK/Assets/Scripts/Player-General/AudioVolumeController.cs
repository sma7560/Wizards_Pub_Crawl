using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Script attached to all volume sliders in the game to allow their functionality.
/// </summary>
public class AudioVolumeController : MonoBehaviour
{
    /// <summary>
    /// Sets the volume slider to the correct volume level.
    /// </summary>
    void OnEnable()
    {
        Slider volumeSlider = GetComponent<Slider>();
        volumeSlider.value = AudioManager.GetVolume();
        volumeSlider.onValueChanged.AddListener(delegate { VolumeChange(volumeSlider); });
    }

    /// <summary>
    /// Sets the volume upon volume slider value change.
    /// </summary>
    /// <param name="volumeSlider">Slider that controls the volume levels.</param>
    private void VolumeChange(Slider volumeSlider)
    {
        AudioManager.SetVolume(volumeSlider.value);
    }
}
