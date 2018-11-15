using UnityEngine;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuPlayTest
{
    readonly string menuSceneName = "Menu";
    readonly string prephaseSceneName = "Prephase";
    readonly int timeToWait = 2;    // number of seconds to wait for after menu scene is loaded

    [SetUp]
    public void SetUp()
    {
        // Load menu scene
        SceneManager.LoadSceneAsync(menuSceneName, LoadSceneMode.Single);
    }

    /// <summary>
    /// Test M1: Checks that the main menu is the only active menu upon initial launch of menu scene
    /// Requirement: FR-M1
    /// </summary>
    [UnityTest]
    public IEnumerator Menu_MainMenu_IsActive()
    {
        // Wait for menu scene to be loaded
        yield return new WaitForSeconds(timeToWait);

        // Get all menus
        GameObject mainMenu = GameObject.Find("MainMenu");
        GameObject settingsMenu = GameObject.Find("SettingsMenu");
        GameObject startGameMenu = GameObject.Find("StartGameMenu");
        GameObject newMatchMenu = GameObject.Find("NewMatchMenu");
        GameObject joinMatchMenu = GameObject.Find("JoinMatchMenu");
        GameObject howToPlayMenu = GameObject.Find("HowToPlayMenu");

        // Assertions; only main menu should be not null (only one active)
        Assert.IsNotNull(mainMenu);
        Assert.IsNull(settingsMenu);
        Assert.IsNull(startGameMenu);
        Assert.IsNull(newMatchMenu);
        Assert.IsNull(joinMatchMenu);
        Assert.IsNull(howToPlayMenu);

        yield return null;
    }

    /// <summary>
    /// Test M2: Checks that all buttons (Start Game, How to Play, Settings, Quit) are present and active on the main menu.
    /// Requirement: FR-M1
    /// </summary>
    [UnityTest]
    public IEnumerator Menu_MainMenu_ButtonsAreActive()
    {
        // Wait for menu scene to be loaded
        yield return new WaitForSeconds(timeToWait);

        // Get all buttons
        GameObject startGameButton = GameObject.Find("StartGame Button");
        GameObject howToPlayButton = GameObject.Find("HowToPlay Button");
        GameObject settingsButton = GameObject.Find("Settings Button");
        GameObject quitButton = GameObject.Find("Quit Button");

        // Assertions; all buttons should not be null
        Assert.IsNotNull(startGameButton);
        Assert.IsNotNull(howToPlayButton);
        Assert.IsNotNull(settingsButton);
        Assert.IsNotNull(quitButton);

        yield return null;
    }

    /// <summary>
    /// Test M3: Checks functionality of the Start Game button, and that it brings the user to the Start Game Menu.
    /// Requirement: FR-M2
    /// </summary>
    [UnityTest]
    public IEnumerator Menu_MainMenu_StartGameButtonWorks()
    {
        // Wait for menu scene to be loaded
        yield return new WaitForSeconds(timeToWait);

        // Get Start Game button and invoke a click on it
        Button startGameButton = GameObject.Find("StartGame Button").GetComponent<Button>();
        startGameButton.onClick.Invoke();

        // Assert that Start Game Menu is active
        GameObject startGameMenu = GameObject.Find("StartGameMenu");
        Assert.IsNotNull(startGameMenu);

        // Assert that the main menu is no longer active
        GameObject mainMenu = GameObject.Find("MainMenu");
        Assert.IsNull(mainMenu);

        yield return null;
    }

    /// <summary>
    /// Test M4: Checks functionality of the How to Play button, and that it brings the user to the How to Play Menu.
    /// Requirement: FR-M8
    /// </summary>
    [UnityTest]
    public IEnumerator Menu_MainMenu_HowToPlayButtonWorks()
    {
        // Wait for menu scene to be loaded
        yield return new WaitForSeconds(timeToWait);

        // Get How to Play button and invoke a click on it
        Button howToPlayButton = GameObject.Find("HowToPlay Button").GetComponent<Button>();
        howToPlayButton.onClick.Invoke();

        // Assert that the How to Play Menu is active
        GameObject howToPlayMenu = GameObject.Find("HowToPlayMenu");
        Assert.IsNotNull(howToPlayMenu);

        // Assert that the main menu is no longer active
        GameObject mainMenu = GameObject.Find("MainMenu");
        Assert.IsNull(mainMenu);

        yield return null;
    }

    /// <summary>
    /// Test M5: Checks functionality of the Settings button, and that it brings the user to the Settings Menu.
    /// Requirement: FR-M5
    /// </summary>
    [UnityTest]
    public IEnumerator Menu_MainMenu_SettingsButtonWorks()
    {
        // Wait for menu scene to be loaded
        yield return new WaitForSeconds(timeToWait);

        // Get Settings button and invoke a click on it
        Button settingsButton = GameObject.Find("Settings Button").GetComponent<Button>();
        settingsButton.onClick.Invoke();

        // Assert that the Settings Menu is active
        GameObject settingsMenu = GameObject.Find("SettingsMenu");
        Assert.IsNotNull(settingsMenu);

        // Assert that the main menu is no longer active
        GameObject mainMenu = GameObject.Find("MainMenu");
        Assert.IsNull(mainMenu);

        yield return null;
    }

    /// <summary>
    /// Test M6: Checks functionality of the Quit button.
    /// Requirement: FR-M9
    /// </summary>
    [UnityTest]
    public IEnumerator Menu_MainMenu_QuitButtonWorks()
    {
        // Wait for menu scene to be loaded
        yield return new WaitForSeconds(timeToWait);

        // Get Quit button and invoke a click on it
        Button quitButton = GameObject.Find("Quit Button").GetComponent<Button>();
        quitButton.onClick.Invoke();

        // Ideally, we would assert if the game is closed here.
        // However, we cannot assert if game is closed in play mode, as Application.Quit() only works on builds and not in play mode.
        // These tests run in play mode.

        yield return null;
    }

    /// <summary>
    /// Test M7: Checks that all buttons (New Match, Join Match) are present and active on the Start Game Menu.
    /// Requirement: FR-M2
    /// </summary>
    [UnityTest]
    public IEnumerator Menu_StartGameMenu_ButtonsAreActive()
    {
        // Wait for menu scene to be loaded
        yield return new WaitForSeconds(timeToWait);

        // Get to Start Game Menu
        Button startGameButton = GameObject.Find("StartGame Button").GetComponent<Button>();
        startGameButton.onClick.Invoke();

        // Get all buttons
        GameObject newMatchButton = GameObject.Find("NewMatch Button");
        GameObject joinMatchButton = GameObject.Find("JoinMatch Button");
        GameObject backButton = GameObject.Find("Back Button");

        // Assertions; all buttons should not be null
        Assert.IsNotNull(newMatchButton);
        Assert.IsNotNull(joinMatchButton);
        Assert.IsNotNull(backButton);

        yield return null;
    }

    /// <summary>
    /// Test M8: Checks functionality of the Back button on the Start Game Menu, and that it brings the user back to the main menu.
    /// Requirement: FR-M2
    /// </summary>
    [UnityTest]
    public IEnumerator Menu_StartGameMenu_BackButtonWorks()
    {
        // Wait for menu scene to be loaded
        yield return new WaitForSeconds(timeToWait);

        // Get Back button and invoke a click on it
        Button backButton = GameObject.Find("Back Button").GetComponent<Button>();
        backButton.onClick.Invoke();

        // Assert that the main menu is active
        GameObject mainMenu = GameObject.Find("MainMenu");
        Assert.IsNotNull(mainMenu);

        // Assert that the Start Game menu is no longer active
        GameObject startGameMenu = GameObject.Find("StartGameMenu");
        Assert.IsNull(startGameMenu);

        yield return null;
    }

    /// <summary>
    /// Test M9: Checks functionality of the New Match button, and that it transitions the user to the next scene (prephase scene).
    /// Requirement: FR-M3
    /// </summary>
    [UnityTest]
    public IEnumerator Menu_StartGameMenu_NewMatchButtonWorks()
    {
        // Wait for menu scene to be loaded
        yield return new WaitForSeconds(timeToWait);

        // Get to Start Game Menu
        Button startGameButton = GameObject.Find("StartGame Button").GetComponent<Button>();
        startGameButton.onClick.Invoke();

        // Invoke click on New Match Button
        Button newMatchButton = GameObject.Find("NewMatch Button").GetComponent<Button>();
        newMatchButton.onClick.Invoke();

        // Assert that New Match Menu is active
        GameObject newMatchMenu = GameObject.Find("NewMatchMenu");
        Assert.IsNotNull(newMatchMenu);

        // Invoke click on Create Dungeon button
        Button createDungeonButton = GameObject.Find("HostButton").GetComponent<Button>();
        createDungeonButton.onClick.Invoke();

        // Wait for new scene to be loaded
        yield return new WaitForSeconds(timeToWait);

        // Assert that the current scene is the prephase scene
        Assert.AreEqual(prephaseSceneName, SceneManager.GetActiveScene().name);

        yield return null;
    }

    /// <summary>
    /// Test M10: Checks functionality of the Back button on the New Match Menu, and that it brings the user back to the Start Game menu.
    /// Requirement: FR-M2
    /// </summary>
    [UnityTest]
    public IEnumerator Menu_NewMatchMenu_BackButtonWorks()
    {
        // Wait for menu scene to be loaded
        yield return new WaitForSeconds(timeToWait);

        // Get Back button and invoke a click on it
        Button backButton = GameObject.Find("Back Button").GetComponent<Button>();
        backButton.onClick.Invoke();

        // Assert that the main menu is active
        GameObject startGameMenu = GameObject.Find("StartGameMenu");
        Assert.IsNotNull(startGameMenu);

        // Assert that the Start Game menu is no longer active
        GameObject newMatchMenu = GameObject.Find("NewMatchMenu");
        Assert.IsNull(newMatchMenu);

        yield return null;
    }

    /// <summary>
    /// Test M11: Checks functionality of the Join Match button, and that it transitions the user to the next scene (prephase scene).
    /// Requirement: FR-M4
    /// </summary>
    [UnityTest]
    public IEnumerator Menu_StartGameMenu_JoinMatchButtonWorks()
    {
        // Wait for menu scene to be loaded
        yield return new WaitForSeconds(timeToWait);

        // Get to Start Game Menu
        Button startGameButton = GameObject.Find("StartGame Button").GetComponent<Button>();
        startGameButton.onClick.Invoke();

        // Invoke click on Join Match
        Button joinMatchButton = GameObject.Find("JoinMatch Button").GetComponent<Button>();
        joinMatchButton.onClick.Invoke();

        // Assert that Join Match Menu is active
        GameObject joinMatchMenu = GameObject.Find("JoinMatchMenu");
        Assert.IsNotNull(joinMatchMenu);

        // Enter IP address
        InputField ipInput = GameObject.Find("IpInput").GetComponent<InputField>();
        ipInput.text = "172.17.54.245";

        // Invoke click on Join Match
        joinMatchButton = GameObject.Find("JoinMatch Button").GetComponent<Button>();
        joinMatchButton.onClick.Invoke();

        // Wait for new scene to be loaded
        yield return new WaitForSeconds(timeToWait);

        // Assert that the current scene is the prephase scene
        Assert.AreEqual("DungeonLevel", SceneManager.GetActiveScene().name);

        yield return null;
    }

    /// <summary>
    /// Test M12: Checks functionality of the Back button on the Join Match Menu, and that it brings the user back to the Start Game menu.
    /// Requirement: FR-M2
    /// </summary>
    [UnityTest]
    public IEnumerator Menu_JoinMatchMenu_BackButtonWorks()
    {
        // Wait for menu scene to be loaded
        yield return new WaitForSeconds(timeToWait);

        // Get Back button and invoke a click on it
        Button backButton = GameObject.Find("Back Button").GetComponent<Button>();
        backButton.onClick.Invoke();

        // Assert that the Start Game menu is active
        GameObject startGameMenu = GameObject.Find("StartGameMenu");
        Assert.IsNotNull(startGameMenu);

        // Assert that the Join Match menu is no longer active
        GameObject joinMatchMenu = GameObject.Find("JoinMatchMenu");
        Assert.IsNull(joinMatchMenu);

        yield return null;
    }
}