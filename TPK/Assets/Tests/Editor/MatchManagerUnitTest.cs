using UnityEngine;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;

/// <summary>
/// This class holds the unit test for MatchManager.cs located in the Scripts > Game-Management folder
/// </summary>
public class MatchManagerUnitTest
{
    /// <summary>
    /// Test UT-MM1: Tests the AddDefender() function in MatchManager, and that it returns the expected value.
    /// Requirement: N/A
    /// </summary>
    [Test]
    public void MatchManager_AddDefender_ShouldReturnTrue()
    {
        //// Setup MatchManager
        //GameObject gameObject = new GameObject("TestGameObject");
        //MatchManager matchManager = gameObject.AddComponent<MatchManager>();

        //// Setup defenderExists
        //matchManager.defenderExist = false;

        //// Assert that AddDefender() returns true
        //Assert.IsTrue(matchManager.AddDefender(), "AddDefender() did not return true!");
    }

    /// <summary>
    /// Test UT-MM2: Tests the AddDefender() function in MatchManager, and that it returns the expected value.
    /// Requirement: N/A
    /// </summary>
    [Test]
    public void MatchManager_AddDefender_ShouldReturnFalse()
    {
        //// Setup MatchManager
        //GameObject gameObject = new GameObject("TestGameObject");
        //MatchManager matchManager = gameObject.AddComponent<MatchManager>();

        //// Setup defenderExists
        //matchManager.defenderExist = true;

        //// Assert that AddDefender() returns false
        //Assert.IsFalse(matchManager.AddDefender(), "AddDefender() did not return false!");
    }

    /// <summary>
    /// Test UT-MM3: Tests the AddAttacker() function in MatchManager, and that attackers can be added if there are currently less than 3.
    /// Requirement: FR-3
    /// </summary>
    [Test]
    public void MatchManager_AddAttacker_ShouldReturnTrue()
    {
        //// Setup MatchManager
        //GameObject gameObject = new GameObject("TestGameObject");
        //MatchManager matchManager = gameObject.AddComponent<MatchManager>();

        //// Setup currentAttacker to none
        //matchManager.currentAttacker = 0;

        //// Assert that AddAttacker() returns true for up to 3 attackers
        //Assert.IsTrue(matchManager.AddAttacker(), "AddAttacker() did not return true!");
        //Assert.IsTrue(matchManager.AddAttacker(), "AddAttacker() did not return true!");
        //Assert.IsTrue(matchManager.AddAttacker(), "AddAttacker() did not return true!");
    }

    /// <summary>
    /// Test UT-MM4: Tests the AddAttacker() function in MatchManager, and that additional attackers cannot be added if there are already 3.
    /// Requirement: FR-3
    /// </summary>
    [Test]
    public void MatchManager_AddAttacker_ShouldReturnFalse()
    {
        //// Setup MatchManager
        //GameObject gameObject = new GameObject("TestGameObject");
        //MatchManager matchManager = gameObject.AddComponent<MatchManager>();

        //// Setup currentAttacker to max (3)
        //matchManager.currentAttacker = 3;

        //// Assert that AddAttacker() returns false
        //Assert.IsFalse(matchManager.AddAttacker(), "AddAttacker() did not return false!");
    }
}
