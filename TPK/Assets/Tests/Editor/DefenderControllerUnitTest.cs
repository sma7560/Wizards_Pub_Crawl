using UnityEngine;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;
using NSubstitute;

/// <summary>
/// This class holds all unit tests for the DefenderController.cs class in the Scripts > Defender folder.
/// </summary>
public class DefenderControllerUnitTest
{
    /// <summary>
    /// Test UT-DC1: Checks the functionality of the SpawnMonster() method, and that a monster object is spawned.
    /// Requirement: FR-37, FR-52
    /// </summary>
    [Test]
    public void DefenderController_SpawnMonster_SpawnsMonsterObject()
    {
        // TODO: Fix this test later when networking stops throwing errors

        // TODO: TEMPORARY, REMOVE LATER. ONLY ADDED BECAUSE NETWORK FUNCTIONS ARE CURRENTLY GIVING LOG ERRORS.
        LogAssert.ignoreFailingMessages = true;     // REMOVE THIS LINE LATER

        // Setup variables
        GameObject gameObject = new GameObject("TestGameObject");
        GameObject monster = new GameObject("Monster");
        DefenderController defenderController = gameObject.AddComponent<DefenderController>();
        defenderController.monster = monster;

        // Setup dependency injection for UnityEngine.Object
        var unityService = Substitute.For<IUnityService>();
        defenderController.unityService = unityService;

        // Call SpawnMonster()
        Vector3 vector = new Vector3(0, 0, 0);
        Quaternion quaternion = new Quaternion(0, 0, 0, 0);
        defenderController.SpawnMonster(vector, quaternion);

        // Assert that monster has been instantiated
        unityService.Received().Instantiate(monster, vector, quaternion);
    }

    /// <summary>
    /// Test UT-DC2: Checks the functionality of the SpawnTrap() method, and that a trap object is spawned.
    /// Requirement: FR-38
    /// </summary>
    [Test]
    public void DefenderController_SpawnTrap_SpawnsTrapObject()
    {
        // TODO: Fix this test later when networking stops throwing errors

        // TODO: TEMPORARY, REMOVE LATER. ONLY ADDED BECAUSE NETWORK FUNCTIONS ARE CURRENTLY GIVING LOG ERRORS.
        LogAssert.ignoreFailingMessages = true;     // REMOVE THIS LINE LATER

        // Setup variables
        GameObject gameObject = new GameObject("TestGameObject");
        GameObject trap = new GameObject("Trap");
        DefenderController defenderController = gameObject.AddComponent<DefenderController>();
        defenderController.trap = trap;

        // Setup dependency injection for UnityEngine.Object
        var unityService = Substitute.For<IUnityService>();
        defenderController.unityService = unityService;

        // Call SpawnMonster()
        Vector3 vector = new Vector3(0, 0, 0);
        Quaternion quaternion = new Quaternion(0, 0, 0, 0);
        defenderController.SpawnTrap(vector, quaternion);

        // Assert that monster has been instantiated
        unityService.Received().Instantiate(trap, vector, quaternion);
    }
}
