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
        volumePopup = transform.Find("VolumePopup").gameObject;
        volumeText = volumePopup.transform.Find("VolumeText").gameObject.GetComponent<TextMeshProUGUI>();
        volumePopup.SetActive(false);
    }

    /// <summary>
    /// Sets the volume slider to the correct volume level.
    /// </summary>
    void OnEnable()
    {
        Slider volumeSlider = GetComponent<Slider>();

        // Set the slider value
        if (transform.parent.name == "BackgroundMusic")
        {
            volumeSlider.value = AudioManager.GetBgVolumeIgnoreMute();
        }
        else if (transform.parent.name == "SfxMusic")
        {
            volumeSlider.value = AudioManager.GetSfxVolumeIgnoreMute();
        }
        else
        {
            Debug.Log("ERROR: could not recognize audio type of this slider!");
        }

        volumeSlider.onValueChanged.AddListener(delegate { VolumeChange(volumeSlider); });
    }

    /// <summary>
    /// Sets the volume upon volume slider value change.
    /// </summary>
    /// <param name="volumeSlider">Slider that controls the volume levels.</param>
    private void VolumeChange(Slider volumeSlider)
    {
        if (transform.parent.name == "BackgroundMusic")
        {
            AudioManager.SetBgVolume(volumeSlider.value);
        }
        else if (transform.parent.name == "SfxMusic")
        {
            AudioManager.SetSfxVolume(volumeSlider.value);
        }
        else
        {
            Debug.Log("ERROR: could not recognize audio type of this slider!");
        }
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
        if (transform.parent.name == "BackgroundMusic")
        {
            volumeText.text = (int)(AudioManager.GetBgVolumeIgnoreMute() * 100) + "%";
        }
        else if (transform.parent.name == "SfxMusic")
        {
            volumeText.text = (int)(AudioManager.GetSfxVolumeIgnoreMute() * 100) + "%";
        }
        else
        {
            Debug.Log("ERROR: could not recognize audio type of this slider!");
        }

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
        RectTransform handle = transform.Find("HandleSlideArea").Find("Handle").GetComponent<RectTransform>();
        volumePopup.GetComponent<RectTransform>().anchorMin = handle.anchorMin;
        volumePopup.GetComponent<RectTransform>().anchorMax = handle.anchorMax;

        // Set volume popup text
        if (transform.parent.name == "BackgroundMusic")
        {
            volumeText.text = (int)(AudioManager.GetBgVolumeIgnoreMute() * 100) + "%";
        }
        else if (transform.parent.name == "SfxMusic")
        {
            volumeText.text = (int)(AudioManager.GetSfxVolumeIgnoreMute() * 100) + "%";
        }
        else
        {
            Debug.Log("ERROR: could not recognize audio type of this slider!");
        }
    }
}
