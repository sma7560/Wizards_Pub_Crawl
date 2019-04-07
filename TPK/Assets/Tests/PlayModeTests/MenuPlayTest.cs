//using UnityEngine;
//using UnityEngine.TestTools;
//using NUnit.Framework;
//using System.Collections;
//using UnityEngine.SceneManagement;
//using UnityEngine.UI;

///// <summary>
///// This class contains system tests for the Menu screen on the Menu scene.
///// These tests run on Play Mode in Unity's Test Runner.
///// </summary>
//public class MenuPlayTest
//{

//    readonly string menuSceneName = "Menu";
//    readonly string prephaseSceneName = "Prephase";
//    readonly int timeToWait = 2;    // number of seconds to wait for after menu scene is loaded

//    [UnitySetUp]
//    public IEnumerator SetUp()
//    {
//        // Load menu scene
//        SceneManager.LoadSceneAsync(menuSceneName, LoadSceneMode.Single);

//        // Wait for menu scene to be loaded
//        yield return new WaitForSeconds(timeToWait);
//    }

//    /// <summary>
//    /// Test ST-M1: Checks that the main menu is the only active menu upon initial launch of menu scene
//    /// Requirement: FR-M1
//    /// </summary>
//    [UnityTest]
//    public IEnumerator Menu_MainMenu_IsActive()
//    {
//        // Get all menus
//        GameObject mainMenu = GameObject.Find("MainMenu");
//        GameObject settingsMenu = GameObject.Find("SettingsMenu");
//        GameObject startGameMenu = GameObject.Find("StartGameMenu");
//        GameObject newMatchMenu = GameObject.Find("NewMatchMenu");
//        GameObject joinMatchMenu = GameObject.Find("JoinMatchMenu");
//        GameObject howToPlayMenu = GameObject.Find("HowToPlayMenu");

//        // Assertions; only main menu should be not null (only one active)
//        Assert.IsNotNull(mainMenu, "MainMenu is null!");
//        Assert.IsNull(settingsMenu, "SettingsMenu is not null when it should be.");
//        Assert.IsNull(startGameMenu, "StartGameMenu is not null when it should be.");
//        Assert.IsNull(newMatchMenu, "NewMatchMenu is not null when it should be.");
//        Assert.IsNull(joinMatchMenu, "JoinMatchMenu is not null when it should be.");
//        Assert.IsNull(howToPlayMenu, "HowToPlayMenu is not null when it should be.");

//        yield return null;
//    }

//    /// <summary>
//    /// Test ST-M2: Checks that all buttons (Start Game, How to Play, Settings, Quit) are present and active on the main menu.
//    /// Requirement: FR-M1
//    /// </summary>
//    [UnityTest]
//    public IEnumerator Menu_MainMenu_ButtonsAreActive()
//    {
//        // Get all buttons
//        GameObject startGameButton = GameObject.Find("StartGame Button");
//        GameObject howToPlayButton = GameObject.Find("HowToPlay Button");
//        GameObject settingsButton = GameObject.Find("Settings Button");
//        GameObject quitButton = GameObject.Find("Quit Button");

//        // Assertions; all buttons should not be null
//        Assert.IsNotNull(startGameButton, "StartGame Button is null!");
//        Assert.IsNotNull(howToPlayButton, "HowToPlay Button is null!");
//        Assert.IsNotNull(settingsButton, "Settings Button is null!");
//        Assert.IsNotNull(quitButton, "Quit Button is null!");

//        yield return null;
//    }

//    /// <summary>
//    /// Test ST-M3: Checks functionality of the Start Game button, and that it brings the user to the Start Game Menu.
//    /// Requirement: FR-M2
//    /// </summary>
//    [UnityTest]
//    public IEnumerator Menu_MainMenu_StartGameButtonWorks()
//    {
//        // Get Start Game button and invoke a click on it
//        Button startGameButton = GameObject.Find("StartGame Button").GetComponent<Button>();
//        startGameButton.onClick.Invoke();

//        // Assert that Start Game Menu is active
//        GameObject startGameMenu = GameObject.Find("StartGameMenu");
//        Assert.IsNotNull(startGameMenu, "StartGameMenu is null!");

//        // Assert that the main menu is no longer active
//        GameObject mainMenu = GameObject.Find("MainMenu");
//        Assert.IsNull(mainMenu, "MainMenu is not null when it should be.");

//        yield return null;
//    }

//    /// <summary>
//    /// Test ST-M4: Checks functionality of the How to Play button, and that it brings the user to the How to Play Menu.
//    /// Requirement: FR-M8
//    /// </summary>
//    [UnityTest]
//    public IEnumerator Menu_MainMenu_HowToPlayButtonWorks()
//    {
//        // Get How to Play button and invoke a click on it
//        Button howToPlayButton = GameObject.Find("HowToPlay Button").GetComponent<Button>();
//        howToPlayButton.onClick.Invoke();

//        // Assert that the How to Play Menu is active
//        GameObject howToPlayMenu = GameObject.Find("HowToPlayMenu");
//        Assert.IsNotNull(howToPlayMenu, "HowToPlayMenu is null!");

//        // Assert that the main menu is no longer active
//        GameObject mainMenu = GameObject.Find("MainMenu");
//        Assert.IsNull(mainMenu, "MainMenu is not null when it should be.");

//        yield return null;
//    }

//    /// <summary>
//    /// Test ST-M5: Checks functionality of the Settings button, and that it brings the user to the Settings Menu.
//    /// Requirement: FR-M5
//    /// </summary>
//    [UnityTest]
//    public IEnumerator Menu_MainMenu_SettingsButtonWorks()
//    {
//        // Get Settings button and invoke a click on it
//        Button settingsButton = GameObject.Find("Settings Button").GetComponent<Button>();
//        settingsButton.onClick.Invoke();

//        // Assert that the Settings Menu is active
//        GameObject settingsMenu = GameObject.Find("SettingsMenu");
//        Assert.IsNotNull(settingsMenu, "SettingsMenu is null!");

//        // Assert that the main menu is no longer active
//        GameObject mainMenu = GameObject.Find("MainMenu");
//        Assert.IsNull(mainMenu, "MainMenu is not null when it should be.");

//        yield return null;
//    }

//    /// <summary>
//    /// Test ST-M6: Checks functionality of the Quit button by checking if the Quit button is clickable.
//    /// Requirement: FR-M9
//    /// </summary>
//    [UnityTest]
//    public IEnumerator Menu_MainMenu_QuitButtonWorks()
//    {
//        // Get Quit button and invoke a click on it
//        Button quitButton = GameObject.Find("Quit Button").GetComponent<Button>();
//        quitButton.onClick.Invoke();

//        // NOTE:
//        // Ideally, we would assert if the game is closed here.
//        // However, we cannot assert if game is closed in play mode, as Application.Quit() only works on builds and not in play mode.
//        // These tests run in play mode, therefore it is not possible to check this condition in automated testing.

//        yield return null;
//    }

//    /// <summary>
//    /// Test ST-M7: Checks that all buttons (New Match, Join Match, Back) are present and active on the Start Game Menu.
//    /// Requirement: FR-M2
//    /// </summary>
//    [UnityTest]
//    public IEnumerator Menu_StartGameMenu_ButtonsAreActive()
//    {
//        // Get to Start Game Menu
//        Button startGameButton = GameObject.Find("StartGame Button").GetComponent<Button>();
//        startGameButton.onClick.Invoke();

//        // Get all buttons
//        GameObject newMatchButton = GameObject.Find("NewMatch Button");
//        GameObject joinMatchButton = GameObject.Find("JoinMatch Button");
//        GameObject backButton = GameObject.Find("Back Button");

//        // Assertions; all buttons should not be null
//        Assert.IsNotNull(newMatchButton, "NewMatch Button is null!");
//        Assert.IsNotNull(joinMatchButton, "JoinMatch Button is null!");
//        Assert.IsNotNull(backButton, "Back Button is null!");

//        yield return null;
//    }

//    /// <summary>
//    /// Test ST-M8: Checks functionality of the Back button on the Start Game Menu, and that it brings the user back to the main menu.
//    /// Requirement: FR-M10
//    /// </summary>
//    [UnityTest]
//    public IEnumerator Menu_StartGameMenu_BackButtonWorks()
//    {
//        // Get to Start Game Menu
//        Button startGameButton = GameObject.Find("StartGame Button").GetComponent<Button>();
//        startGameButton.onClick.Invoke();

//        // Get Back button and invoke a click on it
//        Button backButton = GameObject.Find("Back Button").GetComponent<Button>();
//        backButton.onClick.Invoke();

//        // Assert that the main menu is active
//        GameObject mainMenu = GameObject.Find("MainMenu");
//        Assert.IsNotNull(mainMenu, "MainMenu is null!");

//        // Assert that the Start Game menu is no longer active
//        GameObject startGameMenu = GameObject.Find("StartGameMenu");
//        Assert.IsNull(startGameMenu, "StartGameMenu is not null when it should be.");

//        yield return null;
//    }

//    /// <summary>
//    /// Test ST-M9: Checks functionality of the New Match button, and that it transitions the user to the next scene (prephase scene).
//    /// Requirement: FR-M3
//    /// </summary>
//    [UnityTest]
//    public IEnumerator Menu_StartGameMenu_NewMatchButtonWorks()
//    {
//        // Get to Start Game Menu
//        Button startGameButton = GameObject.Find("StartGame Button").GetComponent<Button>();
//        startGameButton.onClick.Invoke();

//        // Invoke click on New Match Button
//        Button newMatchButton = GameObject.Find("NewMatch Button").GetComponent<Button>();
//        newMatchButton.onClick.Invoke();

//        // Assert that New Match Menu is active
//        GameObject newMatchMenu = GameObject.Find("NewMatchMenu");
//        Assert.IsNotNull(newMatchMenu, "NewMatchMenu is null!");

//        // Invoke click on Create Dungeon button
//        Button createDungeonButton = GameObject.Find("HostButton").GetComponent<Button>();
//        createDungeonButton.onClick.Invoke();

//        // Wait for new scene to be loaded
//        yield return new WaitForSeconds(timeToWait);

//        // Assert that the current scene is the prephase scene
//        Assert.AreEqual(prephaseSceneName, SceneManager.GetActiveScene().name,
//            "Scene loaded is not the correct scene. Expected loaded scene is the Prephase scene.");

//        yield return null;
//    }

//    /// <summary>
//    /// Test ST-M10: Checks functionality of the Back button on the New Match Menu, and that it brings the user back to the Start Game menu.
//    /// Requirement: FR-M10
//    /// </summary>
//    [UnityTest]
//    public IEnumerator Menu_NewMatchMenu_BackButtonWorks()
//    {
//        // Get to New Match Menu
//        Button startGameButton = GameObject.Find("StartGame Button").GetComponent<Button>();
//        startGameButton.onClick.Invoke();
//        Button newMatchButton = GameObject.Find("NewMatch Button").GetComponent<Button>();
//        newMatchButton.onClick.Invoke();

//        // Get Back button and invoke a click on it
//        Button backButton = GameObject.Find("Back Button").GetComponent<Button>();
//        backButton.onClick.Invoke();

//        // Assert that the main menu is active
//        GameObject startGameMenu = GameObject.Find("StartGameMenu");
//        Assert.IsNotNull(startGameMenu, "StartGameMenu is null!");

//        // Assert that the Start Game menu is no longer active
//        GameObject newMatchMenu = GameObject.Find("NewMatchMenu");
//        Assert.IsNull(newMatchMenu, "NewMatchMenu is not null when it should be.");

//        yield return null;
//    }

//    /// <summary>
//    /// Test ST-M11: Checks that all elements (Join Match button, IP Address input field, Back button) are present and active on the Join Match menu.
//    /// Requirement: FR-M4
//    /// </summary>
//    [UnityTest]
//    public IEnumerator Menu_JoinMatchMenu_ElementsAreActive()
//    {
//        // Get to Join Match Menu
//        Button startGameButton = GameObject.Find("StartGame Button").GetComponent<Button>();
//        startGameButton.onClick.Invoke();
//        Button joinMatchButton = GameObject.Find("JoinMatch Button").GetComponent<Button>();
//        joinMatchButton.onClick.Invoke();

//        // Get all elements
//        Button backButton = GameObject.Find("Back Button").GetComponent<Button>();
//        Button joinMatchButton2 = GameObject.Find("JoinMatch Button").GetComponent<Button>();
//        InputField ipInput = GameObject.Find("IPInput").GetComponent<InputField>();

//        // Assert not null on all elements
//        Assert.IsNotNull(backButton, "Back Button is null!");
//        Assert.IsNotNull(joinMatchButton2, "JoinMatch Button is null!");
//        Assert.IsNotNull(ipInput, "Ip InputField is null!");

//        yield return null;
//    }

//    /// <summary>
//    /// Test ST-M12: Checks functionality of the Back button on the Join Match Menu, and that it brings the user back to the Start Game menu.
//    /// Requirement: FR-M10
//    /// </summary>
//    [UnityTest]
//    public IEnumerator Menu_JoinMatchMenu_BackButtonWorks()
//    {
//        // Get to Join Match Menu
//        Button startGameButton = GameObject.Find("StartGame Button").GetComponent<Button>();
//        startGameButton.onClick.Invoke();
//        Button joinMatchButton = GameObject.Find("JoinMatch Button").GetComponent<Button>();
//        joinMatchButton.onClick.Invoke();

//        // Get Back button and invoke a click on it
//        Button backButton = GameObject.Find("Back Button").GetComponent<Button>();
//        backButton.onClick.Invoke();

//        // Assert that the Start Game menu is active
//        GameObject startGameMenu = GameObject.Find("StartGameMenu");
//        Assert.IsNotNull(startGameMenu, "StartGameMenu is null!");

//        // Assert that the Join Match menu is no longer active
//        GameObject joinMatchMenu = GameObject.Find("JoinMatchMenu");
//        Assert.IsNull(joinMatchMenu, "JoinMatchMenu is not null when it should be.");

//        yield return null;
//    }

//    /// <summary>
//    /// Test ST-M13: Checks that all elements (Instructions, Back button) are present and active on the How To Play menu.
//    /// Requirement: FR-M8
//    /// </summary>
//    [UnityTest]
//    public IEnumerator Menu_HowToPlayMenu_ElementsAreActive()
//    {
//        // Get to How to Play Menu
//        Button howToPlayButton = GameObject.Find("HowToPlay Button").GetComponent<Button>();
//        howToPlayButton.onClick.Invoke();

//        // Get all elements
//        Button backButton = GameObject.Find("Back Button").GetComponent<Button>();
//        GameObject instructions = GameObject.Find("Instructions");

//        // Assert that elements are not null
//        Assert.IsNotNull(backButton, "Back button is null!");
//        Assert.IsNotNull(instructions, "Instructions are null!");

//        yield return null;
//    }

//    /// <summary>
//    /// Test ST-M14: Checks functionality of the Back button on the How to Play Menu, and that it brings the user back to the main menu.
//    /// Requirement: FR-M10
//    /// </summary>
//    [UnityTest]
//    public IEnumerator Menu_HowToPlayMenu_BackButtonWorks()
//    {
//        // Get to How to Play Menu
//        Button howToPlayButton = GameObject.Find("HowToPlay Button").GetComponent<Button>();
//        howToPlayButton.onClick.Invoke();

//        // Get Back button and invoke a click on it
//        Button backButton = GameObject.Find("Back Button").GetComponent<Button>();
//        backButton.onClick.Invoke();

//        // Assert that the main menu is active
//        GameObject mainMenu = GameObject.Find("MainMenu");
//        Assert.IsNotNull(mainMenu, "MainMenu is null!");

//        // Assert that the How to Play menu is no longer active
//        GameObject howToPlayMenu = GameObject.Find("HowToPlayMenu");
//        Assert.IsNull(howToPlayMenu, "HowToPlay menu is not null when it should be.");

//        yield return null;
//    }

//    /// <summary>
//    /// Test ST-M15: Checks that all elements (Volume slider, Graphics Quality, Back button) are present and active on the Settings menu.
//    /// Requirement: FR-M5
//    /// </summary>
//    [UnityTest]
//    public IEnumerator Menu_SettingsMenu_ElementsAreActive()
//    {
//        // Get to Settings Menu
//        Button settingsButton = GameObject.Find("Settings Button").GetComponent<Button>();
//        settingsButton.onClick.Invoke();

//        // Get all elements
//        Button backButton = GameObject.Find("Back Button").GetComponent<Button>();
//        Slider volumeSlider = GameObject.Find("Slider").GetComponent<Slider>();
//        GameObject quality = GameObject.Find("GraphicsQuality");

//        // Assert that elements are not null
//        Assert.IsNotNull(backButton, "Back button is null!");
//        Assert.IsNotNull(volumeSlider, "Volume slider is null!");
//        Assert.IsNotNull(quality, "Graphics quality selector is null!");

//        yield return null;
//    }

//    /// <summary>
//    /// Test ST-M16: Checks the functionality of the volume slider and that the volume of the game changes when the slider is moved.
//    /// Requirement: FR-M7
//    /// </summary>
//    [UnityTest]
//    public IEnumerator Menu_SettingsMenu_VolumeSliderWorks()
//    {
//        // Get to Settings Menu
//        Button settingsButton = GameObject.Find("Settings Button").GetComponent<Button>();
//        settingsButton.onClick.Invoke();

//        // Change volume with volume slider
//        Slider volumeSlider = GameObject.Find("Slider").GetComponent<Slider>();
//        volumeSlider.value = 0.5f;

//        // Assert that volume has changed
//        Assert.AreEqual(volumeSlider.value, AudioListener.volume, "Volume is not equal to the value set in volume slider!");

//        // Check different volume values
//        volumeSlider.value = 0f;
//        Assert.AreEqual(volumeSlider.value, AudioListener.volume, "Volume is not equal to the value set in volume slider!");
//        volumeSlider.value = 1f;
//        Assert.AreEqual(volumeSlider.value, AudioListener.volume, "Volume is not equal to the value set in volume slider!");

//        yield return null;
//    }

//    /// <summary>
//    /// Test ST-M17: Checks the functionality of the graphics selector and that the graphics quality of the game changes when the selector is changed.
//    /// Requirement: FR-M7
//    /// </summary>
//    [UnityTest]
//    public IEnumerator Menu_SettingsMenu_GraphicsQualitySelectorWorks()
//    {
//        // Get to Settings Menu
//        Button settingsButton = GameObject.Find("Settings Button").GetComponent<Button>();
//        settingsButton.onClick.Invoke();

//        // TODO: still need to implement this test case
//        Assert.Fail("PLACEHOLDER: still need to implement graphics quality selector to be able to write a test for it.");

//        yield return null;
//    }

//}