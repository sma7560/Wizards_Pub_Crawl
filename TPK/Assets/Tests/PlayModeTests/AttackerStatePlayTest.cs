using UnityEngine;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;
using UnityEngine.SceneManagement;

/// <summary>
/// This class contains system tests for Attacker State on the Test scene.
/// These tests run on Play Mode in Unity's Test Runner.
/// </summary>
public class AttackerStatePlayTest
{
    readonly int timeToWait = 2;    // number of seconds to wait for after test scene is loaded

    [SetUp]
    public void SetUp()
    {
        // Load Test scene
        SceneManager.LoadSceneAsync("TestScene", LoadSceneMode.Single);
    }

    /// <summary>
    /// Test ST-AS1: Checks that the hero camera is instantiated when hero is spawned.
    /// Requirement: FR-26
    /// </summary>
    [UnityTest]
    public IEnumerator AttackerState_HeroInitialization_HeroCamInstantiated()
    {
        // Wait for test scene to be loaded
        yield return new WaitForSeconds(timeToWait);

        // Setup Hero GameObject
        GameObject heroes = GameObject.Find("Heroes");
        GameObject hero = heroes.transform.Find("Hero1").gameObject;
        hero.SetActive(true);

        // Wait for Hero Camera to be instantiated
        yield return new WaitForSeconds(timeToWait);

        // Find Hero Camera object
        Camera heroCamera = null;

        Object[] gameObjects = GameObject.FindObjectsOfType<GameObject>();
        foreach (GameObject gameObject in gameObjects)
        {
            if (gameObject.name.Contains("HeroCamera"))
            {
                heroCamera = gameObject.GetComponent<Camera>();
            }
        }

        // Assert that Hero Camera is not null
        Assert.IsNotNull(heroCamera);

        yield return null;
    }

    /// <summary>
    /// Test ST-AS2: Checks that the attacker UI is instantiated when hero is spawned.
    /// Requirement: FR-9, FR-10, FR-11
    /// </summary>
    [UnityTest]
    public IEnumerator AttackerState_HeroInitialization_AttackerUIInstantiated()
    {
        // Wait for test scene to be loaded
        yield return new WaitForSeconds(timeToWait);

        // Setup Hero GameObject
        GameObject heroes = GameObject.Find("Heroes");
        GameObject hero = heroes.transform.Find("Hero1").gameObject;
        hero.SetActive(true);

        // Wait for Hero Camera to be instantiated
        yield return new WaitForSeconds(timeToWait);

        // Find Hero Camera object
        GameObject attackerUI = null;

        Object[] gameObjects = GameObject.FindObjectsOfType<GameObject>();
        foreach (GameObject gameObject in gameObjects)
        {
            if (gameObject.name.Contains("Attacker UI"))
            {
                attackerUI = gameObject;
            }
        }

        // Assert that Hero Camera is not null
        Assert.IsNotNull(attackerUI);

        yield return null;
    }

    /// <summary>
    /// Test ST-AS3: Checks that the hero starts off with max health upon spawning.
    /// Requirement: FR-20
    /// </summary>
    [UnityTest]
    public IEnumerator AttackerState_HeroInitialization_AtMaxHealth()
    {
        // Wait for test scene to be loaded
        yield return new WaitForSeconds(timeToWait);

        // Setup Hero and HeroController
        GameObject heroes = GameObject.Find("Heroes");
        GameObject hero = heroes.transform.Find("Hero1").gameObject;
        hero.SetActive(true);
        HeroController heroController = hero.GetComponent<HeroController>();

        // Allow health to update
        yield return new WaitForSeconds(timeToWait);

        // Assert that hero's health is at max health
        Assert.AreEqual(heroController.heroStats.maxHealth, heroController.heroStats.GetCurrentHealth(), "Hero is not at max health!");

        yield return null;
    }

    /// <summary>
    /// Test ST-AS4: Checks that the hero is not in knocked out status when they are spawned at max health.
    /// Requirement: FR-22
    /// </summary>
    [UnityTest]
    public IEnumerator AttackerState_HeroInitialization_NotKnockedOut()
    {
        // Wait for test scene to be loaded
        yield return new WaitForSeconds(timeToWait);

        // Setup Hero and HeroController
        GameObject heroes = GameObject.Find("Heroes");
        GameObject hero = heroes.transform.Find("Hero1").gameObject;
        hero.SetActive(true);
        HeroController heroController = hero.GetComponent<HeroController>();

        // Allow health to update
        yield return new WaitForSeconds(timeToWait);

        // Assert that hero is not knocked out
        Assert.IsFalse(heroController.GetKnockedOutStatus());

        yield return null;
    }

    /// <summary>
    /// Test ST-AS5: Checks that the hero is at knocked out status when their health reaches 0.
    /// Requirement: FR-22
    /// </summary>
    [UnityTest]
    public IEnumerator AttackerState_HeroKnockedOut_AtZeroHealth()
    {
        // Wait for test scene to be loaded
        yield return new WaitForSeconds(timeToWait);

        // Setup Hero and HeroController
        GameObject heroes = GameObject.Find("Heroes");
        GameObject hero = heroes.transform.Find("Hero1").gameObject;
        hero.SetActive(true);

        // Allow hero initialization
        yield return new WaitForSeconds(timeToWait);

        // Set hero's health to 0
        HeroController heroController = hero.GetComponent<HeroController>();
        heroController.heroStats.TakeDamage(int.MaxValue);

        // Allow health to update
        yield return new WaitForSeconds(timeToWait);

        // Assert that hero is not knocked out
        Assert.IsTrue(heroController.GetKnockedOutStatus());

        yield return null;
    }

}
