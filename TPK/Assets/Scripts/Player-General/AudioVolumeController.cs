using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Script attached to all volume sliders in the game to allow their functionality.
/// </summary>
public class AudioVolumeController : MonoBehaviour
{
    private GameObject volumePopup;
    private TextMeshProUGUI volumeText;

    /// <summary>
    /// Initialize variables.
    /// </summary>
    private void Awake()
    {
        volumePopup = GameObject.Find("VolumePopup");
        volumeText = GameObject.Find("VolumeText").GetComponent<TextMeshProUGUI>();
        volumePopup.SetActive(false);
    }

    /// <summary>
    /// Sets the volume slider to the correct volume level.
    /// </summary>
    void OnEnable()
    {
        Slider volumeSlider = GetComponent<Slider>();
        volumeSlider.value = AudioManager.GetVolumeIgnoreMute();
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

    /// <summary>
    /// Called when dragging of slider has ended.
    /// Disables volume with text popup.
    /// </summary>
    public void EndDrag()
    {
        // Disable volume popup
        volumePopup.SetActive(false);
    }

    /// <summary>
    /// Called when dragging of slider begins.
    /// Allows volume with text to popup.
    /// </summary>
    public void BeginDrag()
    {
        // Set position of volume popup
        RectTransform handle = GameObject.Find("Handle").GetComponent<RectTransform>();
        volumePopup.GetComponent<RectTransform>().anchorMin = handle.anchorMin;
        volumePopup.GetComponent<RectTransform>().anchorMax = handle.anchorMax;

        // Set volume popup text
        volumeText.text = (int)(AudioManager.GetVolumeIgnoreMute() * 100) + "%";

        // Enable volume popup
        volumePopup.SetActive(true);
    }

    /// <summary>
    /// Called during dragging of the slider.
    /// Updates the position and text of the volume popup.
    /// </summary>
    public void OnDrag()
    {
        // Set position of volume popup
        RectTransform handle = GameObject.Find("Handle").GetComponent<RectTransform>();
        volumePopup.GetComponent<RectTransform>().anchorMin = handle.anchorMin;
        volumePopup.GetComponent<RectTransform>().anchorMax = handle.anchorMax;

        // Set volume popup text
        volumeText.text = (int)(AudioManager.GetVolumeIgnoreMute() * 100) + "%";
    }
}
