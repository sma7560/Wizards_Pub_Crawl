using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;

/// <summary>
/// This class contains system tests for the win condition.
/// These tests run on Play Mode in Unity's Test Runner.
/// TODO: Implement tests once win conditions are in place.
/// </summary>
public class WinConditionPlayTest
{
    //readonly int timeToWait = 2;        // number of seconds to wait for after test scene is loaded

    //[UnitySetUp]
    //public IEnumerator SetUp()
    //{
    //    // Load Test scene
    //    SceneManager.LoadSceneAsync("TestScene", LoadSceneMode.Single);

    //    // Wait for test scene to be loaded
    //    yield return new WaitForSeconds(timeToWait);
    //}

    /// <summary>
    /// Test ST-W1: Checks that the game ends with defender's victory when attackers lose all their lives.
    /// Requirement: FR-59
    /// </summary>
    [UnityTest]
    public IEnumerator WinCondition_DefenderWin_AttackersLostAllLives()
    {
        // TODO: Implement once logic to be tested is in place
        Assert.Fail("PLACEHOLDER: Implement this test once attacker lives and win condition are added");
        yield return null;
    }

    /// <summary>
    /// Test ST-W2: Checks that the game ends with defender's victory when attackers do not reach the floor objective within the time limit.
    /// Requirement: FR-60
    /// </summary>
    [UnityTest]
    public IEnumerator WinCondition_DefenderWin_AttackersRunOutOfTime()
    {
        // TODO: Implement once logic to be tested is in place
        Assert.Fail("PLACEHOLDER: Implement this test once time limit and win condition are added");
        yield return null;
    }

    /// <summary>
    /// Test ST-W3: Checks that the game ends with attacker's victory when attackers clear all stages.
    /// Requirement: FR-61
    /// </summary>
    [UnityTest]
    public IEnumerator WinCondition_AttackersWin_AttackersDefeatBoss()
    {
        // TODO: Implement once logic to be tested is in place
        Assert.Fail("PLACEHOLDER: Implement this test once all stages and win condition are added");
        yield return null;
    }
}
