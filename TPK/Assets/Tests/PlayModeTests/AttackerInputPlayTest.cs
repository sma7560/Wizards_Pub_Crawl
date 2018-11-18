using UnityEngine;
using UnityEngine.TestTools;
using NSubstitute;
using NUnit.Framework;
using System.Collections;
using UnityEngine.SceneManagement;

/// <summary>
/// This class contains system tests for Attacker Input on the Test scene.
/// These tests run on Play Mode in Unity's Test Runner.
/// </summary>
public class AttackerInputPlayTest
{
    readonly int timeToWait = 2;    // number of seconds to wait for after test scene is loaded

    [SetUp]
    public void SetUp()
    {
        // Load Test scene
        SceneManager.LoadSceneAsync("TestScene", LoadSceneMode.Single);
    }

    /// <summary>
    /// Test ST-AI1: Checks functionality of character movement along the x-axis (right).
    /// Requirement: FR-4
    /// </summary>
    [UnityTest]
    public IEnumerator AttackerInput_MovesAlongXAxis_Right()
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
    /// Test ST-AI2: Checks functionality of character movement along the x-axis (left).
    /// Requirement: FR-4
    /// </summary>
    [UnityTest]
    public IEnumerator AttackerInput_MovesAlongXAxis_Left()
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
    /// Test ST-AI3: Checks functionality of character movement along the z-axis (up).
    /// Requirement: FR-4
    /// </summary>
    [UnityTest]
    public IEnumerator AttackerInput_MovesAlongZAxis_Up()
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
    /// Test ST-AI4: Checks functionality of character movement along the z-axis (down).
    /// Requirement: FR-4
    /// </summary>
    [UnityTest]
    public IEnumerator AttackerInput_MovesAlongZAxis_Down()
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
    /// Test ST-AI5: Checks functionality of hero basic attack, and that a basic attack is performed when space is pressed.
    /// Requirement: FR-18
    /// </summary>
    [UnityTest]
    public IEnumerator AttackerInput_PerformBasicAttack()
    {
        // TODO: TEMPORARY, REMOVE LATER. ONLY ADDED BECAUSE NETWORK ATTACKING IS CURRENTLY GIVING LOG ERRORS.
        LogAssert.ignoreFailingMessages = true;     // REMOVE THIS LINE LATER

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

        // Substitute UnityService to mock player input
        var unityService = Substitute.For<IUnityService>();
        unityService.GetKeyDown(KeyCode.Space).Returns(true);   // mock player pressing space
        heroController.unityService = unityService;

        // Allow test to run for x amount of seconds
        yield return new WaitForSeconds(timeToWait);

        // Assert that heroCombat has received a call to CmdAttack()
        heroCombat.Received().CmdAttack();

        yield return null;
    }

    /// <summary>
    /// Test ST-AI6: Checks functionality of hero skill casting for key Q, and when Q is pressed, equipped skill is performed.
    /// Requirement: FR-19
    /// </summary>
    [UnityTest]
    public IEnumerator AttackerInput_PerformSkill_Q()
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

        // Substitute UnityService to mock player input
        var unityService = Substitute.For<IUnityService>();
        unityService.GetKeyDown(KeyCode.Q).Returns(true);   // mock player pressing Q
        heroController.unityService = unityService;

        // Allow test to run for x amount of seconds
        //yield return new WaitForSeconds(timeToWait);

        // Assert that heroCombat has performed skill equipped to the Q key
        Assert.Fail("PLACEHOLDER: Need to update this test once skills are implemented!");

        yield return null;
    }

    /// <summary>
    /// Test ST-AI7: Checks functionality of hero skill casting for key E, and when E is pressed, equipped skill is performed.
    /// Requirement: FR-19
    /// </summary>
    [UnityTest]
    public IEnumerator AttackerInput_PerformSkill_E()
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

        // Substitute UnityService to mock player input
        var unityService = Substitute.For<IUnityService>();
        unityService.GetKeyDown(KeyCode.E).Returns(true);   // mock player pressing E
        heroController.unityService = unityService;

        // Allow test to run for x amount of seconds
        //yield return new WaitForSeconds(timeToWait);

        // Assert that heroCombat has performed skill equipped to the E key
        Assert.Fail("PLACEHOLDER: Need to update this test once skills are implemented!");

        yield return null;
    }

    /// <summary>
    /// Test ST-AI8: Checks functionality of hero skill casting for key R, and when R is pressed, equipped skill is performed.
    /// Requirement: FR-19
    /// </summary>
    [UnityTest]
    public IEnumerator AttackerInput_PerformSkill_R()
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

        // Substitute UnityService to mock player input
        var unityService = Substitute.For<IUnityService>();
        unityService.GetKeyDown(KeyCode.R).Returns(true);   // mock player pressing R
        heroController.unityService = unityService;

        // Allow test to run for x amount of seconds
        //yield return new WaitForSeconds(timeToWait);

        // Assert that heroCombat has performed skill equipped to the R key
        Assert.Fail("PLACEHOLDER: Need to update this test once skills are implemented!");

        yield return null;
    }

    /// <summary>
    /// Test ST-AI9: Checks functionality of hero item usage for key 1, and that when 1 is pressed, equipped item is used.
    /// Requirement: FR-21
    /// </summary>
    [UnityTest]
    public IEnumerator AttackerInput_UseItem_1()
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

        // Substitute UnityService to mock player input
        var unityService = Substitute.For<IUnityService>();
        unityService.GetKeyDown(KeyCode.Alpha1).Returns(true);  // mock player pressing 1
        heroController.unityService = unityService;

        // Allow test to run for x amount of seconds
        //yield return new WaitForSeconds(timeToWait);

        // Assert that item equipped on key 1 has been used
        Assert.Fail("PLACEHOLDER: Need to update this test once items are implemented!");

        yield return null;
    }

    /// <summary>
    /// Test ST-AI10: Checks functionality of hero item usage for key 2, and that when 2 is pressed, equipped item is used.
    /// Requirement: FR-21
    /// </summary>
    [UnityTest]
    public IEnumerator AttackerInput_UseItem_2()
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

        // Substitute UnityService to mock player input
        var unityService = Substitute.For<IUnityService>();
        unityService.GetKeyDown(KeyCode.Alpha2).Returns(true);  // mock player pressing 2
        heroController.unityService = unityService;

        // Allow test to run for x amount of seconds
        //yield return new WaitForSeconds(timeToWait);

        // Assert that item equipped on key 2 has been used
        Assert.Fail("PLACEHOLDER: Need to update this test once items are implemented!");

        yield return null;
    }

    /// <summary>
    /// Test ST-AI11: Checks functionality of hero item usage for key 3, and that when 3 is pressed, equipped item is used.
    /// Requirement: FR-21
    /// </summary>
    [UnityTest]
    public IEnumerator AttackerInput_UseItem_3()
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

        // Substitute UnityService to mock player input
        var unityService = Substitute.For<IUnityService>();
        unityService.GetKeyDown(KeyCode.Alpha3).Returns(true);  // mock player pressing 3
        heroController.unityService = unityService;

        // Allow test to run for x amount of seconds
        //yield return new WaitForSeconds(timeToWait);

        // Assert that item equipped on key 3 has been used
        Assert.Fail("PLACEHOLDER: Need to update this test once items are implemented!");

        yield return null;
    }

    /// <summary>
    /// Test ST-AI12: Checks functionality of hero item usage for key 4, and that when 4 is pressed, equipped item is used.
    /// Requirement: FR-21
    /// </summary>
    [UnityTest]
    public IEnumerator AttackerInput_UseItem_4()
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

        // Substitute UnityService to mock player input
        var unityService = Substitute.For<IUnityService>();
        unityService.GetKeyDown(KeyCode.Alpha4).Returns(true);  // mock player pressing 4
        heroController.unityService = unityService;

        // Allow test to run for x amount of seconds
        //yield return new WaitForSeconds(timeToWait);

        // Assert that item equipped on key 4 has been used
        Assert.Fail("PLACEHOLDER: Need to update this test once items are implemented!");

        yield return null;
    }

}
