using UnityEngine;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;
using UnityEngine.SceneManagement;
using NSubstitute;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// This class contains system tests for In-Game Menu on the Test scene.
/// These tests run on Play Mode in Unity's Test Runner.
/// </summary>
public class InGameMenuPlayTest
{
    readonly int timeToWait = 2;    // number of seconds to wait for after test scene is loaded
    readonly string menuSceneName = "Menu";

    [SetUp]
    public void SetUp()
    {
        // Load Test scene
        SceneManager.LoadSceneAsync("TestScene", LoadSceneMode.Single);
    }

    /// <summary>
    /// Test ST-IGM1: Checks that when ESC is pressed, the in-game menu displays.
    /// Requirement: FR-M6
    /// </summary>
    [UnityTest]
    public IEnumerator InGameMenu_DisplaysWhenEscPressed()
    {
        // Wait for test scene to be loaded
        yield return new WaitForSeconds(timeToWait);

        // Get in-game menu
        GameObject menu = GameObject.Find("Menu");
        GameObject inGameMenu = menu.transform.Find("In-Game Menu").gameObject;
        Assert.IsNotNull(inGameMenu, "In-Game Menu is null!");

        // Get DungeonController
        DungeonController dungeonController = GameObject.Find("EventSystem").GetComponent<DungeonController>();
        Assert.IsNotNull(dungeonController, "DungeonController is null!");

        // Substitute UnityService to mock player input
        var unityService = Substitute.For<IUnityService>();
        unityService.GetKeyDown(KeyCode.Escape).Returns(true);  // mock player pressing ESC
        dungeonController.unityService = unityService;

        // Allow test to run for x amount of seconds
        yield return new WaitForSeconds(timeToWait);

        // Assert that in-game menu is active
        Assert.IsTrue(inGameMenu.activeSelf);

        yield return null;
    }

    /// <summary>
    /// Test ST-IGM2: Checks that the Options menu is the only menu active upon initial launch of the in-game menu.
    /// Requirement: FR-M6
    /// </summary>
    [UnityTest]
    public IEnumerator InGameMenu_DisplaysOptionsMenu()
    {
        // Wait for test scene to be loaded
        yield return new WaitForSeconds(timeToWait);

        // Get in-game menu
        GameObject menu = GameObject.Find("Menu");
        GameObject inGameMenu = menu.transform.Find("In-Game Menu").gameObject;
        Assert.IsNotNull(inGameMenu, "In-Game Menu is null!");

        // Get DungeonController
        DungeonController dungeonController = GameObject.Find("EventSystem").GetComponent<DungeonController>();
        Assert.IsNotNull(dungeonController, "DungeonController is null!");

        // Substitute UnityService to mock player input
        var unityService = Substitute.For<IUnityService>();
        unityService.GetKeyDown(KeyCode.Escape).Returns(true);  // mock player pressing ESC
        dungeonController.unityService = unityService;

        // Allow test to run for x amount of seconds
        yield return new WaitForSeconds(timeToWait);

        // Get menus
        GameObject optionsMenu = GameObject.Find("OptionsMenu");
        GameObject settingsMenu = GameObject.Find("SettingsMenu");
        GameObject graphicsMenu = GameObject.Find("GraphicsMenu");
        GameObject audioMenu = GameObject.Find("AudioMenu");
        GameObject networkMenu = GameObject.Find("NetworkMenu");
        GameObject quitMenu = GameObject.Find("QuitMenu");

        // Assert that only the Options menu is not null; all other menus should be null
        Assert.IsNotNull(optionsMenu);
        Assert.IsNull(settingsMenu);
        Assert.IsNull(graphicsMenu);
        Assert.IsNull(audioMenu);
        Assert.IsNull(networkMenu);
        Assert.IsNull(quitMenu);

        yield return null;
    }

    /// <summary>
    /// Test ST-IGM3: Checks that the in-game menu closes when the Resume button is pressed.
    /// Requirement: FR-M6
    /// </summary>
    [UnityTest]
    public IEnumerator InGameMenu_OptionsMenu_ResumeButtonWorks()
    {
        // Wait for test scene to be loaded
        yield return new WaitForSeconds(timeToWait);

        // Get DungeonController
        DungeonController dungeonController = GameObject.Find("EventSystem").GetComponent<DungeonController>();

        // Get to in-game menu
        GameObject menu = GameObject.Find("Menu");
        GameObject inGameMenu = menu.transform.Find("In-Game Menu").gameObject;
        inGameMenu.SetActive(true);

        // Select Resume Button
        Button resumeButton = GameObject.Find("ResumeButton").GetComponent<Button>();
        Assert.IsNotNull(resumeButton, "ResumeButton is null!");
        resumeButton.onClick.Invoke();

        // Assert that in-game menu is inactive
        Assert.IsFalse(inGameMenu.activeSelf);

        yield return null;
    }

    /// <summary>
    /// Test ST-IGM4: Checks that the Settings menu becomes active when the Settings button is pressed.
    /// Requirement: FR-M6
    /// </summary>
    [UnityTest]
    public IEnumerator InGameMenu_OptionsMenu_SettingsButtonWorks()
    {
        // Wait for test scene to be loaded
        yield return new WaitForSeconds(timeToWait);

        // Get DungeonController
        DungeonController dungeonController = GameObject.Find("EventSystem").GetComponent<DungeonController>();

        // Get to in-game menu
        GameObject menu = GameObject.Find("Menu");
        GameObject inGameMenu = menu.transform.Find("In-Game Menu").gameObject;
        inGameMenu.SetActive(true);

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
    /// Test ST-IGM5: Checks that the Quit menu becomes active when the Quit button is pressed.
    /// Requirement: FR-M9
    /// </summary>
    [UnityTest]
    public IEnumerator InGameMenu_OptionsMenu_QuitButtonWorks()
    {
        // Wait for test scene to be loaded
        yield return new WaitForSeconds(timeToWait);

        // Get DungeonController
        DungeonController dungeonController = GameObject.Find("EventSystem").GetComponent<DungeonController>();

        // Get to in-game menu
        GameObject menu = GameObject.Find("Menu");
        GameObject inGameMenu = menu.transform.Find("In-Game Menu").gameObject;
        inGameMenu.SetActive(true);

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
    /// Test ST-IGM6: Checks that the Graphics menu becomes active when the Graphics button is pressed.
    /// Requirement: FR-M7
    /// </summary>
    [UnityTest]
    public IEnumerator InGameMenu_SettingsMenu_GraphicsButtonWorks()
    {
        // Wait for test scene to be loaded
        yield return new WaitForSeconds(timeToWait);

        // Get DungeonController
        DungeonController dungeonController = GameObject.Find("EventSystem").GetComponent<DungeonController>();

        // Get to in-game settings menu
        GameObject menu = GameObject.Find("Menu");
        GameObject inGameMenu = menu.transform.Find("In-Game Menu").gameObject;
        inGameMenu.SetActive(true);
        Button settingsButton = GameObject.Find("SettingsButton").GetComponent<Button>();
        settingsButton.onClick.Invoke();

        // Select Graphics Button
        Button graphicsButton = GameObject.Find("GraphicsButton").GetComponent<Button>();
        Assert.IsNotNull(graphicsButton, "GraphicsButton is null!");
        graphicsButton.onClick.Invoke();

        // Get menus
        GameObject optionsMenu = GameObject.Find("OptionsMenu");
        GameObject settingsMenu = GameObject.Find("SettingsMenu");
        GameObject graphicsMenu = GameObject.Find("GraphicsMenu");

        // Assert that GraphicsMenu is not null, and OptionsMenu & SettingsMenu is null
        Assert.IsNull(optionsMenu);
        Assert.IsNull(settingsMenu);
        Assert.IsNotNull(graphicsMenu);

        yield return null;
    }

    /// <summary>
    /// Test ST-IGM7: Checks that the Audio menu becomes active when the Audio button is pressed.
    /// Requirement: FR-M7
    /// </summary>
    [UnityTest]
    public IEnumerator InGameMenu_SettingsMenu_AudioButtonWorks()
    {
        // Wait for test scene to be loaded
        yield return new WaitForSeconds(timeToWait);

        // Get DungeonController
        DungeonController dungeonController = GameObject.Find("EventSystem").GetComponent<DungeonController>();

        // Get to in-game settings menu
        GameObject menu = GameObject.Find("Menu");
        GameObject inGameMenu = menu.transform.Find("In-Game Menu").gameObject;
        inGameMenu.SetActive(true);
        Button settingsButton = GameObject.Find("SettingsButton").GetComponent<Button>();
        settingsButton.onClick.Invoke();

        // Select Audio Button
        Button audioButton = GameObject.Find("AudioButton").GetComponent<Button>();
        Assert.IsNotNull(audioButton, "AudioButton is null!");
        audioButton.onClick.Invoke();

        // Get menus
        GameObject optionsMenu = GameObject.Find("OptionsMenu");
        GameObject settingsMenu = GameObject.Find("SettingsMenu");
        GameObject audioMenu = GameObject.Find("AudioMenu");

        // Assert that AudioMenu is not null, and OptionsMenu & SettingsMenu is null
        Assert.IsNull(optionsMenu);
        Assert.IsNull(settingsMenu);
        Assert.IsNotNull(audioMenu);

        yield return null;
    }

    /// <summary>
    /// Test ST-IGM8: Checks that the Network menu becomes active when the Network button is pressed.
    /// Requirement: FR-M7
    /// </summary>
    [UnityTest]
    public IEnumerator InGameMenu_SettingsMenu_NetworkButtonWorks()
    {
        // Wait for test scene to be loaded
        yield return new WaitForSeconds(timeToWait);

        // Get DungeonController
        DungeonController dungeonController = GameObject.Find("EventSystem").GetComponent<DungeonController>();

        // Get to in-game settings menu
        GameObject menu = GameObject.Find("Menu");
        GameObject inGameMenu = menu.transform.Find("In-Game Menu").gameObject;
        inGameMenu.SetActive(true);
        Button settingsButton = GameObject.Find("SettingsButton").GetComponent<Button>();
        settingsButton.onClick.Invoke();

        // Select Network Button
        Button networkButton = GameObject.Find("NetworkButton").GetComponent<Button>();
        Assert.IsNotNull(networkButton, "NetworkButton is null!");
        networkButton.onClick.Invoke();

        // Get menus
        GameObject optionsMenu = GameObject.Find("OptionsMenu");
        GameObject settingsMenu = GameObject.Find("SettingsMenu");
        GameObject networkMenu = GameObject.Find("NetworkMenu");

        // Assert that NetworkMenu is not null, and OptionsMenu & SettingsMenu is null
        Assert.IsNull(optionsMenu);
        Assert.IsNull(settingsMenu);
        Assert.IsNotNull(networkMenu);

        yield return null;
    }

    /// <summary>
    /// Test ST-IGM9: Checks that the Back button on the Settings menu works, and that it returns to the Options menu.
    /// Requirement: FR-M10
    /// </summary>
    [UnityTest]
    public IEnumerator InGameMenu_SettingsMenu_BackButtonWorks()
    {
        // Wait for test scene to be loaded
        yield return new WaitForSeconds(timeToWait);

        // Get DungeonController
        DungeonController dungeonController = GameObject.Find("EventSystem").GetComponent<DungeonController>();

        // Get to in-game settings menu
        GameObject menu = GameObject.Find("Menu");
        GameObject inGameMenu = menu.transform.Find("In-Game Menu").gameObject;
        inGameMenu.SetActive(true);
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
    /// Test ST-IGM10: Checks that the No button on the Quit menu returns the user to Options menu.
    /// Requirement: FR-M10
    /// </summary>
    [UnityTest]
    public IEnumerator InGameMenu_QuitMenu_NoButtonWorks()
    {
        // Wait for test scene to be loaded
        yield return new WaitForSeconds(timeToWait);

        // Get DungeonController
        DungeonController dungeonController = GameObject.Find("EventSystem").GetComponent<DungeonController>();

        // Get to in-game quit menu
        GameObject menu = GameObject.Find("Menu");
        GameObject inGameMenu = menu.transform.Find("In-Game Menu").gameObject;
        inGameMenu.SetActive(true);
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
    /// Test ST-IGM11: Checks that the Yes button on the Quit menu returns the user to the main menu.
    /// Requirement: FR-M9
    /// </summary>
    [UnityTest]
    public IEnumerator InGameMenu_QuitMenu_YesButtonWorks()
    {
        // Wait for test scene to be loaded
        yield return new WaitForSeconds(timeToWait);

        // Get DungeonController
        DungeonController dungeonController = GameObject.Find("EventSystem").GetComponent<DungeonController>();

        // Get to in-game quit menu
        GameObject menu = GameObject.Find("Menu");
        GameObject inGameMenu = menu.transform.Find("In-Game Menu").gameObject;
        inGameMenu.SetActive(true);
        Button quitButton = GameObject.Find("QuitButton").GetComponent<Button>();
        quitButton.onClick.Invoke();

        // Select Yes Button
        Button yesButton = GameObject.Find("YesButton").GetComponent<Button>();
        Assert.IsNotNull(yesButton, "YesButton is null!");
        yesButton.onClick.Invoke();

        // Wait for menu scene to be loaded
        yield return new WaitForSeconds(timeToWait);

        // Assert that the scene has returned to the Menu scene (main menu)
        Assert.AreEqual(menuSceneName, SceneManager.GetActiveScene().name, "Current scene is not Menu!");

        yield return null;
    }

    /// <summary>
    /// Test ST-IGM12: Checks that all important elements (quality selector, resolution selector) are present and active on the in-game Graphics menu.
    /// Requirement: FR-M7
    /// </summary>
    [UnityTest]
    public IEnumerator InGameMenu_GraphicsMenu_ElementsAreActive()
    {
        // Wait for test scene to be loaded
        yield return new WaitForSeconds(timeToWait);

        // Get DungeonController
        DungeonController dungeonController = GameObject.Find("EventSystem").GetComponent<DungeonController>();

        // Get to the in-game graphics menu
        GameObject menu = GameObject.Find("Menu");
        GameObject inGameMenu = menu.transform.Find("In-Game Menu").gameObject;
        inGameMenu.SetActive(true);
        Button settingsButton = GameObject.Find("SettingsButton").GetComponent<Button>();
        settingsButton.onClick.Invoke();
        Button graphicsButton = GameObject.Find("GraphicsButton").GetComponent<Button>();
        graphicsButton.onClick.Invoke();

        // Get active elements
        GameObject qualityDropdown = GameObject.Find("QualityDropdown");
        GameObject resolutionDropdown = GameObject.Find("ResolutionDropdown");

        // Assert that all elements are not null
        Assert.IsNotNull(qualityDropdown, "QualityDropdown is null!");
        Assert.IsNotNull(resolutionDropdown, "ResultionDropdown is null!");

        yield return null;
    }

    /// <summary>
    /// Test ST-IGM13: Checks that the Back button on the Graphics menu brings the user back to the Settings menu.
    /// Requirement: FR-M10
    /// </summary>
    [UnityTest]
    public IEnumerator InGameMenu_GraphicsMenu_BackButtonWorks()
    {
        // Wait for test scene to be loaded
        yield return new WaitForSeconds(timeToWait);

        // Get DungeonController
        DungeonController dungeonController = GameObject.Find("EventSystem").GetComponent<DungeonController>();

        // Get to the in-game graphics menu
        GameObject menu = GameObject.Find("Menu");
        GameObject inGameMenu = menu.transform.Find("In-Game Menu").gameObject;
        inGameMenu.SetActive(true);
        Button settingsButton = GameObject.Find("SettingsButton").GetComponent<Button>();
        settingsButton.onClick.Invoke();
        Button graphicsButton = GameObject.Find("GraphicsButton").GetComponent<Button>();
        graphicsButton.onClick.Invoke();

        // Select the Back button
        Button backButton = GameObject.Find("BackButton").GetComponent<Button>();
        backButton.onClick.Invoke();

        // Get menus
        GameObject graphicsMenu = GameObject.Find("GraphicsMenu");
        GameObject settingsMenu = GameObject.Find("SettingsMenu");

        // Assert SettingsMenu is not null, and GraphicsMenu is null
        Assert.IsNotNull(settingsMenu, "SettingsMenu is null!");
        Assert.IsNull(graphicsMenu, "GraphicsMenu is not null!");

        yield return null;
    }

    /// <summary>
    /// Test ST-IGM14:  Checks that the options of the quality selector in the in-game Graphics menu are the expected values (High, Medium, Low).
    /// Requirement: FR-M7
    /// </summary>
    [UnityTest]
    public IEnumerator InGameMenu_GraphicsMenu_QualitySelectorOptionsAreExpected()
    {
        // Wait for test scene to be loaded
        yield return new WaitForSeconds(timeToWait);

        // Get DungeonController
        DungeonController dungeonController = GameObject.Find("EventSystem").GetComponent<DungeonController>();

        // Get to the in-game graphics menu
        GameObject menu = GameObject.Find("Menu");
        GameObject inGameMenu = menu.transform.Find("In-Game Menu").gameObject;
        inGameMenu.SetActive(true);
        Button settingsButton = GameObject.Find("SettingsButton").GetComponent<Button>();
        settingsButton.onClick.Invoke();
        Button graphicsButton = GameObject.Find("GraphicsButton").GetComponent<Button>();
        graphicsButton.onClick.Invoke();

        // Get quality selector
        TMP_Dropdown qualityDropdown = GameObject.Find("QualityDropdown").GetComponent<TMP_Dropdown>();
        Assert.IsNotNull(qualityDropdown, "QualityDropdown is null!");

        // Assert that options are as expected
        Assert.AreEqual("High", qualityDropdown.options[0].text);   // High quality option
        Assert.AreEqual("Medium", qualityDropdown.options[1].text); // Medium quality option
        Assert.AreEqual("Low", qualityDropdown.options[2].text);    // Low quality option

        yield return null;
    }

    /// <summary>
    /// Test ST-IGM15:  Checks the functionality of the quality selector in the in-game Graphics menu.
    ///                 Checks that when the selection is changed, the game's quality changes.
    /// Requirement: FR-M7
    /// </summary>
    [UnityTest]
    public IEnumerator InGameMenu_GraphicsMenu_QualitySelectorWorks()
    {
        // Wait for test scene to be loaded
        yield return new WaitForSeconds(timeToWait);

        // Get DungeonController
        DungeonController dungeonController = GameObject.Find("EventSystem").GetComponent<DungeonController>();

        // Get to the in-game graphics menu
        GameObject menu = GameObject.Find("Menu");
        GameObject inGameMenu = menu.transform.Find("In-Game Menu").gameObject;
        inGameMenu.SetActive(true);
        Button settingsButton = GameObject.Find("SettingsButton").GetComponent<Button>();
        settingsButton.onClick.Invoke();
        Button graphicsButton = GameObject.Find("GraphicsButton").GetComponent<Button>();
        graphicsButton.onClick.Invoke();

        // Get quality selector
        TMP_Dropdown qualityDropdown = GameObject.Find("QualityDropdown").GetComponent<TMP_Dropdown>();
        Assert.IsNotNull(qualityDropdown, "QualityDropdown is null!");
        qualityDropdown.value = 0;  // Select High quality
        qualityDropdown.value = 1;  // Select Medium quality
        qualityDropdown.value = 2;  // Select Low quality

        // Assert that all elements are not null
        Assert.Fail("PLACEHOLDER: Need to implement graphics quality selection in-game to test this function.");

        yield return null;
    }

    /// <summary>
    /// Test ST-IGM16:  Checks that the options of the resolution selector in the in-game Graphics menu are the expected values (1920x1080, 1280x720).
    /// Requirement: FR-M7
    /// </summary>
    [UnityTest]
    public IEnumerator InGameMenu_GraphicsMenu_ResolutionSelectorOptionsAreExpected()
    {
        // Wait for test scene to be loaded
        yield return new WaitForSeconds(timeToWait);

        // Get DungeonController
        DungeonController dungeonController = GameObject.Find("EventSystem").GetComponent<DungeonController>();

        // Get to the in-game graphics menu
        GameObject menu = GameObject.Find("Menu");
        GameObject inGameMenu = menu.transform.Find("In-Game Menu").gameObject;
        inGameMenu.SetActive(true);
        Button settingsButton = GameObject.Find("SettingsButton").GetComponent<Button>();
        settingsButton.onClick.Invoke();
        Button graphicsButton = GameObject.Find("GraphicsButton").GetComponent<Button>();
        graphicsButton.onClick.Invoke();

        // Get quality selector
        TMP_Dropdown resolutionDropdown = GameObject.Find("ResolutionDropdown").GetComponent<TMP_Dropdown>();
        Assert.IsNotNull(resolutionDropdown, "ResolutionDropdown is null!");

        // Assert that options are as expected
        Assert.AreEqual("1920x1080", resolutionDropdown.options[0].text);   // 1920x1080 resolution option
        Assert.AreEqual("1280x720", resolutionDropdown.options[1].text);    // 1280x720 resolution option

        yield return null;
    }

    /// <summary>
    /// Test ST-IGM17:  Checks the functionality of the resolution selector in the in-game Graphics menu.
    ///                 Checks that when the selection is changed, the game's resolution changes.
    /// Requirement: FR-M7
    /// </summary>
    [UnityTest]
    public IEnumerator InGameMenu_GraphicsMenu_ResolutionSelectorWorks()
    {
        // Wait for test scene to be loaded
        yield return new WaitForSeconds(timeToWait);

        // Get DungeonController
        DungeonController dungeonController = GameObject.Find("EventSystem").GetComponent<DungeonController>();

        // Get to the in-game graphics menu
        GameObject menu = GameObject.Find("Menu");
        GameObject inGameMenu = menu.transform.Find("In-Game Menu").gameObject;
        inGameMenu.SetActive(true);
        Button settingsButton = GameObject.Find("SettingsButton").GetComponent<Button>();
        settingsButton.onClick.Invoke();
        Button graphicsButton = GameObject.Find("GraphicsButton").GetComponent<Button>();
        graphicsButton.onClick.Invoke();

        // Get quality selector
        TMP_Dropdown resolutionDropdown = GameObject.Find("ResolutionDropdown").GetComponent<TMP_Dropdown>();
        Assert.IsNotNull(resolutionDropdown, "ResolutionDropdown is null!");

        // Test 1920x1080 resolution
        resolutionDropdown.value = 0;   // Select 1920x1080 resolution
        Assert.AreEqual(1920, Screen.width, "Screen width is not 1920px!");
        Assert.AreEqual(1080, Screen.height, "Screen height is not 1080px!");

        // Test 1280x720 resolution
        resolutionDropdown.value = 1;   // Select 1280x720 resolution
        Assert.AreEqual(1280, Screen.width, "Screen width is not 1280px!");
        Assert.AreEqual(720, Screen.height, "Screen height is not 720px!");

        yield return null;
    }

    /// <summary>
    /// Test ST-IGM18: Checks that all element(s) (volume selector) are present and active on the in-game Audio menu.
    /// Requirement: FR-M7
    /// </summary>
    [UnityTest]
    public IEnumerator InGameMenu_AudioMenu_ElementsAreActive()
    {
        // Wait for test scene to be loaded
        yield return new WaitForSeconds(timeToWait);

        // Get DungeonController
        DungeonController dungeonController = GameObject.Find("EventSystem").GetComponent<DungeonController>();

        // Get to the in-game audio menu
        GameObject menu = GameObject.Find("Menu");
        GameObject inGameMenu = menu.transform.Find("In-Game Menu").gameObject;
        inGameMenu.SetActive(true);
        Button settingsButton = GameObject.Find("SettingsButton").GetComponent<Button>();
        settingsButton.onClick.Invoke();
        Button audioButton = GameObject.Find("AudioButton").GetComponent<Button>();
        audioButton.onClick.Invoke();

        // Get active elements
        Slider volumeSlider = GameObject.Find("VolumeSlider").GetComponent<Slider>();

        // Assert that all elements are not null
        Assert.IsNotNull(volumeSlider, "Volume Slider is null!");

        yield return null;
    }

    /// <summary>
    /// Test ST-IGM19: Checks the functionality of the volume slider on the in-game Audio menu, and that the volume of the game changes as the slider is changed.
    /// Requirement: FR-M7
    /// </summary>
    [UnityTest]
    public IEnumerator InGameMenu_AudioMenu_VolumeSliderWorks()
    {
        // Wait for test scene to be loaded
        yield return new WaitForSeconds(timeToWait);

        // Get DungeonController
        DungeonController dungeonController = GameObject.Find("EventSystem").GetComponent<DungeonController>();

        // Get to the in-game audio menu
        GameObject menu = GameObject.Find("Menu");
        GameObject inGameMenu = menu.transform.Find("In-Game Menu").gameObject;
        inGameMenu.SetActive(true);
        Button settingsButton = GameObject.Find("SettingsButton").GetComponent<Button>();
        settingsButton.onClick.Invoke();
        Button audioButton = GameObject.Find("AudioButton").GetComponent<Button>();
        audioButton.onClick.Invoke();

        // Set value of volume Slider
        Slider volumeSlider = GameObject.Find("VolumeSlider").GetComponent<Slider>();
        volumeSlider.value = 0.5f;

        // Assert that volume has changed
        Assert.AreEqual(volumeSlider.value, AudioListener.volume, "Volume is not equal to the value set in volume slider!");

        // Check different volume values
        volumeSlider.value = 0f;
        Assert.AreEqual(volumeSlider.value, AudioListener.volume, "Volume is not equal to the value set in volume slider!");
        volumeSlider.value = 1f;
        Assert.AreEqual(volumeSlider.value, AudioListener.volume, "Volume is not equal to the value set in volume slider!");

        yield return null;
    }

    /// <summary>
    /// Test ST-IGM20: Checks that the Back button on the Audio menu brings the user back to the Settings menu.
    /// Requirement: FR-M10
    /// </summary>
    [UnityTest]
    public IEnumerator InGameMenu_AudioMenu_BackButtonWorks()
    {
        // Wait for test scene to be loaded
        yield return new WaitForSeconds(timeToWait);

        // Get DungeonController
        DungeonController dungeonController = GameObject.Find("EventSystem").GetComponent<DungeonController>();

        // Get to the in-game graphics menu
        GameObject menu = GameObject.Find("Menu");
        GameObject inGameMenu = menu.transform.Find("In-Game Menu").gameObject;
        inGameMenu.SetActive(true);
        Button settingsButton = GameObject.Find("SettingsButton").GetComponent<Button>();
        settingsButton.onClick.Invoke();
        Button audioButton = GameObject.Find("AudioButton").GetComponent<Button>();
        audioButton.onClick.Invoke();

        // Select the Back button
        Button backButton = GameObject.Find("BackButton").GetComponent<Button>();
        backButton.onClick.Invoke();

        // Get menus
        GameObject audioMenu = GameObject.Find("AudioMenu");
        GameObject settingsMenu = GameObject.Find("SettingsMenu");

        // Assert SettingsMenu is not null, and AudioMenu is null
        Assert.IsNotNull(settingsMenu, "SettingsMenu is null!");
        Assert.IsNull(audioMenu, "AudioMenu is not null!");

        yield return null;
    }

    /// <summary>
    /// Test ST-IGM21: Checks that the HostIp element is present and active on the in-game Network menu.
    /// Requirement: FR-M6
    /// </summary>
    [UnityTest]
    public IEnumerator InGameMenu_NetworkMenu_ElementsAreActive()
    {
        // Wait for test scene to be loaded
        yield return new WaitForSeconds(timeToWait);

        // Get DungeonController
        DungeonController dungeonController = GameObject.Find("EventSystem").GetComponent<DungeonController>();

        // Get to the in-game audio menu
        GameObject menu = GameObject.Find("Menu");
        GameObject inGameMenu = menu.transform.Find("In-Game Menu").gameObject;
        inGameMenu.SetActive(true);
        Button settingsButton = GameObject.Find("SettingsButton").GetComponent<Button>();
        settingsButton.onClick.Invoke();
        Button networkButton = GameObject.Find("NetworkButton").GetComponent<Button>();
        networkButton.onClick.Invoke();

        // Get active elements
        GameObject hostIp = GameObject.Find("HostIP");

        // Assert that all elements are not null
        Assert.IsNotNull(hostIp, "HostIP is null!");

        yield return null;
    }

    /// <summary>
    /// Test ST-IGM22: Checks that the Back button on the Network menu brings the user back to the Settings menu.
    /// Requirement: FR-M10
    /// </summary>
    [UnityTest]
    public IEnumerator InGameMenu_NetworkMenu_BackButtonWorks()
    {
        // Wait for test scene to be loaded
        yield return new WaitForSeconds(timeToWait);

        // Get DungeonController
        DungeonController dungeonController = GameObject.Find("EventSystem").GetComponent<DungeonController>();

        // Get to the in-game graphics menu
        GameObject menu = GameObject.Find("Menu");
        GameObject inGameMenu = menu.transform.Find("In-Game Menu").gameObject;
        inGameMenu.SetActive(true);
        Button settingsButton = GameObject.Find("SettingsButton").GetComponent<Button>();
        settingsButton.onClick.Invoke();
        Button networkButton = GameObject.Find("NetworkButton").GetComponent<Button>();
        networkButton.onClick.Invoke();

        // Select the Back button
        Button backButton = GameObject.Find("BackButton").GetComponent<Button>();
        backButton.onClick.Invoke();

        // Get menus
        GameObject networkMenu = GameObject.Find("NetworkMenu");
        GameObject settingsMenu = GameObject.Find("SettingsMenu");

        // Assert SettingsMenu is not null, and NetWorkMenu is null
        Assert.IsNotNull(settingsMenu, "SettingsMenu is null!");
        Assert.IsNull(networkMenu, "NetWorkMenu is not null!");

        yield return null;
    }

}
