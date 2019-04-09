using UnityEngine;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/// <summary>
/// This class contains system tests for the Menu screen on the Menu scene.
/// These tests run on Play Mode in Unity's Test Runner.
/// </summary>
public class MenuPlayTest
{
    private readonly string menuSceneName = "Menu";
    private readonly string dungeonSceneName = "DungeonLevel";
    private readonly int timeToWait = 2;    // number of seconds to wait for after menu scene is loaded

    [UnitySetUp]
    public IEnumerator SetUp()
    {
        // Load menu scene
        SceneManager.LoadSceneAsync(menuSceneName, LoadSceneMode.Single);

        // Wait for menu scene to be loaded
        yield return new WaitForSeconds(timeToWait);
    }

    /// <summary>
    /// Checks that the main menu is the only active menu upon initial launch of menu scene.
    /// </summary>
    [UnityTest]
    public IEnumerator Menu_MainMenu_IsActive()
    {
        // Get all menus
        GameObject mainMenu = GameObject.Find("MainMenu");
        GameObject settingsMenu = GameObject.Find("SettingsMenu");
        GameObject startGameMenu = GameObject.Find("StartGameMenu");
        GameObject joinMatchMenu = GameObject.Find("JoinMatchMenu");
        GameObject howToPlayMenu = GameObject.Find("HowToPlayMenu");

        // Assertions; only main menu should be not null (only one active)
        Assert.IsNotNull(mainMenu, "MainMenu is null!");
        Assert.IsNull(settingsMenu, "SettingsMenu is not null when it should be.");
        Assert.IsNull(startGameMenu, "StartGameMenu is not null when it should be.");
        Assert.IsNull(joinMatchMenu, "JoinMatchMenu is not null when it should be.");
        Assert.IsNull(howToPlayMenu, "HowToPlayMenu is not null when it should be.");

        yield return null;
    }

    /// <summary>
    /// Checks that all buttons (Start Game, How to Play, Settings, Quit) are present and active on the main menu.
    /// </summary>
    [UnityTest]
    public IEnumerator Menu_MainMenu_ButtonsAreActive()
    {
        // Get all buttons
        GameObject startGameButton = GameObject.Find("StartGame Button");
        GameObject howToPlayButton = GameObject.Find("HowToPlay Button");
        GameObject settingsButton = GameObject.Find("Settings Button");
        GameObject quitButton = GameObject.Find("Quit Button");

        // Assertions; all buttons should not be null
        Assert.IsNotNull(startGameButton, "StartGame Button is null!");
        Assert.IsNotNull(howToPlayButton, "HowToPlay Button is null!");
        Assert.IsNotNull(settingsButton, "Settings Button is null!");
        Assert.IsNotNull(quitButton, "Quit Button is null!");

        yield return null;
    }

    /// <summary>
    /// Checks functionality of the Start Game button, and that it brings the user to the Start Game Menu.
    /// </summary>
    [UnityTest]
    public IEnumerator Menu_MainMenu_StartGameButtonWorks()
    {
        // Get Start Game button and invoke a click on it
        Button startGameButton = GameObject.Find("StartGame Button").GetComponent<Button>();
        startGameButton.onClick.Invoke();

        // Assert that Start Game Menu is active
        GameObject startGameMenu = GameObject.Find("StartGameMenu");
        Assert.IsNotNull(startGameMenu, "StartGameMenu is null!");

        // Assert that the main menu is no longer active
        GameObject mainMenu = GameObject.Find("MainMenu");
        Assert.IsNull(mainMenu, "MainMenu is not null when it should be.");

        yield return null;
    }

    /// <summary>
    /// Checks functionality of the How to Play button, and that it brings the user to the How to Play Menu.
    /// </summary>
    [UnityTest]
    public IEnumerator Menu_MainMenu_HowToPlayButtonWorks()
    {
        // Get How to Play button and invoke a click on it
        Button howToPlayButton = GameObject.Find("HowToPlay Button").GetComponent<Button>();
        howToPlayButton.onClick.Invoke();

        // Assert that the How to Play Menu is active
        GameObject howToPlayMenu = GameObject.Find("HowToPlayMenu");
        Assert.IsNotNull(howToPlayMenu, "HowToPlayMenu is null!");

        // Assert that the main menu is no longer active
        GameObject mainMenu = GameObject.Find("MainMenu");
        Assert.IsNull(mainMenu, "MainMenu is not null when it should be.");

        yield return null;
    }

    /// <summary>
    /// Checks functionality of the Settings button, and that it brings the user to the Settings Menu.
    /// </summary>
    [UnityTest]
    public IEnumerator Menu_MainMenu_SettingsButtonWorks()
    {
        // Get Settings button and invoke a click on it
        Button settingsButton = GameObject.Find("Settings Button").GetComponent<Button>();
        settingsButton.onClick.Invoke();

        // Assert that the Settings Menu is active
        GameObject settingsMenu = GameObject.Find("SettingsMenu");
        Assert.IsNotNull(settingsMenu, "SettingsMenu is null!");

        // Assert that the main menu is no longer active
        GameObject mainMenu = GameObject.Find("MainMenu");
        Assert.IsNull(mainMenu, "MainMenu is not null when it should be.");

        yield return null;
    }

    /// <summary>
    /// Checks functionality of the Quit button by checking if the Quit button is clickable.
    /// </summary>
    [UnityTest]
    public IEnumerator Menu_MainMenu_QuitButtonWorks()
    {
        // Get Quit button and invoke a click on it
        Button quitButton = GameObject.Find("Quit Button").GetComponent<Button>();
        quitButton.onClick.Invoke();

        // NOTE:
        // Ideally, we would assert if the game is closed here.
        // However, we cannot assert if game is closed in play mode, as Application.Quit() only works on builds and not in play mode.
        // These tests run in play mode, therefore it is not possible to check this condition in automated testing.

        yield return null;
    }

    /// <summary>
    /// Checks that all buttons (New Match, Join Match, Back) are present and active on the Start Game Menu.
    /// </summary>
    [UnityTest]
    public IEnumerator Menu_StartGameMenu_ButtonsAreActive()
    {
        // Get to Start Game Menu
        Button startGameButton = GameObject.Find("StartGame Button").GetComponent<Button>();
        startGameButton.onClick.Invoke();

        // Get all buttons
        GameObject hostButton = GameObject.Find("HostButton");
        GameObject joinMatchButton = GameObject.Find("JoinMatchButton");
        GameObject backButton = GameObject.Find("BackButton");

        // Assertions; all buttons should not be null
        Assert.IsNotNull(hostButton, "HostButton is null!");
        Assert.IsNotNull(joinMatchButton, "JoinMatchButton is null!");
        Assert.IsNotNull(backButton, "BackButton is null!");

        yield return null;
    }

    /// <summary>
    /// Checks functionality of the Back button on the Start Game Menu, and that it brings the user back to the main menu.
    /// </summary>
    [UnityTest]
    public IEnumerator Menu_StartGameMenu_BackButtonWorks()
    {
        // Get to Start Game Menu
        Button startGameButton = GameObject.Find("StartGame Button").GetComponent<Button>();
        startGameButton.onClick.Invoke();

        // Get Back button and invoke a click on it
        Button backButton = GameObject.Find("BackButton").GetComponent<Button>();
        backButton.onClick.Invoke();

        // Assert that the main menu is active
        GameObject mainMenu = GameObject.Find("MainMenu");
        Assert.IsNotNull(mainMenu, "MainMenu is null!");

        // Assert that the Start Game menu is no longer active
        GameObject startGameMenu = GameObject.Find("StartGameMenu");
        Assert.IsNull(startGameMenu, "StartGameMenu is not null when it should be.");

        yield return null;
    }

    /// <summary>
    /// Checks functionality of the New Match button, and that it transitions the user to the next scene (DungeonLevel scene).
    /// </summary>
    [UnityTest]
    public IEnumerator Menu_StartGameMenu_NewMatchButtonWorks()
    {
        // Get to Start Game Menu
        Button startGameButton = GameObject.Find("StartGame Button").GetComponent<Button>();
        startGameButton.onClick.Invoke();

        // Invoke click on New Match button
        Button createDungeonButton = GameObject.Find("HostButton").GetComponent<Button>();
        createDungeonButton.onClick.Invoke();

        // Wait for new scene to be loaded
        yield return new WaitForSeconds(timeToWait);

        // Assert that the current scene is the prephase scene
        Assert.AreEqual(dungeonSceneName, SceneManager.GetActiveScene().name,
            "Scene loaded is not the correct scene. Expected loaded scene is the DungeonLevel scene.");

        yield return null;
    }

    /// <summary>
    /// Checks that all elements (Join Match button, IP Address input field, Back button) are present and active on the Join Match menu.
    /// </summary>
    [UnityTest]
    public IEnumerator Menu_JoinMatchMenu_ElementsAreActive()
    {
        // Get to Join Match Menu
        Button startGameButton = GameObject.Find("StartGame Button").GetComponent<Button>();
        startGameButton.onClick.Invoke();
        Button joinMatchButton = GameObject.Find("JoinMatchButton").GetComponent<Button>();
        joinMatchButton.onClick.Invoke();

        // Get all elements
        Button backButton = GameObject.Find("BackButton").GetComponent<Button>();
        Button joinMatchButton2 = GameObject.Find("JoinMatchButton").GetComponent<Button>();
        InputField ipInput = GameObject.Find("IPInput").GetComponent<InputField>();

        // Assert not null on all elements
        Assert.IsNotNull(backButton, "Back Button is null!");
        Assert.IsNotNull(joinMatchButton2, "JoinMatchButton is null!");
        Assert.IsNotNull(ipInput, "IP InputField is null!");

        yield return null;
    }

    /// <summary>
    /// Checks functionality of the Back button on the Join Match Menu, and that it brings the user back to the Start Game menu.
    /// </summary>
    [UnityTest]
    public IEnumerator Menu_JoinMatchMenu_BackButtonWorks()
    {
        // Get to Join Match Menu
        Button startGameButton = GameObject.Find("StartGame Button").GetComponent<Button>();
        startGameButton.onClick.Invoke();
        Button joinMatchButton = GameObject.Find("JoinMatchButton").GetComponent<Button>();
        joinMatchButton.onClick.Invoke();

        // Get Back button and invoke a click on it
        Button backButton = GameObject.Find("BackButton").GetComponent<Button>();
        backButton.onClick.Invoke();

        // Assert that the Start Game menu is active
        GameObject startGameMenu = GameObject.Find("StartGameMenu");
        Assert.IsNotNull(startGameMenu, "StartGameMenu is null!");

        // Assert that the Join Match menu is no longer active
        GameObject joinMatchMenu = GameObject.Find("JoinMatchMenu");
        Assert.IsNull(joinMatchMenu, "JoinMatchMenu is not null when it should be.");

        yield return null;
    }

    /// <summary>
    /// Checks that all elements (Instructions, Back button) are present and active on the How To Play menu.
    /// </summary>
    [UnityTest]
    public IEnumerator Menu_HowToPlayMenu_ElementsAreActive()
    {
        // Get to How to Play Menu
        Button howToPlayButton = GameObject.Find("HowToPlay Button").GetComponent<Button>();
        howToPlayButton.onClick.Invoke();

        // Get all elements
        Button backButton = GameObject.Find("BackButton").GetComponent<Button>();
        GameObject instructions = GameObject.Find("ObjectiveText");

        // Assert that elements are not null
        Assert.IsNotNull(backButton, "Back button is null!");
        Assert.IsNotNull(instructions, "Instructions are null!");

        yield return null;
    }

    /// <summary>
    /// Checks functionality of the Back button on the How to Play Menu, and that it brings the user back to the main menu.
    /// </summary>
    [UnityTest]
    public IEnumerator Menu_HowToPlayMenu_BackButtonWorks()
    {
        // Get to How to Play Menu
        Button howToPlayButton = GameObject.Find("HowToPlay Button").GetComponent<Button>();
        howToPlayButton.onClick.Invoke();

        // Get Back button and invoke a click on it
        Button backButton = GameObject.Find("BackButton").GetComponent<Button>();
        backButton.onClick.Invoke();

        // Assert that the main menu is active
        GameObject mainMenu = GameObject.Find("MainMenu");
        Assert.IsNotNull(mainMenu, "MainMenu is null!");

        // Assert that the How to Play menu is no longer active
        GameObject howToPlayMenu = GameObject.Find("HowToPlayMenu");
        Assert.IsNull(howToPlayMenu, "HowToPlay menu is not null when it should be.");

        yield return null;
    }

    /// <summary>
    /// Checks that all elements are present and active on the Settings menu.
    /// </summary>
    [UnityTest]
    public IEnumerator Menu_SettingsMenu_ElementsAreActive()
    {
        // Get to Settings Menu
        Button settingsButton = GameObject.Find("Settings Button").GetComponent<Button>();
        settingsButton.onClick.Invoke();

        // Get all elements
        Button backButton = GameObject.Find("BackButton").GetComponent<Button>();
        Button volumeSettings = GameObject.Find("VolumeMenuButton").GetComponent<Button>();
        Button controlsSettings = GameObject.Find("ControlsMenuButton").GetComponent<Button>();

        // Assert that elements are not null
        Assert.IsNotNull(backButton, "Back button is null!");
        Assert.IsNotNull(volumeSettings, "Volume settings button is null!");
        Assert.IsNotNull(controlsSettings, "Controls settings button is null!");

        yield return null;
    }

    /// <summary>
    /// Checks the functionality of the volume slider and that the volume of the game changes when the slider is moved.
    /// </summary>
    [UnityTest]
    public IEnumerator Menu_SettingsMenu_VolumeSliderWorks()
    {
        // Get to Settings Menu
        Button settingsButton = GameObject.Find("Settings Button").GetComponent<Button>();
        settingsButton.onClick.Invoke();

        // Go to the volume settings menu
        Button volumeSettingsButton = GameObject.Find("VolumeMenuButton").GetComponent<Button>();
        volumeSettingsButton.onClick.Invoke();

        // Change volume with volume slider
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
}