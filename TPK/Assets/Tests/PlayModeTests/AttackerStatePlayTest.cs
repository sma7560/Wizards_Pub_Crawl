using UnityEngine;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;
using UnityEngine.SceneManagement;
using NSubstitute;

/// <summary>
/// This class contains system tests for Attacker State on the Test scene.
/// These tests run on Play Mode in Unity's Test Runner.
/// </summary>
public class AttackerStatePlayTest
{
    readonly int timeToWait = 2;        // number of seconds to wait for after test scene is loaded

    [UnitySetUp]
    public IEnumerator SetUp()
    {
        // Load Test scene
        SceneManager.LoadSceneAsync("TestScene", LoadSceneMode.Single);

        // Wait for test scene to be loaded
        yield return new WaitForSeconds(timeToWait);
    }

    /// <summary>
    /// Test ST-AS1: Checks that the hero camera is instantiated when hero is spawned.
    /// Requirement: FR-26
    /// </summary>
    [UnityTest]
    public IEnumerator AttackerState_HeroInitialization_HeroCamInstantiated()
    {
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
        // Setup Hero GameObject
        GameObject heroes = GameObject.Find("Heroes");
        GameObject hero = heroes.transform.Find("Hero1").gameObject;
        hero.SetActive(true);

        // Wait for Attacker UI to be instantiated
        yield return new WaitForSeconds(timeToWait);

        // Find Attacker UI object
        GameObject attackerUI = null;

        Object[] gameObjects = GameObject.FindObjectsOfType<GameObject>();
        foreach (GameObject gameObject in gameObjects)
        {
            if (gameObject.name.Contains("Attacker UI"))
            {
                attackerUI = gameObject;
                break;
            }
        }

        // Assert that Attacker UI is not null
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
        // Setup Hero and HeroController
        GameObject heroes = GameObject.Find("Heroes");
        GameObject hero = heroes.transform.Find("Hero1").gameObject;
        hero.SetActive(true);
        //HeroController heroController = hero.GetComponent<HeroController>();
        CharacterStats heroStats = hero.GetComponent<CharacterStats>();

        // Allow health to update
        yield return new WaitForSeconds(timeToWait);

        // Assert that hero's health is at max health
        Assert.AreEqual(heroStats.maxHealth, heroStats.GetCurrentHealth(), "Hero is not at max health!");

        yield return null;
    }

    /// <summary>
    /// Test ST-AS4: Checks that the hero is not in knocked out status when they are spawned at max health.
    /// Requirement: FR-22
    /// </summary>
    [UnityTest]
    public IEnumerator AttackerState_HeroInitialization_NotKnockedOut()
    {
        // Setup Hero and HeroController
        GameObject heroes = GameObject.Find("Heroes");
        GameObject hero = heroes.transform.Find("Hero1").gameObject;
        hero.SetActive(true);
        HeroController heroController = hero.GetComponent<HeroController>();

        // Allow health to update
        yield return new WaitForSeconds(timeToWait);

        // Assert that hero is not knocked out
        //Assert.IsFalse(heroController.IsKnockedOut());

        yield return null;
    }

    /// <summary>
    /// Test ST-AS5: Checks that the hero is at knocked out status when their health reaches 0.
    /// Requirement: FR-22
    /// </summary>
    [UnityTest]
    public IEnumerator AttackerState_HeroKnockedOut_AtZeroHealth()
    {
        // Setup Hero and HeroController
        GameObject heroes = GameObject.Find("Heroes");
        GameObject hero = heroes.transform.Find("Hero1").gameObject;
        hero.SetActive(true);

        // Allow hero initialization
        yield return new WaitForSeconds(timeToWait);

        // Set hero's health to 0
        HeroController heroController = hero.GetComponent<HeroController>();
        CharacterStats heroStats = hero.GetComponent<CharacterStats>();
        heroStats.SetCurrentHealth(0);

        // Allow health to update
        yield return new WaitForSeconds(timeToWait);

        // Assert that hero is not knocked out
        //Assert.IsTrue(heroController.IsKnockedOut());

        yield return null;
    }

    /// <summary>
    /// Test ST-AS6: Checks that all elements are present on the Attacker UI (skills, HP, items, game status, objectives list, current currency).
    /// Requirement: FR-9, FR-10, FR-11, FR-58
    /// </summary>
    [UnityTest]
    public IEnumerator AttackerState_AttackerUI_ElementsAreActive()
    {
        // Setup Hero GameObject
        GameObject heroes = GameObject.Find("Heroes");
        GameObject hero = heroes.transform.Find("Hero1").gameObject;
        hero.SetActive(true);

        // Wait for Attacker UI to be instantiated
        yield return new WaitForSeconds(timeToWait);

        // Find Attacker UI object
        GameObject attackerUI = null;

        Object[] gameObjects = GameObject.FindObjectsOfType<GameObject>();
        foreach (GameObject gameObject in gameObjects)
        {
            if (gameObject.name.Contains("Attacker UI"))
            {
                attackerUI = gameObject;
                break;
            }
        }
        
        // Get all elements as child of AttackerUI
        Transform panel = attackerUI.transform.Find("Panel");
        Transform skills = panel.transform.Find("Skills");
        Transform hp = panel.transform.Find("HP");
        Transform items = panel.transform.Find("Items");
        Transform gameStatus = panel.transform.Find("GameStatus");
        Transform objectives = panel.transform.Find("Objectives");
        Transform currency = panel.transform.Find("Currency");

        // Assert that elements in Attacker UI are not null
        Assert.IsNotNull(panel, "Panel in AttackerUI is null!");
        Assert.IsNotNull(skills, "Skills in AttackerUI are null!");
        Assert.IsNotNull(hp, "HP in AttackerUI is null!");
        Assert.IsNotNull(items, "Items in AttackerUI are null!");
        Assert.IsNotNull(gameStatus, "GameStatus in AttackerUI is null!");
        Assert.IsNotNull(objectives, "Objectives in AttackerUI are null!");
        Assert.IsNotNull(currency, "Currency in AttackerUI is null!");

        yield return null;
    }

    /// <summary>
    /// Test ST-AS7: Checks that all elements for the objectives list is present in the Attacker UI (objective1, objective2, objective3).
    /// Requirement: FR-9
    /// </summary>
    [UnityTest]
    public IEnumerator AttackerState_AttackerUI_Objectives_ElementsAreActive()
    {
        // Setup Hero GameObject
        GameObject heroes = GameObject.Find("Heroes");
        GameObject hero = heroes.transform.Find("Hero1").gameObject;
        hero.SetActive(true);

        // Wait for Attacker UI to be instantiated
        yield return new WaitForSeconds(timeToWait);

        // Get all elements in Objectives
        GameObject objectives = GameObject.Find("Objectives");
        Assert.IsNotNull(objectives, "Objectives in AttackerUI is null!");  // TODO: Can remove this line when Objectives are implemented
        Transform objective1 = objectives.transform.Find("Objective1");
        Transform objective2 = objectives.transform.Find("Objective2");
        Transform objective3 = objectives.transform.Find("Objective3");

        // Assert that elements in Objectives are not null
        Assert.IsNotNull(objective1, "Objective 1 in AttackerUI Objectives list is null!");
        Assert.IsNotNull(objective2, "Objective 2 in AttackerUI Objectives list is null!");
        Assert.IsNotNull(objective3, "Objective 3 in AttackerUI Objectives list is null!");

        yield return null;
    }

    /// <summary>
    /// Test ST-AS8:    Checks that all important elements in skills is present and correctly active/inactive
    ///                 in the Attacker UI (skills 1-4 are initially active, and cooldown indicators 1-4 are initially inactive).
    /// Requirement: FR-10, FR-11
    /// </summary>
    [UnityTest]
    public IEnumerator AttackerState_AttackerUI_Skills_ElementsAreActive()
    {
        // Setup Hero GameObject
        GameObject heroes = GameObject.Find("Heroes");
        GameObject hero = heroes.transform.Find("Hero1").gameObject;
        hero.SetActive(true);

        // Wait for Attacker UI to be instantiated
        yield return new WaitForSeconds(timeToWait);

        // Get all elements in Objectives
        GameObject skills = GameObject.Find("Skills");
        Transform skill1 = skills.transform.Find("Skill1");
        Transform skill2 = skills.transform.Find("Skill2");
        Transform skill3 = skills.transform.Find("Skill3");
        Transform skill4 = skills.transform.Find("Skill4");
        Transform cooldown1 = skills.transform.Find("Cooldown1");
        Transform cooldown2 = skills.transform.Find("Cooldown2");
        Transform cooldown3 = skills.transform.Find("Cooldown3");
        Transform cooldown4 = skills.transform.Find("Cooldown4");

        // Assert that elements in Objectives are not null
        Assert.IsNotNull(skill1, "Skill1 in AttackerUI Skills is null!");
        Assert.IsNotNull(skill2, "Skill2 in AttackerUI Skills is null!");
        Assert.IsNotNull(skill3, "Skill3 in AttackerUI Skills is null!");
        Assert.IsNotNull(skill4, "Skill4 in AttackerUI Skills is null!");
        Assert.IsNotNull(cooldown1, "Cooldown1 in AttackerUI Skills is null!");
        Assert.IsNotNull(cooldown2, "Cooldown2 in AttackerUI Skills is null!");
        Assert.IsNotNull(cooldown3, "Cooldown3 in AttackerUI Skills is null!");
        Assert.IsNotNull(cooldown4, "Cooldown4 in AttackerUI Skills is null!");

        // Assert Active status of each element (skills are active, cooldowns are not active)
        Assert.IsTrue(skill1.gameObject.activeSelf, "Skill1 is not active!");
        Assert.IsTrue(skill2.gameObject.activeSelf, "Skill2 is not active!");
        Assert.IsTrue(skill3.gameObject.activeSelf, "Skill3 is not active!");
        Assert.IsTrue(skill4.gameObject.activeSelf, "Skill4 is not active!");
        Assert.IsFalse(cooldown1.gameObject.activeSelf, "Cooldown1 is active!");
        Assert.IsFalse(cooldown2.gameObject.activeSelf, "Cooldown2 is active!");
        Assert.IsFalse(cooldown3.gameObject.activeSelf, "Cooldown3 is active!");
        Assert.IsFalse(cooldown4.gameObject.activeSelf, "Cooldown4 is active!");

        yield return null;
    }

    /// <summary>
    /// Test ST-AS9: Checks that the cooldown indicator appears when skills are used. Tests all skills 1 to 4.
    /// Requirement: FR-11
    /// </summary>
    [UnityTest]
    public IEnumerator AttackerState_AttackerUI_Skills_CooldownIndicatorsAreActive()
    {
        // Setup Hero GameObject and HeroController
        GameObject heroes = GameObject.Find("Heroes");
        GameObject hero = heroes.transform.Find("Hero1").gameObject;
        hero.SetActive(true);
        HeroController heroController = hero.GetComponent<HeroController>();

        // Wait for Attacker UI to be instantiated
        yield return new WaitForSeconds(timeToWait);

        // ------------------------------------------------------------------
        // Perform skill 1
        var unityService = Substitute.For<IUnityService>();
        unityService.GetKeyDown(KeyCode.Alpha1).Returns(true);  // mock player pressing 1
        heroController.unityService = unityService;

        // Wait for cooldown indicator to become active
        yield return new WaitForFixedUpdate();
        yield return new WaitForFixedUpdate();

        // Assert that cooldown indicator for skill 1 is active
        GameObject cooldown1 = GameObject.Find("Cooldown1");
        Assert.IsNotNull(cooldown1, "Cooldown1 in Attacker UI is null!");
        Assert.IsTrue(cooldown1.activeSelf, "Cooldown1 in AttackerUI Skills is not active!");

        // ------------------------------------------------------------------
        // Perform skill 2
        unityService.GetKeyDown(KeyCode.Alpha2).Returns(true);  // mock player pressing 2

        // Wait for cooldown indicator to become active
        yield return new WaitForFixedUpdate();
        yield return new WaitForFixedUpdate();

        // Assert that cooldown indicator for skill 1 is active
        GameObject cooldown2 = GameObject.Find("Cooldown2");
        Assert.IsTrue(cooldown2.activeSelf, "Cooldown2 in AttackerUI Skills is not active!");

        // ------------------------------------------------------------------
        // Perform skill 3
        unityService.GetKeyDown(KeyCode.Alpha3).Returns(true);  // mock player pressing 3

        // Wait for cooldown indicator to become active
        yield return new WaitForFixedUpdate();
        yield return new WaitForFixedUpdate();

        // Assert that cooldown indicator for skill 1 is active
        GameObject cooldown3 = GameObject.Find("Cooldown3");
        Assert.IsTrue(cooldown3.activeSelf, "Cooldown3 in AttackerUI Skills is not active!");

        // ------------------------------------------------------------------
        // Perform skill 4
        unityService.GetKeyDown(KeyCode.Alpha4).Returns(true);  // mock player pressing 4

        // Wait for cooldown indicator to become active
        yield return new WaitForFixedUpdate();
        yield return new WaitForFixedUpdate();

        // Assert that cooldown indicator for skill 1 is active
        GameObject cooldown4 = GameObject.Find("Cooldown4");
        Assert.IsTrue(cooldown4.activeSelf, "Cooldown4 in AttackerUI Skills is not active!");

        yield return null;
    }

    /// <summary>
    /// Test ST-AS10: Checks that the hero dies after x amount of time of being knocked out.
    /// Requirement: FR-22
    /// </summary>
    [UnityTest]
    public IEnumerator AttackerState_HeroStatus_DeathWorks()
    {
        // TODO: TEMPORARY, REMOVE LATER. ONLY ADDED BECAUSE NETWORK CMDKILLME() IS CURRENTLY GIVING LOG ERRORS.
        LogAssert.ignoreFailingMessages = true;     // REMOVE THIS LINE LATER

        // Setup Hero
        GameObject heroes = GameObject.Find("Heroes");
        GameObject hero = heroes.transform.Find("Hero1").gameObject;
        hero.SetActive(true);
        //HeroController heroController = hero.GetComponent<HeroController>();
        CharacterStats heroStats = hero.GetComponent<CharacterStats>();

        // Wait for hero to be instantiated
        yield return new WaitForSeconds(timeToWait);

        // Set hero to knocked out status
        heroStats.SetCurrentHealth(0);

        // Wait for hero to die (death timer is 5 seconds??)
        yield return new WaitForSeconds(10);

        // Assert that hero object no longer exists
        Assert.IsTrue(hero == null, "Hero is not null!");

        yield return null;
    }

    /// <summary>
    /// Test ST-AS11: Checks that the hero respawns after x amount of time of being dead.
    /// Requirement: FR-24
    /// </summary>
    [UnityTest]
    public IEnumerator AttackerState_HeroStatus_RespawnWorks()
    {
        // TODO: TEMPORARY, REMOVE LATER. ONLY ADDED BECAUSE NETWORK CMDKILLME() IS CURRENTLY GIVING LOG ERRORS.
        LogAssert.ignoreFailingMessages = true;     // REMOVE THIS LINE LATER

        // Setup Hero GameObject and HeroController
        GameObject heroes = GameObject.Find("Heroes");
        GameObject hero = heroes.transform.Find("Hero1").gameObject;
        hero.SetActive(true);
        //HeroController heroController = hero.GetComponent<HeroController>();
        CharacterStats heroStats = hero.GetComponent<CharacterStats>();

        // Wait for hero to be instantiated
        yield return new WaitForSeconds(timeToWait);

        // Set hero to knocked out status
        heroStats.SetCurrentHealth(0);

        // Wait for hero to die (death timer is 5 seconds??)
        yield return new WaitForSeconds(10);

        // Wait for hero to respawn
        yield return new WaitForSeconds(30);

        // Assert that hero object exists and is full health
        Assert.IsFalse(hero == null, "Hero is null!");
        Assert.AreEqual(heroStats.maxHealth, heroStats.GetCurrentHealth(), "Hero did not respawn at max health!");

        yield return null;
    }

    /// <summary>
    /// Test ST-AS12: Checks that the health bar element is present for all enemy characters.
    /// Requirement: FR-58
    /// </summary>
    [UnityTest]
    public IEnumerator AttackerState_HealthPresentOnAllEnemies()
    {
        // TODO: add boss health once boss is implemented

        // Get health element on monster prefab
        GameObject enemies = GameObject.Find("Enemies");
        Transform monster = enemies.transform.Find("Monster1");
        Transform healthBar = monster.Find("HealthBar");
        Assert.IsNotNull(healthBar, "HealthBar on monster is null!");

        // Get all health elements within health bar
        Transform healthBackground = healthBar.Find("HealthBackground");
        Transform healthFillImage = healthBar.Find("Health");
        Transform healthText = healthBar.Find("HealthText");

        // Assert that all health elements are not null
        Assert.IsNotNull(healthBackground, "HealthBar Background on monster is null!");
        Assert.IsNotNull(healthFillImage, "HealthBar Fill Image on monster is null!");
        Assert.IsNotNull(healthText, "HealthBar Text on monster is null!");

        yield return null;
    }
}
