using UnityEngine;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;
using UnityEngine.SceneManagement;
using NSubstitute;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// This class contains system tests for In-Game Menu on the TestScene.
/// These tests run on Play Mode in Unity's Test Runner.
/// </summary>
public class InGameMenuPlayTest
{
    private readonly int timeToWait = 2;    // number of seconds to wait for after test scene is loaded
    private GameObject inGameMenu = null;

    [UnitySetUp]
    public IEnumerator SetUp()
    {
        // Load Test scene
        SceneManager.LoadSceneAsync("TestScene", LoadSceneMode.Single);

        // Wait for test scene to be loaded
        yield return new WaitForSeconds(timeToWait);

        // Enable necessary components
        inGameMenu = GameObject.Find("DungeonActivate").transform.Find("In-Game Menu").gameObject;
        inGameMenu.SetActive(true);

        // Wait for components to become active
        yield return new WaitForSeconds(timeToWait);
    }

    /// <summary>
    /// Checks that the Options menu is the only menu active upon initial launch of the in-game menu.
    /// </summary>
    [UnityTest]
    public IEnumerator InGameMenu_DisplaysOptionsMenu()
    {
        Assert.IsNotNull(inGameMenu, "In-Game Menu is null!");

        // Get menus
        GameObject optionsMenu = GameObject.Find("OptionsMenu");
        GameObject settingsMenu = GameObject.Find("SettingsMenu");
        GameObject quitMenu = GameObject.Find("QuitMenu");

        // Assert that only the Options menu is not null; all other menus should be null
        Assert.IsNotNull(optionsMenu);
        Assert.IsNull(settingsMenu);
        Assert.IsNull(quitMenu);

        yield return null;
    }

    /// <summary>
    /// Checks that the in-game menu closes when the Resume button is pressed.
    /// </summary>
    [UnityTest]
    public IEnumerator InGameMenu_OptionsMenu_ResumeButtonWorks()
    {
        Assert.IsNotNull(inGameMenu, "In-Game Menu is null!");

        // Select Resume Button
        Button resumeButton = GameObject.Find("ResumeButton").GetComponent<Button>();
        Assert.IsNotNull(resumeButton, "ResumeButton is null!");
        resumeButton.onClick.Invoke();

        // Assert that in-game menu is inactive
        Assert.IsFalse(inGameMenu.activeSelf);

        yield return null;
    }

    /// <summary>
    /// Checks that the Settings menu becomes active when the Settings button is pressed.
    /// </summary>
    [UnityTest]
    public IEnumerator InGameMenu_OptionsMenu_SettingsButtonWorks()
    {
        Assert.IsNotNull(inGameMenu, "In-Game Menu is null!");

        // Select Settings Button
        Button settingsButton = GameObject.Find("SettingsButton").GetComponent<Button>();
        Assert.IsNotNull(settingsButton, "SettingsButton is null!");
        settingsButton.onClick.Invoke();

        // Get menus
        GameObject optionsMenu = GameObject.Find("OptionsMenu");
        GameObject settingsMenu = GameObject.Find("SettingsMenu");

        // Assert that SettingsMenu is not null, and OptionsMenu is now null
        Assert.IsNull(optionsMenu);
        Assert.IsNotNull(settingsMenu);

        yield return null;
    }

    /// <summary>
    /// Checks that the Quit menu becomes active when the Quit button is pressed.
    /// </summary>
    [UnityTest]
    public IEnumerator InGameMenu_OptionsMenu_QuitButtonWorks()
    {
        Assert.IsNotNull(inGameMenu, "In-Game Menu is null!");

        // Select Quit Button
        Button quitButton = GameObject.Find("QuitButton").GetComponent<Button>();
        Assert.IsNotNull(quitButton, "QuitButton is null!");
        quitButton.onClick.Invoke();

        // Get menus
        GameObject optionsMenu = GameObject.Find("OptionsMenu");
        GameObject quitMenu = GameObject.Find("QuitMenu");

        // Assert that QuitMenu is not null, and OptionsMenu is now null
        Assert.IsNull(optionsMenu);
        Assert.IsNotNull(quitMenu);

        yield return null;
    }

    /// <summary>
    /// Checks that the Back button on the Settings menu works, and that it returns to the Options menu.
    /// </summary>
    [UnityTest]
    public IEnumerator InGameMenu_SettingsMenu_BackButtonWorks()
    {
        Assert.IsNotNull(inGameMenu, "In-Game Menu is null!");

        // Get to in-game settings menu
        Button settingsButton = GameObject.Find("SettingsButton").GetComponent<Button>();
        settingsButton.onClick.Invoke();

        // Select Back Button
        Button backButton = GameObject.Find("BackButton").GetComponent<Button>();
        Assert.IsNotNull(backButton, "BackButton is null!");
        backButton.onClick.Invoke();

        // Get menus
        GameObject optionsMenu = GameObject.Find("OptionsMenu");
        GameObject settingsMenu = GameObject.Find("SettingsMenu");

        // Assert that OptionsMenu is not null, and SettingsMenu is null
        Assert.IsNull(settingsMenu);
        Assert.IsNotNull(optionsMenu);

        yield return null;
    }

    /// <summary>
    /// Checks that the No button on the Quit menu returns the user to Options menu.
    /// </summary>
    [UnityTest]
    public IEnumerator InGameMenu_QuitMenu_NoButtonWorks()
    {
        Assert.IsNotNull(inGameMenu, "In-Game Menu is null!");

        // Get to in-game quit menu
        Button quitButton = GameObject.Find("QuitButton").GetComponent<Button>();
        quitButton.onClick.Invoke();

        // Select No Button
        Button noButton = GameObject.Find("NoButton").GetComponent<Button>();
        Assert.IsNotNull(noButton, "NoButton is null!");
        noButton.onClick.Invoke();

        // Get menus
        GameObject optionsMenu = GameObject.Find("OptionsMenu");
        GameObject quitMenu = GameObject.Find("QuitMenu");

        // Assert that QuitMenu is null, and OptionsMenu is not null
        Assert.IsNotNull(optionsMenu);
        Assert.IsNull(quitMenu);

        yield return null;
    }

    /// <summary>
    /// Checks that the Yes button on the Quit menu quits the match.
    /// </summary>
    [UnityTest]
    public IEnumerator InGameMenu_QuitMenu_YesButtonWorks()
    {
        Assert.IsNotNull(inGameMenu, "In-Game Menu is null!");

        // Get to in-game quit menu
        Button quitButton = GameObject.Find("QuitButton").GetComponent<Button>();
        quitButton.onClick.Invoke();

        // Select Yes Button
        Button yesButton = GameObject.Find("YesButton").GetComponent<Button>();
        Assert.IsNotNull(yesButton, "YesButton is null!");
        yesButton.onClick.Invoke();

        // Wait for menu scene to be loaded
        yield return new WaitForSeconds(timeToWait);

        // Assert that QuitMatch function has been called
        DungeonController subDungeonController = Substitute.For<DungeonController>();
        subDungeonController.Received().QuitMatch();

        yield return null;
    }

    /// <summary>
    /// Checks that all element(s) (volume selector) are present and active on the in-game Volume Settings menu.
    /// </summary>
    [UnityTest]
    public IEnumerator InGameMenu_VolumeSettingsMenu_ElementsAreActive()
    {
        Assert.IsNotNull(inGameMenu, "In-Game Menu is null!");

        // Get to the in-game audio menu
        Button settingsButton = GameObject.Find("SettingsButton").GetComponent<Button>();
        settingsButton.onClick.Invoke();
        Button audioButton = GameObject.Find("VolumeSettingsButton").GetComponent<Button>();
        audioButton.onClick.Invoke();

        // Get active elements
        Slider volumeSlider = GameObject.Find("Slider").GetComponent<Slider>();

        // Assert that all elements are not null
        Assert.IsNotNull(volumeSlider, "Volume Slider is null!");

        yield return null;
    }

    /// <summary>
    /// Checks the functionality of the volume slider on the in-game volume settings menu, and that the volume of the game changes as the slider is changed.
    /// </summary>
    [UnityTest]
    public IEnumerator InGameMenu_VolumeSettingsMenu_VolumeSliderWorks()
    {
        Assert.IsNotNull(inGameMenu, "In-Game Menu is null!");

        // Get to the in-game audio menu
        Button settingsButton = GameObject.Find("SettingsButton").GetComponent<Button>();
        settingsButton.onClick.Invoke();
        Button audioButton = GameObject.Find("VolumeSettingsButton").GetComponent<Button>();
        audioButton.onClick.Invoke();

        // Set value of volume Slider
        Slider volumeSlider = GameObject.Find("Slider").GetComponent<Slider>();
        volumeSlider.value = 0.5f;

        // Assert that volume has changed
        Assert.AreEqual(volumeSlider.value, AudioManager.GetBgVolume(), "Volume is not equal to the value set in volume slider!");

        // Check different volume values
        volumeSlider.value = 0f;
        Assert.AreEqual(volumeSlider.value, AudioManager.GetBgVolume(), "Volume is not equal to the value set in volume slider!");
        volumeSlider.value = 1f;
        Assert.AreEqual(volumeSlider.value, AudioManager.GetBgVolume(), "Volume is not equal to the value set in volume slider!");

        yield return null;
    }

    /// <summary>
    /// Checks that the Back button on the Volume Settings menu brings the user back to the Settings menu.
    /// </summary>
    [UnityTest]
    public IEnumerator InGameMenu_VolumeSettingsMenu_BackButtonWorks()
    {
        Assert.IsNotNull(inGameMenu, "In-Game Menu is null!");

        // Get to the in-game graphics menu
        Button settingsButton = GameObject.Find("SettingsButton").GetComponent<Button>();
        settingsButton.onClick.Invoke();
        Button audioButton = GameObject.Find("VolumeSettingsButton").GetComponent<Button>();
        audioButton.onClick.Invoke();

        // Select the Back button
        Button backButton = GameObject.Find("BackButton").GetComponent<Button>();
        backButton.onClick.Invoke();

        // Get menus
        GameObject audioMenu = GameObject.Find("VolumeSettings");
        GameObject settingsMenu = GameObject.Find("Settings");

        // Assert Settings is not null, and VolumeSettings is null
        Assert.IsNotNull(settingsMenu, "Settings Menu is null!");
        Assert.IsNull(audioMenu, "VolumeSettings Menu is not null!");

        yield return null;
    }
}
