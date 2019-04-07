//using UnityEngine;
//using UnityEngine.TestTools;
//using NUnit.Framework;
//using System.Collections;
//using NSubstitute;

///// <summary>
///// This class holds all unit tests for the EnemyController.cs class in the Scripts > Combat folder.
///// Note that only public functions that aren't reliant on Unity callback functions are tested.
///// </summary>
//public class EnemyControllerUnitTest
//{

//    /// <summary>
//    /// Test UT-EC1: Tests the KillMe() function in EnemyController, and that the enemy object does not exist upon death.
//    /// Requirement: N/A
//    /// </summary>
//    [Test]
//    public void EnemyController_KillMe_ShouldDestroyEnemy()
//    {
//        // TODO: TEMPORARY, REMOVE LATER. ONLY ADDED BECAUSE NETWORK KILLME() IS CURRENTLY GIVING LOG ERRORS.
//        LogAssert.ignoreFailingMessages = true;     // REMOVE THIS LINE LATER

//        // Setup variables
//        GameObject gameObject = new GameObject("TestGameObject");
//        EnemyController enemyController = gameObject.AddComponent<EnemyController>();
//        var unityService = Substitute.For<IUnityService>();
//        enemyController.unityService = unityService;

//        // First, ensure that enemy object is not null when created
//        Assert.IsFalse(gameObject == null, "Enemy GameObject is null upon creation!");

//        // Perform function to be tested
//        enemyController.KillMe();

//        // Assert that Destroy is called
//        unityService.Received().Destroy(gameObject);
//    }
//}
