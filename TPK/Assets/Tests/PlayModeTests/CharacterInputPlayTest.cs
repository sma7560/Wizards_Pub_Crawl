using UnityEngine;
using UnityEngine.TestTools;
using NSubstitute;
using NUnit.Framework;
using System.Collections;
using UnityEngine.SceneManagement;

public class CharacterInputPlayTest
{

    readonly int timeToWait = 2;    // number of seconds to wait for after test scene is loaded

    //private GameObject hero;
    //private HeroController heroController;

    [SetUp]
    public void SetUp()
    {
        // Load Test scene
        SceneManager.LoadSceneAsync("TestScene", LoadSceneMode.Single);
    }

    /// <summary>
    /// Test ST-CI1: Checks functionality of character movement along the x-axis (right).
    /// Requirement: FR-4
    /// </summary>
    [UnityTest]
    public IEnumerator CharacterInput_MovesAlongXAxis_Right()
    {
        // Wait for test scene to be loaded
        yield return new WaitForSeconds(timeToWait);

        // Setup Hero GameObject
        GameObject heroes = GameObject.Find("Heroes");
        GameObject hero = heroes.transform.Find("Hero1").gameObject;
        Assert.IsNotNull(hero, "Hero object is null!");
        hero.SetActive(true);

        // Get HeroController
        HeroController heroController = hero.GetComponent<HeroController>();
        Assert.IsNotNull(heroController, "HeroController is null!");

        // Get initial x-position
        float initialX = hero.transform.position.x;

        // Substitute UnityService to mock player movement input
        var unityService = Substitute.For<IUnityService>();
        unityService.GetAxisRaw("Horizontal").Returns(1);   // move player on x-axis
        unityService.GetAxisRaw("Vertical").Returns(0);
        heroController.unityService = unityService;

        // Allow test to run for x amount of seconds
        yield return new WaitForSeconds(timeToWait);

        // Assert that hero has moved to the right as expected
        Assert.Greater(hero.transform.position.x, initialX, "Hero did not move to the right!");

        yield return null;
    }

    /// <summary>
    /// Test ST-CI2: Checks functionality of character movement along the x-axis (left).
    /// Requirement: FR-4
    /// </summary>
    [UnityTest]
    public IEnumerator CharacterInput_MovesAlongXAxis_Left()
    {
        // Wait for test scene to be loaded
        yield return new WaitForSeconds(timeToWait);

        // Setup Hero GameObject
        GameObject heroes = GameObject.Find("Heroes");
        GameObject hero = heroes.transform.Find("Hero1").gameObject;
        Assert.IsNotNull(hero, "Hero object is null!");
        hero.SetActive(true);

        // Get HeroController
        HeroController heroController = hero.GetComponent<HeroController>();
        Assert.IsNotNull(heroController, "HeroController is null!");

        // Get initial x-position
        float initialX = hero.transform.position.x;

        // Substitute UnityService to mock player movement input
        var unityService = Substitute.For<IUnityService>();
        unityService.GetAxisRaw("Horizontal").Returns(-1);   // move player on x-axis
        unityService.GetAxisRaw("Vertical").Returns(0);
        heroController.unityService = unityService;

        // Allow test to run for x amount of seconds
        yield return new WaitForSeconds(timeToWait);

        // Assert that hero has moved to the left as expected
        Assert.Less(hero.transform.position.x, initialX, "Hero did not move to the left!");

        yield return null;
    }

    /// <summary>
    /// Test ST-CI3: Checks functionality of character movement along the z-axis (up).
    /// Requirement: FR-4
    /// </summary>
    [UnityTest]
    public IEnumerator CharacterInput_MovesAlongZAxis_Up()
    {
        // Wait for test scene to be loaded
        yield return new WaitForSeconds(timeToWait);

        // Setup Hero GameObject
        GameObject heroes = GameObject.Find("Heroes");
        GameObject hero = heroes.transform.Find("Hero1").gameObject;
        Assert.IsNotNull(hero, "Hero object is null!");
        hero.SetActive(true);

        // Get HeroController
        HeroController heroController = hero.GetComponent<HeroController>();
        Assert.IsNotNull(heroController, "HeroController is null!");

        // Get initial x-position
        float initialZ = hero.transform.position.z;

        // Substitute UnityService to mock player movement input
        var unityService = Substitute.For<IUnityService>();
        unityService.GetAxisRaw("Horizontal").Returns(0);
        unityService.GetAxisRaw("Vertical").Returns(1);     // move player along z-axis
        heroController.unityService = unityService;

        // Allow test to run for x amount of seconds
        yield return new WaitForSeconds(timeToWait);

        // Assert that hero has moved up as expected
        Assert.Greater(hero.transform.position.z, initialZ, "Hero did not move up!");

        yield return null;
    }

    /// <summary>
    /// Test ST-CI4: Checks functionality of character movement along the z-axis (down).
    /// Requirement: FR-4
    /// </summary>
    [UnityTest]
    public IEnumerator CharacterInput_MovesAlongZAxis_Down()
    {
        // Wait for test scene to be loaded
        yield return new WaitForSeconds(timeToWait);

        // Setup Hero GameObject
        GameObject heroes = GameObject.Find("Heroes");
        GameObject hero = heroes.transform.Find("Hero1").gameObject;
        Assert.IsNotNull(hero, "Hero object is null!");
        hero.SetActive(true);

        // Get HeroController
        HeroController heroController = hero.GetComponent<HeroController>();
        Assert.IsNotNull(heroController, "HeroController is null!");

        // Get initial x-position
        float initialZ = hero.transform.position.z;

        // Substitute UnityService to mock player movement input
        var unityService = Substitute.For<IUnityService>();
        unityService.GetAxisRaw("Horizontal").Returns(0);
        unityService.GetAxisRaw("Vertical").Returns(-1);     // move player along z-axis
        heroController.unityService = unityService;

        // Allow test to run for x amount of seconds
        yield return new WaitForSeconds(timeToWait);

        // Assert that hero has moved down as expected
        Assert.Less(hero.transform.position.z, initialZ, "Hero did not move up!");

        yield return null;
    }

    /// <summary>
    /// Test ST-CI5: Checks functionality of hero basic attack, and that a basic attack is performed when space is pressed.
    /// Requirement: FR-18
    /// </summary>
    [UnityTest]
    public IEnumerator CharacterInput_PerformBasicAttack()
    {
        // Wait for test scene to be loaded
        yield return new WaitForSeconds(timeToWait);

        // Setup Hero GameObject
        GameObject heroes = GameObject.Find("Heroes");
        GameObject hero = heroes.transform.Find("Hero1").gameObject;
        Assert.IsNotNull(hero, "Hero object is null!");
        hero.SetActive(true);

        // Get HeroController
        HeroController heroController = hero.GetComponent<HeroController>();
        Assert.IsNotNull(heroController, "HeroController is null!");

        // Substitute CharacterCombat in HeroController
        var heroCombat = Substitute.For<CharacterCombat>();
        heroController.heroCombat = heroCombat;

        // Substitute UnityService to mock player movement input
        var unityService = Substitute.For<IUnityService>();
        unityService.GetKeyDown(KeyCode.Space);             // mock player pressing space
        heroController.unityService = unityService;

        // Allow test to run for x amount of seconds
        yield return new WaitForSeconds(timeToWait);

        // Assert that heroCombat has received a call to CmdAttack()
        LogAssert.Expect(LogType.Error, "Command function CmdAttack called on server.");    // TODO: TEMPORARY, REMOVE LATER
        heroCombat.Received().CmdAttack();

        yield return null;
    }

}
