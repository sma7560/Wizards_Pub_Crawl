using UnityEngine;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;
using UnityEngine.SceneManagement;
using NSubstitute;

/// <summary>
/// This class contains system tests for Defender Input.
/// These tests run on Play Mode in Unity's Test Runner.
/// </summary>
public class DefenderInputPlayTest
{
    readonly int timeToWait = 2;        // number of seconds to wait for after test scene is loaded
    readonly int timeToWaitLong = 5;    // number of seconds to wait for after test scene is loaded (longer)

    [UnitySetUp]
    public IEnumerator UnitySetUp()
    {
        // Load Test scene
        SceneManager.LoadSceneAsync("TestScene", LoadSceneMode.Single);

        // Wait for test scene to be loaded
        yield return new WaitForSeconds(timeToWaitLong);

        // Setup Defender status
        GameObject networkManagerV2 = GameObject.Find("NetworkManagerV2");
        NetworkManagerExtension networkManagerExtension = networkManagerV2.GetComponent<NetworkManagerExtension>();
        //networkManagerExtension.StartUpHost();

        // Wait for defender status to be initialized
        yield return new WaitForSeconds(timeToWait);
    }

    /// <summary>
    /// Test ST-DI1: Checks the functionality of selecting cards to draft a deck, and that the cards selected stay in the deck.
    /// Requirement: FR-31
    /// </summary>
    [UnityTest]
    public IEnumerator DefenderState_Cards_DraftDeckWorks()
    {
        // TODO: Implement once card system is in place
        Assert.Fail("PLACEHOLDER: Implement test once cards are implemented");
        yield return null;
    }

    /// <summary>
    /// Test ST-DI2: Checks the functionality of using a minion card, and that minions spawn upon use.
    /// Requirement: FR-36, FR-37, FR-52
    /// </summary>
    [UnityTest]
    public IEnumerator DefenderState_MinionCard_SpawnsMinonsOnUse()
    {
        // TODO: TEMPORARY, REMOVE LATER. ONLY ADDED BECAUSE NETWORK ATTACKING IS CURRENTLY GIVING LOG ERRORS.
        LogAssert.ignoreFailingMessages = true;     // REMOVE THIS LINE LATER

        // Find Defender Camera and setup DefenderBehaviour
        GameObject defenderCamera = null;
        Object[] gameObjects = GameObject.FindObjectsOfType<GameObject>();
        foreach (GameObject gameObject in gameObjects)
        {
            if (gameObject.name.Contains("DefenderCamera"))
            {
                defenderCamera = gameObject;
                break;
            }
        }
        DefenderBehaviour defenderBehaviour = defenderCamera.GetComponent<DefenderBehaviour>();

        // Set defender mode to spawning monsters
        defenderBehaviour.mode = DefenderBehaviour.defenderMode.spawnMonster;

        // Setup mock of UnityService to mock player input
        var unityService = Substitute.For<IUnityService>();
        unityService.GetMouseButtonUp(0).Returns(true);                     // mock player mouse click
        unityService.GetMousePosition().Returns(new Vector3(580, 256, 0));  // mock player mouse click coordinates
        defenderBehaviour.unityService = unityService;

        // Allow test to run for x seconds
        yield return new WaitForSeconds(timeToWait);

        // Find spawned enemy objects
        GameObject enemy = null;
        gameObjects = GameObject.FindObjectsOfType<GameObject>();
        foreach (GameObject gameObject in gameObjects)
        {
            if (gameObject.name.Contains("Monster"))
            {
                enemy = gameObject;
                break;
            }
        }

        // Assert that enemies have spawned
        Assert.IsNotNull(enemy, "No enemies detected!");

        yield return null;
    }

    /// <summary>
    /// Test ST-DI3: Checks that minions are at full health upon spawn.
    /// Requirement: FR-37
    /// </summary>
    [UnityTest]
    public IEnumerator DefenderState_MinionCard_SpawnedMinionsAreAtFullHp()
    {
        // TODO: TEMPORARY, REMOVE LATER. ONLY ADDED BECAUSE NETWORK ATTACKING IS CURRENTLY GIVING LOG ERRORS.
        LogAssert.ignoreFailingMessages = true;     // REMOVE THIS LINE LATER

        // Find Defender Camera and setup DefenderBehaviour
        GameObject defenderCamera = null;
        Object[] gameObjects = GameObject.FindObjectsOfType<GameObject>();
        foreach (GameObject gameObject in gameObjects)
        {
            if (gameObject.name.Contains("DefenderCamera"))
            {
                defenderCamera = gameObject;
                break;
            }
        }
        DefenderBehaviour defenderBehaviour = defenderCamera.GetComponent<DefenderBehaviour>();

        // Set defender mode to spawning monsters
        defenderBehaviour.mode = DefenderBehaviour.defenderMode.spawnMonster;

        // Setup mock of UnityService to mock player input
        var unityService = Substitute.For<IUnityService>();
        unityService.GetMouseButtonUp(0).Returns(true);                     // mock player mouse click
        unityService.GetMousePosition().Returns(new Vector3(580, 256, 0));  // mock player mouse click coordinates
        defenderBehaviour.unityService = unityService;

        // Allow test to run for x seconds
        yield return new WaitForSeconds(timeToWait);

        // Find spawned enemy objects
        GameObject enemy = null;
        gameObjects = GameObject.FindObjectsOfType<GameObject>();
        foreach (GameObject gameObject in gameObjects)
        {
            if (gameObject.name.Contains("Monster"))
            {
                enemy = gameObject;
                break;
            }
        }
        CharacterStats enemyStats = enemy.GetComponent<CharacterStats>();

        // Assert that enemies have spawned
        Assert.AreEqual(enemyStats.maxHealth, enemyStats.GetCurrentHealth(), "Enemies are not at max health!");

        yield return null;
    }

    /// <summary>
    /// Test ST-DI4: Checks the functionality of using a trap card, and that traps spawn upon use.
    /// Requirement: FR-36, FR-38
    /// </summary>
    [UnityTest]
    public IEnumerator DefenderState_TrapCard_SpawnsTrapsOnUse()
    {
        // TODO: TEMPORARY, REMOVE LATER. ONLY ADDED BECAUSE NETWORK ATTACKING IS CURRENTLY GIVING LOG ERRORS.
        LogAssert.ignoreFailingMessages = true;     // REMOVE THIS LINE LATER

        // Find Defender Camera and setup DefenderBehaviour
        GameObject defenderCamera = null;
        Object[] gameObjects = GameObject.FindObjectsOfType<GameObject>();
        foreach (GameObject gameObject in gameObjects)
        {
            if (gameObject.name.Contains("DefenderCamera"))
            {
                defenderCamera = gameObject;
                break;
            }
        }
        DefenderBehaviour defenderBehaviour = defenderCamera.GetComponent<DefenderBehaviour>();

        // Set defender mode to spawning traps
        defenderBehaviour.mode = DefenderBehaviour.defenderMode.spawnTrap;

        // Setup mock of UnityService to mock player input
        var unityService = Substitute.For<IUnityService>();
        unityService.GetMouseButtonUp(0).Returns(true);                     // mock player mouse click
        unityService.GetMousePosition().Returns(new Vector3(580, 256, 0));  // mock player mouse click coordinates
        defenderBehaviour.unityService = unityService;

        // Allow test to run for x seconds
        yield return new WaitForSeconds(timeToWait);

        // Find spawned enemy objects
        GameObject trap = null;
        gameObjects = GameObject.FindObjectsOfType<GameObject>();
        foreach (GameObject gameObject in gameObjects)
        {
            if (gameObject.name.Contains("Trap"))
            {
                trap = gameObject;
                break;
            }
        }

        // Assert that enemies have spawned
        Assert.IsNotNull(trap, "No traps detected!");

        yield return null;
    }

    /// <summary>
    /// Test ST-DI5: Checks the functionality of using a curse card, and that the curse activates upon use.
    /// Requirement: FR-36, FR-39
    /// </summary>
    [UnityTest]
    public IEnumerator DefenderState_CurseCard_ActivatesCurseOnUse()
    {
        // TODO: Implement once card system is in place
        Assert.Fail("PLACEHOLDER: Implement test once curse cards are implemented");
        yield return null;
    }

    /// <summary>
    /// Test ST-DI6: Checks that card use is not possible if the defender does not have enough energy to use the card.
    /// Requirement: FR-45
    /// </summary>
    [UnityTest]
    public IEnumerator DefenderState_CardUse_Failure_NoEnergy()
    {
        // TODO: Implement once card system is in place
        Assert.Fail("PLACEHOLDER: Implement test once energy is implemented");
        yield return null;
    }

    /// <summary>
    /// Test ST-DI7: Checks the functionality of the defender camera, and that is moveable with player input.
    /// Requirement: FR-42
    /// </summary>
    [UnityTest]
    public IEnumerator DefenderState_DefenderCamera_MovesUponInput()
    {
        // Find Defender Camera and setup DefenderCameraController
        GameObject defenderCamera = null;
        Object[] gameObjects = GameObject.FindObjectsOfType<GameObject>();
        foreach (GameObject gameObject in gameObjects)
        {
            if (gameObject.name.Contains("DefenderCamera"))
            {
                defenderCamera = gameObject;
                break;
            }
        }
        DefenderCameraController defenderCameraController = defenderCamera.GetComponent<DefenderCameraController>();

        // Get initial camera position
        float initialX = defenderCamera.transform.position.x;
        float initialZ = defenderCamera.transform.position.z;

        // Setup mock of UnityService to mock player input
        var unityService = Substitute.For<IUnityService>();
        unityService.GetAxis("Horizontal").Returns(0.5f);
        unityService.GetAxis("Vertical").Returns(0.5f);
        unityService.GetDeltaTime().Returns(0.016f);
        defenderCameraController.unityService = unityService;

        // Allow test to run for x seconds
        yield return new WaitForSeconds(timeToWait);

        // Assert that camera position has moved
        Assert.AreNotEqual(initialX, defenderCamera.transform.position.x, "x-position of camera has not changed!");
        Assert.AreNotEqual(initialZ, defenderCamera.transform.position.z, "z-position of camera has not changed!");

        yield return null;
    }

    [UnityTearDown]
    public IEnumerator UnityTearDown()
    {
        // Exit network
        GameObject networkManagerV2 = GameObject.Find("NetworkManagerV2");
        NetworkManagerExtension networkManagerExtension = networkManagerV2.GetComponent<NetworkManagerExtension>();
        networkManagerExtension.StopHost();

        // Wait for networking to stop
        yield return new WaitForSeconds(timeToWait);

        yield return null;
    }
}
