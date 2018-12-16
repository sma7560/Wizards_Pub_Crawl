using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class InGameMenuController : MonoBehaviour
{
    public AudioSource audioSource;     // TODO: set this when music is added

    private GameObject optionsMenu;
    private GameObject settingsMenu;
    private GameObject graphicsMenu;
    private GameObject audioMenu;
    private GameObject networkMenu;
    private GameObject quitMenu;

    /// <summary>
    /// Initialize variables when in-game menu is enabled.
    /// </summary>
    void OnEnable()
    {
        // Get all menu options
        optionsMenu = GameObject.Find("OptionsMenu");
        settingsMenu = GameObject.Find("SettingsMenu");
        graphicsMenu = GameObject.Find("GraphicsMenu");
        audioMenu = GameObject.Find("AudioMenu");
        networkMenu = GameObject.Find("NetworkMenu");
        quitMenu = GameObject.Find("QuitMenu");

        // Only options menu is active initially
        settingsMenu.SetActive(false);
        graphicsMenu.SetActive(false);
        audioMenu.SetActive(false);
        networkMenu.SetActive(false);
        quitMenu.SetActive(false);
    }

    /// <summary>
    /// Set all menus active again so on next enable, menus can be found.
    /// </summary>
    void OnDisable()
    {
        optionsMenu.SetActive(true);
        settingsMenu.SetActive(true);
        graphicsMenu.SetActive(true);
        audioMenu.SetActive(true);
        networkMenu.SetActive(true);
        quitMenu.SetActive(true);
    }

    /// <summary>
    /// Updates the listed Network IP address on the Network menu.
    /// </summary>
    public void UpdateNetworkIPAddress()
    {
        TextMeshProUGUI hostIP = GameObject.Find("HostIPAddressText").GetComponent<TextMeshProUGUI>();
        hostIP.text = NetworkManagerExtension.GetLocalIPAddress();
    }

    /// <summary>
    /// Sets the volume slider to the correct volume level on the Audio menu.
    /// </summary>
    public void SetVolumeSlider()
    {
        if (audioSource != null)
        {
            Slider volumeSlider = GameObject.Find("VolumeSlider").GetComponent<Slider>();
            volumeSlider.value = audioSource.volume;
            volumeSlider.onValueChanged.AddListener(delegate { VolumeChange(volumeSlider); });
        }
    }

    /// <summary>
    /// Sets the volume upon volume slider value change.
    /// </summary>
    /// <param name="volumeSlider"></param>
    private void VolumeChange(Slider volumeSlider)
    {
        if (audioSource != null)
        {
            audioSource.volume = volumeSlider.value;
        }
    }
}