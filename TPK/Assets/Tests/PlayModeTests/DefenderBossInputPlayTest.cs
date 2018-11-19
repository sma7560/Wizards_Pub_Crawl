using UnityEngine;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;
using NSubstitute;
using UnityEngine.SceneManagement;

/// <summary>
/// This class contains system tests for Defender Boss Input.
/// These tests run on Play Mode in Unity's Test Runner.
/// TODO: Implement tests once defender boss is implemented.
/// </summary>
public class DefenderBossInputPlayTest
{
    readonly int timeToWait = 2;    // number of seconds to wait for after test scene is loaded

    [UnitySetUp]
    public IEnumerator UnitySetUp()
    {
        // Load Test scene
        SceneManager.LoadSceneAsync("TestScene", LoadSceneMode.Single);

        // TODO: Initialize boss mode

        // Wait for defender status to be initialized
        yield return new WaitForSeconds(timeToWait);
    }

    /// <summary>
    /// Test ST-DB1: Checks functionality of character movement along the x-axis (right).
    /// Requirement: FR-47
    /// </summary>
    [UnityTest]
    public IEnumerator DefenderBossInput_MovesAlongXAxis_Right()
    {
        // TODO: implement this test when boss is implemented

        //// Setup Boss GameObject
        //GameObject boss = GameObject.Find("Boss").gameObject;
        //BossController bossController = boss.GetComponent<BossController>();

        //// Get initial x-position
        //float initialX = boss.transform.position.x;

        //// Substitute UnityService to mock player movement input
        //var unityService = Substitute.For<IUnityService>();
        //unityService.GetAxisRaw("Horizontal").Returns(1);   // move player on x-axis
        //unityService.GetAxisRaw("Vertical").Returns(0);
        //bossController.unityService = unityService;

        //// Allow test to run for x amount of seconds
        //yield return new WaitForSeconds(timeToWait);

        //// Assert that character has moved to the right as expected
        //Assert.Greater(boss.transform.position.x, initialX, "Boss did not move to the right!");

        Assert.Fail("PLACEHOLDER: Implement this test when boss mechanics are added");

        yield return null;
    }

    /// <summary>
    /// Test ST-DB2: Checks functionality of character movement along the x-axis (left).
    /// Requirement: FR-47
    /// </summary>
    [UnityTest]
    public IEnumerator DefenderBossInput_MovesAlongXAxis_Left()
    {
        // TODO: implement this test when boss is implemented

        //// Setup Boss GameObject
        //GameObject boss = GameObject.Find("Boss").gameObject;
        //BossController bossController = boss.GetComponent<BossController>();

        //// Get initial x-position
        //float initialX = boss.transform.position.x;

        //// Substitute UnityService to mock player movement input
        //var unityService = Substitute.For<IUnityService>();
        //unityService.GetAxisRaw("Horizontal").Returns(-1);   // move player on x-axis
        //unityService.GetAxisRaw("Vertical").Returns(0);
        //bossController.unityService = unityService;

        //// Allow test to run for x amount of seconds
        //yield return new WaitForSeconds(timeToWait);

        //// Assert that character has moved to the right as expected
        //Assert.Less(boss.transform.position.x, initialX, "Boss did not move to the left!");

        Assert.Fail("PLACEHOLDER: Implement this test when boss mechanics are added");

        yield return null;
    }

    /// <summary>
    /// Test ST-DB3: Checks functionality of character movement along the z-axis (up).
    /// Requirement: FR-47
    /// </summary>
    [UnityTest]
    public IEnumerator DefenderBossInput_MovesAlongZAxis_Up()
    {
        // TODO: implement this test when boss is implemented

        //// Setup Boss GameObject
        //GameObject boss = GameObject.Find("Boss").gameObject;
        //BossController bossController = boss.GetComponent<BossController>();

        //// Get initial z-position
        //float initialZ = boss.transform.position.z;

        //// Substitute UnityService to mock player movement input
        //var unityService = Substitute.For<IUnityService>();
        //unityService.GetAxisRaw("Horizontal").Returns(0);
        //unityService.GetAxisRaw("Vertical").Returns(1);
        //bossController.unityService = unityService;

        //// Allow test to run for x amount of seconds
        //yield return new WaitForSeconds(timeToWait);

        //// Assert that character has moved up as expected
        //Assert.Greater(boss.transform.position.z, initialZ, "Boss did not move up!");

        Assert.Fail("PLACEHOLDER: Implement this test when boss mechanics are added");

        yield return null;
    }

    /// <summary>
    /// Test ST-DB4: Checks functionality of character movement along the z-axis (down).
    /// Requirement: FR-47
    /// </summary>
    [UnityTest]
    public IEnumerator DefenderBossInput_MovesAlongZAxis_Down()
    {
        // TODO: implement this test when boss is implemented

        //// Setup Boss GameObject
        //GameObject boss = GameObject.Find("Boss").gameObject;
        //BossController bossController = boss.GetComponent<BossController>();

        //// Get initial z-position
        //float initialZ = boss.transform.position.z;

        //// Substitute UnityService to mock player movement input
        //var unityService = Substitute.For<IUnityService>();
        //unityService.GetAxisRaw("Horizontal").Returns(0);
        //unityService.GetAxisRaw("Vertical").Returns(-1);
        //bossController.unityService = unityService;

        //// Allow test to run for x amount of seconds
        //yield return new WaitForSeconds(timeToWait);

        //// Assert that character has moved down as expected
        //Assert.Less(boss.transform.position.z, initialZ, "Boss did not move down!");

        Assert.Fail("PLACEHOLDER: Implement this test when boss mechanics are added");

        yield return null;
    }

    /// <summary>
    /// Test ST-DB5: Checks functionality of boss basic attack, and that a basic attack is performed when space is pressed.
    /// Requirement: FR-48
    /// </summary>
    [UnityTest]
    public IEnumerator DefenderBossInput_PerformBasicAttack()
    {
        //// Setup Boss GameObject
        //GameObject boss = GameObject.Find("Boss").gameObject;
        //BossController bossController = boss.GetComponent<BossController>();

        //// Substitute CharacterCombat in HeroController
        //var bossCombat = Substitute.For<CharacterCombat>();
        //bossController.heroCombat = bossCombat;

        //// Substitute UnityService to mock player input
        //var unityService = Substitute.For<IUnityService>();
        //unityService.GetKeyDown(KeyCode.Space).Returns(true);   // mock player pressing space
        //bossController.unityService = unityService;

        //// Allow test to run for x amount of seconds
        //yield return new WaitForSeconds(timeToWait);
        //unityService.GetKeyDown(KeyCode.Space).Returns(false);  // disable space mock input

        //// Assert that heroCombat has received a call to CmdAttack()
        //bossCombat.Received().CmdAttack();

        Assert.Fail("PLACEHOLDER: Implement this test when boss mechanics are added");

        yield return null;
    }

    /// <summary>
    /// Test ST-DB6: Checks functionality of boss skill casting for key 1, and when 1 is pressed, equipped skill is performed.
    /// Requirement: FR-49
    /// </summary>
    [UnityTest]
    public IEnumerator DefenderBossInput_PerformSkill_1()
    {
        // TODO: update test once logic is implemented

        //// Setup Hero GameObject
        //GameObject boss = GameObject.Find("Boss").gameObject;
        //BossController bossController = boss.GetComponent<BossController>();

        //// Substitute UnityService to mock player input
        //var unityService = Substitute.For<IUnityService>();
        //unityService.GetKeyDown(KeyCode.Alpha1).Returns(true);  // mock player pressing 1
        //bossController.unityService = unityService;

        //// Allow test to run for x amount of seconds
        ////yield return new WaitForSeconds(timeToWait);

        //// Assert that boss has performed skill equipped to the 1 key
        Assert.Fail("PLACEHOLDER: Need to update this test once boss is implemented!");

        yield return null;
    }

    /// <summary>
    /// Test ST-DB7: Checks functionality of boss skill casting for key 2, and when 2 is pressed, equipped skill is performed.
    /// Requirement: FR-49
    /// </summary>
    [UnityTest]
    public IEnumerator DefenderBossInput_PerformSkill_2()
    {
        // TODO: update test once logic is implemented

        //// Setup Hero GameObject
        //GameObject boss = GameObject.Find("Boss").gameObject;
        //BossController bossController = boss.GetComponent<BossController>();

        //// Substitute UnityService to mock player input
        //var unityService = Substitute.For<IUnityService>();
        //unityService.GetKeyDown(KeyCode.Alpha2).Returns(true);  // mock player pressing 2
        //bossController.unityService = unityService;

        //// Allow test to run for x amount of seconds
        ////yield return new WaitForSeconds(timeToWait);

        //// Assert that boss has performed skill equipped to the 2 key
        Assert.Fail("PLACEHOLDER: Need to update this test once boss is implemented!");

        yield return null;
    }
}
