using NSubstitute;
using NUnit.Framework;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;

/// <summary>
/// This class contains system tests for player input on the TestScene.
/// These tests run on Play Mode in Unity's Test Runner.
/// NOTE: Currently fails because setting up of all components in the TestScene without a network is VERY difficult. However, the idea of system testing is there.
/// </summary>
public class PlayerDungeonPlayTest
{
    private readonly int timeToWait = 2;    // number of seconds to wait for after test scene is loaded

    [UnitySetUp]
    public IEnumerator SetUp()
    {
        // Load Test scene
        SceneManager.LoadSceneAsync("TestScene", LoadSceneMode.Single);

        // Wait for test scene to be loaded
        yield return new WaitForSeconds(timeToWait);

        // Enable necessary components
        GameObject.Find("DungeonActivate").transform.Find("MatchManager").gameObject.SetActive(true);
        GameObject.Find("DungeonActivate").transform.Find("Player").gameObject.SetActive(true);

        // Wait for components to become active
        yield return new WaitForSeconds(timeToWait);
    }

    /// <summary>
    /// Checks functionality of character movement along the x-axis (right).
    /// </summary>
    [UnityTest]
    public IEnumerator PlayerDungeon_MovesAlongXAxis_Right()
    {
        // Get player object
        GameObject player = GameObject.Find("Player");
        Assert.IsNotNull(player, "Player object is null!");
        HeroController heroController = player.GetComponent<HeroController>();
        Assert.IsNotNull(heroController, "HeroController is null!");

        // Get initial x-position
        float initialX = player.transform.position.x;
        
        // Substitute UnityService to mock player movement input
        var unityService = Substitute.For<IUnityService>();
        unityService.GetKey(CustomKeyBinding.GetRightKey()).Returns(true);
        heroController.unityService = unityService;

        // Allow test to run for x amount of seconds
        yield return new WaitForSeconds(timeToWait);

        // Assert that hero has moved to the right as expected
        Assert.Greater(player.transform.position.x, initialX, "Hero did not move to the right!");
    }

    /// <summary>
    /// Checks functionality of character movement along the x-axis (left).
    /// </summary>
    [UnityTest]
    public IEnumerator PlayerDungeon_MovesAlongXAxis_Left()
    {
        // Get player object
        GameObject player = GameObject.Find("Player");
        Assert.IsNotNull(player, "Player object is null!");
        HeroController heroController = player.GetComponent<HeroController>();
        Assert.IsNotNull(heroController, "HeroController is null!");

        // Get initial x-position
        float initialX = player.transform.position.x;

        // Substitute UnityService to mock player movement input
        var unityService = Substitute.For<IUnityService>();
        unityService.GetKey(CustomKeyBinding.GetLeftKey()).Returns(true);
        heroController.unityService = unityService;

        // Allow test to run for x amount of seconds
        yield return new WaitForSeconds(timeToWait);

        // Assert that hero has moved to the right as expected
        Assert.Less(player.transform.position.x, initialX, "Hero did not move to the left!");
    }

    /// <summary>
    /// Checks functionality of character movement along the z-axis (up).
    /// </summary>
    [UnityTest]
    public IEnumerator PlayerDungeon_MovesAlongZAxis_Up()
    {
        // Get player object
        GameObject player = GameObject.Find("Player");
        Assert.IsNotNull(player, "Player object is null!");
        HeroController heroController = player.GetComponent<HeroController>();
        Assert.IsNotNull(heroController, "HeroController is null!");

        // Get initial x-position
        float initialZ = player.transform.position.z;

        // Substitute UnityService to mock player movement input
        var unityService = Substitute.For<IUnityService>();
        unityService.GetKey(CustomKeyBinding.GetForwardKey()).Returns(true);
        heroController.unityService = unityService;

        // Allow test to run for x amount of seconds
        yield return new WaitForSeconds(timeToWait);

        // Assert that hero has moved to the right as expected
        Assert.Greater(player.transform.position.z, initialZ, "Hero did not move to the up!");
    }

    /// <summary>
    /// Checks functionality of character movement along the z-axis (down).
    /// </summary>
    [UnityTest]
    public IEnumerator PlayerDungeon_MovesAlongZAxis_Down()
    {
        // Get player object
        GameObject player = GameObject.Find("Player");
        Assert.IsNotNull(player, "Player object is null!");
        HeroController heroController = player.GetComponent<HeroController>();
        Assert.IsNotNull(heroController, "HeroController is null!");

        // Get initial x-position
        float initialZ = player.transform.position.z;

        // Substitute UnityService to mock player movement input
        var unityService = Substitute.For<IUnityService>();
        unityService.GetKey(CustomKeyBinding.GetBackKey()).Returns(true);
        heroController.unityService = unityService;

        // Allow test to run for x amount of seconds
        yield return new WaitForSeconds(timeToWait);

        // Assert that hero has moved to the right as expected
        Assert.Less(player.transform.position.z, initialZ, "Hero did not move to the down!");
    }

    /// <summary>
    /// Checks functionality of hero basic attack, and that a basic attack is performed when appropriate key is pressed.
    /// </summary>
    [UnityTest]
    public IEnumerator PlayerDungeon_PerformBasicAttack()
    {
        // Get player object
        GameObject player = GameObject.Find("Player");
        Assert.IsNotNull(player, "Player object is null!");
        HeroController heroController = player.GetComponent<HeroController>();
        Assert.IsNotNull(heroController, "HeroController is null!");

        // Substitute UnityService to mock player input
        var unityService = Substitute.For<IUnityService>();
        unityService.GetKey(CustomKeyBinding.GetBasicAttackKey()).Returns(true);
        heroController.unityService = unityService;

        // Allow test to run for x amount of seconds
        yield return new WaitForSeconds(timeToWait);

        // Assert that basic attack projectile is on screen
        Assert.IsNotNull(GameObject.FindGameObjectWithTag("Projectile"), "No projectile detected!");
    }

    /// <summary>
    /// Checks functionality of hero skill casting for skill equipped in first skill slot.
    /// </summary>
    [UnityTest]
    public IEnumerator PlayerDungeon_CastSkill1()
    {
        // Get player object
        GameObject player = GameObject.Find("Player");
        Assert.IsNotNull(player, "Player object is null!");
        AbilityManager abilityManager = player.GetComponent<AbilityManager>();
        Assert.IsNotNull(abilityManager, "AbilityManager is null!");

        // Equip skill to cast in first skill slot
        abilityManager.EquipSkill(abilityManager.knownSkills[0], 0);

        // Substitute UnityService to mock player input
        var unityService = Substitute.For<IUnityService>();
        unityService.GetKeyDown(CustomKeyBinding.GetSkill1Key()).Returns(true);
        abilityManager.unityService = unityService;

        // Allow test to run for x amount of seconds
        yield return new WaitForSeconds(timeToWait);

        // Assert that skill has been cast
        Assert.IsNotNull(GameObject.FindGameObjectWithTag("Projectile"), "No projectile detected!");
    }

    /// <summary>
    /// Checks functionality of hero skill casting for skill equipped in second skill slot.
    /// </summary>
    [UnityTest]
    public IEnumerator PlayerDungeon_CastSkill2()
    {
        // Get player object
        GameObject player = GameObject.Find("Player");
        Assert.IsNotNull(player, "Player object is null!");
        AbilityManager abilityManager = player.GetComponent<AbilityManager>();
        Assert.IsNotNull(abilityManager, "AbilityManager is null!");

        // Equip skill to cast in second skill slot
        abilityManager.EquipSkill(abilityManager.knownSkills[0], 1);

        // Substitute UnityService to mock player input
        var unityService = Substitute.For<IUnityService>();
        unityService.GetKeyDown(CustomKeyBinding.GetSkill2Key()).Returns(true);
        abilityManager.unityService = unityService;

        // Allow test to run for x amount of seconds
        yield return new WaitForSeconds(timeToWait);

        // Assert that skill has been cast
        Assert.IsNotNull(GameObject.FindGameObjectWithTag("Projectile"), "No projectile detected!");
    }

    /// <summary>
    /// Checks functionality of hero skill casting for skill equipped in third skill slot.
    /// </summary>
    [UnityTest]
    public IEnumerator PlayerDungeon_CastSkill3()
    {
        // Get player object
        GameObject player = GameObject.Find("Player");
        Assert.IsNotNull(player, "Player object is null!");
        AbilityManager abilityManager = player.GetComponent<AbilityManager>();
        Assert.IsNotNull(abilityManager, "AbilityManager is null!");

        // Equip skill to cast in third skill slot
        abilityManager.EquipSkill(abilityManager.knownSkills[0], 2);

        // Substitute UnityService to mock player input
        var unityService = Substitute.For<IUnityService>();
        unityService.GetKeyDown(CustomKeyBinding.GetSkill3Key()).Returns(true);
        abilityManager.unityService = unityService;

        // Allow test to run for x amount of seconds
        yield return new WaitForSeconds(timeToWait);

        // Assert that skill has been cast
        Assert.IsNotNull(GameObject.FindGameObjectWithTag("Projectile"), "No projectile detected!");
    }

    /// <summary>
    /// Checks functionality of hero skill casting for skill equipped in fourth skill slot.
    /// </summary>
    [UnityTest]
    public IEnumerator PlayerDungeon_CastSkill4()
    {
        // Get player object
        GameObject player = GameObject.Find("Player");
        Assert.IsNotNull(player, "Player object is null!");
        AbilityManager abilityManager = player.GetComponent<AbilityManager>();
        Assert.IsNotNull(abilityManager, "AbilityManager is null!");

        // Equip skill to cast in fourth skill slot
        abilityManager.EquipSkill(abilityManager.knownSkills[0], 3);

        // Substitute UnityService to mock player input
        var unityService = Substitute.For<IUnityService>();
        unityService.GetKeyDown(CustomKeyBinding.GetSkill4Key()).Returns(true);
        abilityManager.unityService = unityService;

        // Allow test to run for x amount of seconds
        yield return new WaitForSeconds(timeToWait);

        // Assert that skill has been cast
        Assert.IsNotNull(GameObject.FindGameObjectWithTag("Projectile"), "No projectile detected!");
    }
}
