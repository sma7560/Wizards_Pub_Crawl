using UnityEngine;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;
using UnityEngine.SceneManagement;

/// <summary>
/// This class contains system tests for Attacker Prephase on the Test scene (TODO: change to Prephase scene).
/// These tests run on Play Mode in Unity's Test Runner.
/// </summary>
public class AttackerPrephasePlayerTest
{
    readonly int timeToWait = 2;    // number of seconds to wait for after test scene is loaded

    [SetUp]
    public void SetUp()
    {
        // Load Test scene
        // TODO: change to Prephase scene
        SceneManager.LoadSceneAsync("TestScene", LoadSceneMode.Single);
    }

    /// <summary>
    /// Test ST-AP1: Checks that attackers can select their skills from available skills during the pre-phase.
    /// Requirement: FR-1
    /// </summary>
    [UnityTest]
    public IEnumerator AttackerPrephase_SelectSkills()
    {
        // TODO
        Assert.Fail("PLACEHOLDER: Implement test once pre-phase is complete");

        yield return null;
    }

    /// <summary>
    /// Test ST-AP2: Checks that attackers can select their appearance from a list of presets during the pre-phase.
    /// Requirement: FR-2
    /// </summary>
    [UnityTest]
    public IEnumerator AttackerPrephase_SelectHeroAppearance()
    {
        // TODO
        Assert.Fail("PLACEHOLDER: Implement test once pre-phase is complete");

        yield return null;
    }
}
