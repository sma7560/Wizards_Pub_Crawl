using UnityEngine;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;
using UnityEngine.SceneManagement;

public class MenuPlayTest
{
    readonly string testSceneName = "TestScene";
    readonly string menuSceneName = "Menu";

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
    public IEnumerator Menu_MenusExist()
    {
        // Wait for menu scene to be loaded
        yield return new WaitForSeconds(3);

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

    [TearDown]
    public void TearDown()
    {
        // Unload menu scene
        SceneManager.UnloadSceneAsync(menuSceneName);
    }
}
