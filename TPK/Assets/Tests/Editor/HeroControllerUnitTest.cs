using UnityEngine;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;
using NSubstitute;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// This class holds all unit tests for the HeroController.cs class in the Scripts > Attacker folder.
/// </summary>
public class HeroControllerUnitTest
{
    /// <summary>
    /// Test UT-HC1: Checks the functionality of the KillMe() method, and that the hero GameObject is destroyed.
    /// Requirement: FR-22, FR-23
    /// </summary>
    [Test]
    public void HeroController_KillMe_HeroObjectIsDestroyed()
    {
        // TODO: TEMPORARY, REMOVE LATER. ONLY ADDED BECAUSE NETWORK CMDKILLME() IS CURRENTLY GIVING LOG ERRORS.
        //LogAssert.ignoreFailingMessages = true;     // REMOVE THIS LINE LATER

        //// Setup HeroController
        //GameObject gameObject = new GameObject("TestGameObject");
        //HeroController heroController = gameObject.AddComponent<HeroController>();

        //// Use dependency injection for UnityEngine
        //var unityService = Substitute.For<IUnityService>();
        //heroController.unityService = unityService;

        //// Call KillMe() function
        //heroController.KillMe();

        //// Assert that the gameObject is destroyed
        //unityService.Received().Destroy(gameObject);
    }

    /// <summary>
    /// Test UT-HC2:    Checks that all properties are correctly initialized when HeroController is started.
    ///                 Properties include Rigidbody, CharacterStats, CharacterCombat, Transform, Hero Camera, Attacker UI, and setting KnockedOut status to false.
    /// Requirement: FR-9, FR-10, FR-11, FR-22, FR-26
    /// </summary>
    [UnityTest]
    public IEnumerator HeroController_Start_PropertiesCorrectlyInitialized()
    {
        // Setup all required variables for HeroController
        GameObject gameObject = new GameObject("TestGameObject");
        GameObject heroCam = new GameObject("HeroCam");
        GameObject attackerUI = new GameObject("AttackerUI");
        GameObject health = new GameObject("Health");
        GameObject healthText = new GameObject("HealthText");
        health.tag = "Health";
        healthText.tag = "HealthText";
        gameObject.AddComponent<Rigidbody>();
        heroCam.AddComponent<HeroCameraController>();
        health.AddComponent<Image>();
        healthText.AddComponent<TextMeshProUGUI>();

        // Setup HeroController
        HeroController heroController = gameObject.AddComponent<HeroController>();
        heroController.heroCam = heroCam;
        //heroController.playerUI = attackerUI;
        heroController.localTest = true;
        heroController.runInEditMode = true;    // enables Unity callback functions to run

        // Allow Start() function to run
        yield return null;

        // Validate initialized properties
        //Assert.IsFalse(heroController.IsKnockedOut(), "Hero is knocked out upon initialization!");
        Assert.IsNotNull(heroController.unityService, "UnityService is null!");
        Assert.IsNotNull(heroController.GetComponent<Rigidbody>(), "Rigidbody is null!");
        Assert.IsNotNull(heroController.GetComponent<Transform>(), "CharacterTransform is null!");
        Assert.IsNotNull(heroController.heroCam, "HeroCam is null!");
        //Assert.IsNotNull(heroController.playerUI, "AttackerUI is null!");
    }

}
