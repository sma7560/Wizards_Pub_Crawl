//using UnityEngine;
//using UnityEngine.TestTools;
//using NSubstitute;
//using NUnit.Framework;
//using System.Collections;
//using UnityEngine.SceneManagement;

///// <summary>
///// This class contains system tests for Attacker Input on the Test scene.
///// These tests run on Play Mode in Unity's Test Runner.
///// </summary>
//public class AttackerInputPlayTest
//{
//    readonly int timeToWait = 2;    // number of seconds to wait for after test scene is loaded

//    [UnitySetUp]
//    public IEnumerator SetUp()
//    {
//        // Load Test scene
//        SceneManager.LoadSceneAsync("TestScene", LoadSceneMode.Single);

//        // Wait for test scene to be loaded
//        yield return new WaitForSeconds(timeToWait);
//    }

//    /// <summary>
//    /// Test ST-AI1: Checks functionality of character movement along the x-axis (right).
//    /// Requirement: FR-4
//    /// </summary>
//    [UnityTest]
//    public IEnumerator AttackerInput_MovesAlongXAxis_Right()
//    {
//        // Setup Hero GameObject
//        GameObject heroes = GameObject.Find("Heroes");
//        GameObject hero = heroes.transform.Find("Hero1").gameObject;
//        Assert.IsNotNull(hero, "Hero object is null!");
//        hero.SetActive(true);

//        // Get HeroController
//        HeroController heroController = hero.GetComponent<HeroController>();
//        Assert.IsNotNull(heroController, "HeroController is null!");

//        // Get initial x-position
//        float initialX = hero.transform.position.x;

//        // Substitute UnityService to mock player movement input
//        var unityService = Substitute.For<IUnityService>();
//        unityService.GetAxisRaw("Horizontal").Returns(1);   // move player on x-axis
//        unityService.GetAxisRaw("Vertical").Returns(0);
//        heroController.unityService = unityService;

//        // Allow test to run for x amount of seconds
//        yield return new WaitForSeconds(timeToWait);

//        // Assert that hero has moved to the right as expected
//        Assert.Greater(hero.transform.position.x, initialX, "Hero did not move to the right!");

//        yield return null;
//    }

//    /// <summary>
//    /// Test ST-AI2: Checks functionality of character movement along the x-axis (left).
//    /// Requirement: FR-4
//    /// </summary>
//    [UnityTest]
//    public IEnumerator AttackerInput_MovesAlongXAxis_Left()
//    {
//        // Setup Hero GameObject
//        GameObject heroes = GameObject.Find("Heroes");
//        GameObject hero = heroes.transform.Find("Hero1").gameObject;
//        Assert.IsNotNull(hero, "Hero object is null!");
//        hero.SetActive(true);

//        // Get HeroController
//        HeroController heroController = hero.GetComponent<HeroController>();
//        Assert.IsNotNull(heroController, "HeroController is null!");

//        // Get initial x-position
//        float initialX = hero.transform.position.x;

//        // Substitute UnityService to mock player movement input
//        var unityService = Substitute.For<IUnityService>();
//        unityService.GetAxisRaw("Horizontal").Returns(-1);   // move player on x-axis
//        unityService.GetAxisRaw("Vertical").Returns(0);
//        heroController.unityService = unityService;

//        // Allow test to run for x amount of seconds
//        yield return new WaitForSeconds(timeToWait);

//        // Assert that hero has moved to the left as expected
//        Assert.Less(hero.transform.position.x, initialX, "Hero did not move to the left!");

//        yield return null;
//    }

//    /// <summary>
//    /// Test ST-AI3: Checks functionality of character movement along the z-axis (up).
//    /// Requirement: FR-4
//    /// </summary>
//    [UnityTest]
//    public IEnumerator AttackerInput_MovesAlongZAxis_Up()
//    {
//        // Setup Hero GameObject
//        GameObject heroes = GameObject.Find("Heroes");
//        GameObject hero = heroes.transform.Find("Hero1").gameObject;
//        Assert.IsNotNull(hero, "Hero object is null!");
//        hero.SetActive(true);

//        // Get HeroController
//        HeroController heroController = hero.GetComponent<HeroController>();
//        Assert.IsNotNull(heroController, "HeroController is null!");

//        // Get initial x-position
//        float initialZ = hero.transform.position.z;

//        // Substitute UnityService to mock player movement input
//        var unityService = Substitute.For<IUnityService>();
//        unityService.GetAxisRaw("Horizontal").Returns(0);
//        unityService.GetAxisRaw("Vertical").Returns(1);     // move player along z-axis
//        heroController.unityService = unityService;

//        // Allow test to run for x amount of seconds
//        yield return new WaitForSeconds(timeToWait);

//        // Assert that hero has moved up as expected
//        Assert.Greater(hero.transform.position.z, initialZ, "Hero did not move up!");

//        yield return null;
//    }

//    /// <summary>
//    /// Test ST-AI4: Checks functionality of character movement along the z-axis (down).
//    /// Requirement: FR-4
//    /// </summary>
//    [UnityTest]
//    public IEnumerator AttackerInput_MovesAlongZAxis_Down()
//    {
//        // Setup Hero GameObject
//        GameObject heroes = GameObject.Find("Heroes");
//        GameObject hero = heroes.transform.Find("Hero1").gameObject;
//        Assert.IsNotNull(hero, "Hero object is null!");
//        hero.SetActive(true);

//        // Get HeroController
//        HeroController heroController = hero.GetComponent<HeroController>();
//        Assert.IsNotNull(heroController, "HeroController is null!");

//        // Get initial x-position
//        float initialZ = hero.transform.position.z;

//        // Substitute UnityService to mock player movement input
//        var unityService = Substitute.For<IUnityService>();
//        unityService.GetAxisRaw("Horizontal").Returns(0);
//        unityService.GetAxisRaw("Vertical").Returns(-1);     // move player along z-axis
//        heroController.unityService = unityService;

//        // Allow test to run for x amount of seconds
//        yield return new WaitForSeconds(timeToWait);

//        // Assert that hero has moved down as expected
//        Assert.Less(hero.transform.position.z, initialZ, "Hero did not move up!");

//        yield return null;
//    }

//    /// <summary>
//    /// Test ST-AI5: Checks functionality of hero basic attack, and that a basic attack is performed when space is pressed.
//    /// Requirement: FR-18
//    /// </summary>
//    [UnityTest]
//    public IEnumerator AttackerInput_PerformBasicAttack()
//    {
//        // TODO: TEMPORARY, REMOVE LATER. ONLY ADDED BECAUSE NETWORK ATTACKING IS CURRENTLY GIVING LOG ERRORS.
//        LogAssert.ignoreFailingMessages = true;     // REMOVE THIS LINE LATER
        
//        // Setup Hero GameObject
//        GameObject heroes = GameObject.Find("Heroes");
//        GameObject hero = heroes.transform.Find("Hero1").gameObject;
//        Assert.IsNotNull(hero, "Hero object is null!");
//        hero.SetActive(true);

//        // Get HeroController
//        HeroController heroController = hero.GetComponent<HeroController>();
//        Assert.IsNotNull(heroController, "HeroController is null!");

//        // Substitute CharacterCombat in HeroController
//        var heroCombat = Substitute.For<CharacterCombat>();
//        //heroController.heroCombat = heroCombat;

//        // Substitute UnityService to mock player input
//        var unityService = Substitute.For<IUnityService>();
//        unityService.GetKeyDown(KeyCode.Space).Returns(true);   // mock player pressing space
//        heroController.unityService = unityService;

//        // Allow test to run for x amount of seconds
//        yield return new WaitForSeconds(timeToWait);
//        unityService.GetKeyDown(KeyCode.Space).Returns(false);  // disable space mock input

//        // Assert that heroCombat has received a call to CmdAttack()
//        heroCombat.Received().CmdAttack();

//        yield return null;
//    }

//    /// <summary>
//    /// Test ST-AI6: Checks functionality of hero skill casting for key 1, and when 1 is pressed, equipped skill is performed.
//    /// Requirement: FR-19
//    /// </summary>
//    [UnityTest]
//    public IEnumerator AttackerInput_PerformSkill_1()
//    {
//        // TODO: update test once logic is implemented
        
//        // Setup Hero GameObject
//        GameObject heroes = GameObject.Find("Heroes");
//        GameObject hero = heroes.transform.Find("Hero1").gameObject;
//        Assert.IsNotNull(hero, "Hero object is null!");
//        hero.SetActive(true);

//        // Get HeroController
//        HeroController heroController = hero.GetComponent<HeroController>();
//        Assert.IsNotNull(heroController, "HeroController is null!");

//        // Substitute UnityService to mock player input
//        var unityService = Substitute.For<IUnityService>();
//        unityService.GetKeyDown(KeyCode.Alpha1).Returns(true);  // mock player pressing 1
//        heroController.unityService = unityService;

//        // Allow test to run for x amount of seconds
//        //yield return new WaitForSeconds(timeToWait);

//        // Assert that heroCombat has performed skill equipped to the 1 key
//        Assert.Fail("PLACEHOLDER: Need to update this test once skills are implemented!");

//        yield return null;
//    }

//    /// <summary>
//    /// Test ST-AI7: Checks functionality of hero skill casting for key 2, and when 2 is pressed, equipped skill is performed.
//    /// Requirement: FR-19
//    /// </summary>
//    [UnityTest]
//    public IEnumerator AttackerInput_PerformSkill_2()
//    {
//        // TODO: update test once logic is implemented
        
//        // Setup Hero GameObject
//        GameObject heroes = GameObject.Find("Heroes");
//        GameObject hero = heroes.transform.Find("Hero1").gameObject;
//        Assert.IsNotNull(hero, "Hero object is null!");
//        hero.SetActive(true);

//        // Get HeroController
//        HeroController heroController = hero.GetComponent<HeroController>();
//        Assert.IsNotNull(heroController, "HeroController is null!");

//        // Substitute UnityService to mock player input
//        var unityService = Substitute.For<IUnityService>();
//        unityService.GetKeyDown(KeyCode.Alpha2).Returns(true);  // mock player pressing 2
//        heroController.unityService = unityService;

//        // Allow test to run for x amount of seconds
//        //yield return new WaitForSeconds(timeToWait);

//        // Assert that heroCombat has performed skill equipped to the 2 key
//        Assert.Fail("PLACEHOLDER: Need to update this test once skills are implemented!");

//        yield return null;
//    }

//    /// <summary>
//    /// Test ST-AI8: Checks functionality of hero skill casting for key 3, and when 3 is pressed, equipped skill is performed.
//    /// Requirement: FR-19
//    /// </summary>
//    [UnityTest]
//    public IEnumerator AttackerInput_PerformSkill_3()
//    {
//        // TODO: update test once logic is implemented
        
//        // Setup Hero GameObject
//        GameObject heroes = GameObject.Find("Heroes");
//        GameObject hero = heroes.transform.Find("Hero1").gameObject;
//        Assert.IsNotNull(hero, "Hero object is null!");
//        hero.SetActive(true);

//        // Get HeroController
//        HeroController heroController = hero.GetComponent<HeroController>();
//        Assert.IsNotNull(heroController, "HeroController is null!");

//        // Substitute UnityService to mock player input
//        var unityService = Substitute.For<IUnityService>();
//        unityService.GetKeyDown(KeyCode.Alpha3).Returns(true);  // mock player pressing 3
//        heroController.unityService = unityService;

//        // Allow test to run for x amount of seconds
//        //yield return new WaitForSeconds(timeToWait);

//        // Assert that heroCombat has performed skill equipped to the 3 key
//        Assert.Fail("PLACEHOLDER: Need to update this test once skills are implemented!");

//        yield return null;
//    }

//    /// <summary>
//    /// Test ST-AI9: Checks functionality of hero skill casting for key 4, and when 4 is pressed, equipped skill is performed.
//    /// Requirement: FR-19
//    /// </summary>
//    [UnityTest]
//    public IEnumerator AttackerInput_PerformSkill_4()
//    {
//        // TODO: update test once logic is implemented
        
//        // Setup Hero GameObject
//        GameObject heroes = GameObject.Find("Heroes");
//        GameObject hero = heroes.transform.Find("Hero1").gameObject;
//        Assert.IsNotNull(hero, "Hero object is null!");
//        hero.SetActive(true);

//        // Get HeroController
//        HeroController heroController = hero.GetComponent<HeroController>();
//        Assert.IsNotNull(heroController, "HeroController is null!");

//        // Substitute UnityService to mock player input
//        var unityService = Substitute.For<IUnityService>();
//        unityService.GetKeyDown(KeyCode.Alpha4).Returns(true);  // mock player pressing 4
//        heroController.unityService = unityService;

//        // Allow test to run for x amount of seconds
//        //yield return new WaitForSeconds(timeToWait);

//        // Assert that heroCombat has performed skill equipped to the 4 key
//        Assert.Fail("PLACEHOLDER: Need to update this test once skills are implemented!");

//        yield return null;
//    }

//    /// <summary>
//    /// Test ST-AI10: Checks functionality of hero item usage for key Q, and that when Q is pressed, equipped item is used.
//    /// Requirement: FR-21, FR-56
//    /// </summary>
//    [UnityTest]
//    public IEnumerator AttackerInput_UseItem_Q()
//    {
//        // TODO: update test once logic is implemented
        
//        // Setup Hero GameObject
//        GameObject heroes = GameObject.Find("Heroes");
//        GameObject hero = heroes.transform.Find("Hero1").gameObject;
//        Assert.IsNotNull(hero, "Hero object is null!");
//        hero.SetActive(true);

//        // Get HeroController
//        HeroController heroController = hero.GetComponent<HeroController>();
//        Assert.IsNotNull(heroController, "HeroController is null!");

//        // Substitute UnityService to mock player input
//        var unityService = Substitute.For<IUnityService>();
//        unityService.GetKeyDown(KeyCode.Q).Returns(true);   // mock player pressing Q
//        heroController.unityService = unityService;

//        // Allow test to run for x amount of seconds
//        //yield return new WaitForSeconds(timeToWait);

//        // Assert that item equipped on key Q has been used
//        Assert.Fail("PLACEHOLDER: Need to update this test once items are implemented!");

//        yield return null;
//    }

//    /// <summary>
//    /// Test ST-AI11: Checks functionality of hero item usage for key 2, and that when 2 is pressed, equipped item is used.
//    /// Requirement: FR-21, FR-56
//    /// </summary>
//    [UnityTest]
//    public IEnumerator AttackerInput_UseItem_E()
//    {
//        // TODO: update test once logic is implemented
        
//        // Setup Hero GameObject
//        GameObject heroes = GameObject.Find("Heroes");
//        GameObject hero = heroes.transform.Find("Hero1").gameObject;
//        Assert.IsNotNull(hero, "Hero object is null!");
//        hero.SetActive(true);

//        // Get HeroController
//        HeroController heroController = hero.GetComponent<HeroController>();
//        Assert.IsNotNull(heroController, "HeroController is null!");

//        // Substitute UnityService to mock player input
//        var unityService = Substitute.For<IUnityService>();
//        unityService.GetKeyDown(KeyCode.Alpha2).Returns(true);  // mock player pressing 2
//        heroController.unityService = unityService;

//        // Allow test to run for x amount of seconds
//        //yield return new WaitForSeconds(timeToWait);

//        // Assert that item equipped on key E has been used
//        Assert.Fail("PLACEHOLDER: Need to update this test once items are implemented!");

//        yield return null;
//    }

//    /// <summary>
//    /// Test ST-AI12: Checks functionality of hero item usage for key R, and that when R is pressed, equipped item is used.
//    /// Requirement: FR-21, FR-56
//    /// </summary>
//    [UnityTest]
//    public IEnumerator AttackerInput_UseItem_R()
//    {
//        // TODO: update test once logic is implemented
        
//        // Setup Hero GameObject
//        GameObject heroes = GameObject.Find("Heroes");
//        GameObject hero = heroes.transform.Find("Hero1").gameObject;
//        Assert.IsNotNull(hero, "Hero object is null!");
//        hero.SetActive(true);

//        // Get HeroController
//        HeroController heroController = hero.GetComponent<HeroController>();
//        Assert.IsNotNull(heroController, "HeroController is null!");

//        // Substitute UnityService to mock player input
//        var unityService = Substitute.For<IUnityService>();
//        unityService.GetKeyDown(KeyCode.R).Returns(true);   // mock player pressing R
//        heroController.unityService = unityService;

//        // Allow test to run for x amount of seconds
//        //yield return new WaitForSeconds(timeToWait);

//        // Assert that item equipped on key R has been used
//        Assert.Fail("PLACEHOLDER: Need to update this test once items are implemented!");

//        yield return null;
//    }

//    /// <summary>
//    /// Test ST-AI13: Checks that character movement is not possible when hero is knocked out.
//    /// Requirement: FR-23
//    /// </summary>
//    [UnityTest]
//    public IEnumerator AttackerInput_KnockedOut_NoCharacterMovement()
//    {
//        // Setup Hero GameObject
//        GameObject heroes = GameObject.Find("Heroes");
//        GameObject hero = heroes.transform.Find("Hero1").gameObject;
//        hero.SetActive(true);

//        // Allow hero initialization
//        yield return new WaitForSeconds(timeToWait);

//        // Get HeroController & HeroStats
//        HeroController heroController = hero.GetComponent<HeroController>();
//        CharacterStats heroStats = hero.GetComponent<CharacterStats>();

//        // Set Hero to knocked out status
//        Assert.IsNotNull(heroStats);
//        heroStats.SetCurrentHealth(0);

//        // Allow health to update and hero to knock out
//        yield return new WaitForSeconds(timeToWait);

//        // Get initial x and z-position
//        float initialX = hero.transform.position.x;
//        float initialZ = hero.transform.position.z;

//        // Substitute UnityService to mock player movement input
//        var unityService = Substitute.For<IUnityService>();
//        unityService.GetAxisRaw("Horizontal").Returns(1);   // move player on x-axis
//        unityService.GetAxisRaw("Vertical").Returns(1);     // move player on y-axis
//        heroController.unityService = unityService;

//        // Allow test to run for x amount of seconds
//        yield return new WaitForSeconds(timeToWait);

//        // Assert that hero has not moved
//        Assert.AreEqual(hero.transform.position.x, initialX, 0.1f, "Hero moved along the x-axis while knocked out!");
//        Assert.AreEqual(hero.transform.position.z, initialZ, 0.1f, "Hero moved along the z-axis while knocked out!");

//        yield return null;
//    }

//    /// <summary>
//    /// Test ST-AI14: Checks functionality of hero item interaction with the F key.
//    /// Requirement: FR-5
//    /// </summary>
//    [UnityTest]
//    public IEnumerator AttackerInput_InteractWithItem_F()
//    {
//        // TODO: update test once logic is implemented
        
//        // Setup Hero GameObject
//        GameObject heroes = GameObject.Find("Heroes");
//        GameObject hero = heroes.transform.Find("Hero1").gameObject;
//        Assert.IsNotNull(hero, "Hero object is null!");
//        hero.SetActive(true);

//        // Get HeroController
//        HeroController heroController = hero.GetComponent<HeroController>();
//        Assert.IsNotNull(heroController, "HeroController is null!");

//        // Substitute UnityService to mock player input
//        var unityService = Substitute.For<IUnityService>();
//        unityService.GetKeyDown(KeyCode.F).Returns(true);   // mock player pressing F
//        heroController.unityService = unityService;

//        // Allow test to run for x amount of seconds
//        //yield return new WaitForSeconds(timeToWait);

//        // Assert that item interaction has taken place
//        Assert.Fail("PLACEHOLDER: Need to update this test once items are implemented!");

//        yield return null;
//    }

//    /// <summary>
//    /// Test ST-AI15: Checks functionality of hero door interaction with the F key, and that the door opens/closes as a result.
//    /// Requirement: FR-7
//    /// </summary>
//    [UnityTest]
//    public IEnumerator AttackerInput_InteractWithDoor_F()
//    {
//        // TODO: update test once logic is implemented
        
//        // Setup Hero GameObject
//        GameObject heroes = GameObject.Find("Heroes");
//        GameObject hero = heroes.transform.Find("Hero1").gameObject;
//        hero.SetActive(true);

//        // Get HeroController
//        HeroController heroController = hero.GetComponent<HeroController>();

//        // Substitute UnityService to mock player input
//        var unityService = Substitute.For<IUnityService>();
//        unityService.GetKeyDown(KeyCode.F).Returns(true);   // mock player pressing F
//        heroController.unityService = unityService;

//        // Allow test to run for x amount of seconds
//        //yield return new WaitForSeconds(timeToWait);

//        // Assert that door state has been changed
//        Assert.Fail("PLACEHOLDER: Need to update this test once items are implemented!");

//        yield return null;
//    }

//    /// <summary>
//    /// Test ST-AI16: Checks that the hero status/inventory window opens when the key I is pressed.
//    /// Requirement: FR-8
//    /// </summary>
//    [UnityTest]
//    public IEnumerator AttackerInput_OpenStatusInventoryWindow_I()
//    {
//        // TODO: update test once logic is implemented
        
//        // Setup Hero GameObject
//        GameObject heroes = GameObject.Find("Heroes");
//        GameObject hero = heroes.transform.Find("Hero1").gameObject;
//        hero.SetActive(true);

//        // Get HeroController
//        HeroController heroController = hero.GetComponent<HeroController>();

//        // Substitute UnityService to mock player input
//        var unityService = Substitute.For<IUnityService>();
//        unityService.GetKeyDown(KeyCode.I).Returns(true);   // mock player pressing I
//        heroController.unityService = unityService;

//        // Allow test to run for x amount of seconds
//        //yield return new WaitForSeconds(timeToWait);

//        // Assert that status/inventory window has opened
//        Assert.Fail("PLACEHOLDER: Need to update this test once items are implemented!");

//        yield return null;
//    }

//    /// <summary>
//    /// Test ST-AI17: Checks that the hero skills window opens when the key K is pressed.
//    /// Requirement: FR-13
//    /// </summary>
//    [UnityTest]
//    public IEnumerator AttackerInput_OpenSkillsWindow_K()
//    {
//        // TODO: update test once logic is implemented
        
//        // Setup Hero GameObject
//        GameObject heroes = GameObject.Find("Heroes");
//        GameObject hero = heroes.transform.Find("Hero1").gameObject;
//        hero.SetActive(true);

//        // Get HeroController
//        HeroController heroController = hero.GetComponent<HeroController>();

//        // Substitute UnityService to mock player input
//        var unityService = Substitute.For<IUnityService>();
//        unityService.GetKeyDown(KeyCode.K).Returns(true);   // mock player pressing K
//        heroController.unityService = unityService;

//        // Allow test to run for x amount of seconds
//        //yield return new WaitForSeconds(timeToWait);

//        // Assert that status/inventory window has opened
//        Assert.Fail("PLACEHOLDER: Need to update this test once items are implemented!");

//        yield return null;
//    }

//    /// <summary>
//    /// Test ST-AI18: Checks functionality of teammate revival with F key, and that the knocked out teammate is revived.
//    /// Requirement: FR-25
//    /// </summary>
//    [UnityTest]
//    public IEnumerator AttackerInput_ReviveKnockedOutTeammate_F()
//    {
//        // TODO: update test once logic is implemented
        
//        // Setup Hero GameObject
//        GameObject heroes = GameObject.Find("Heroes");
//        GameObject hero = heroes.transform.Find("Hero1").gameObject;
//        hero.SetActive(true);

//        // Get HeroController
//        HeroController heroController = hero.GetComponent<HeroController>();

//        // Substitute UnityService to mock player input
//        var unityService = Substitute.For<IUnityService>();
//        unityService.GetKeyDown(KeyCode.F).Returns(true);   // mock player pressing F
//        heroController.unityService = unityService;

//        // Allow test to run for x amount of seconds
//        //yield return new WaitForSeconds(timeToWait);

//        // Assert that teammate has revived has been changed
//        Assert.Fail("PLACEHOLDER: Need to update this test once items are implemented!");

//        yield return null;
//    }

//    /// <summary>
//    /// Test ST-AI19: Checks that all interactions are not possible when hero is knocked out.
//    /// Requirement: FR-23
//    /// </summary>
//    [UnityTest]
//    public IEnumerator AttackerInput_KnockedOut_NoInteractions_F()
//    {
//        // Setup Hero GameObject
//        GameObject heroes = GameObject.Find("Heroes");
//        GameObject hero = heroes.transform.Find("Hero1").gameObject;
//        hero.SetActive(true);

//        // Allow hero initialization
//        yield return new WaitForSeconds(timeToWait);

//        // Get Hero componennts
//        HeroController heroController = hero.GetComponent<HeroController>();
//        CharacterStats heroStats = hero.GetComponent<CharacterStats>();

//        // Set Hero to knocked out status
//        Assert.IsNotNull(heroStats);
//        heroStats.SetCurrentHealth(0);

//        // Allow health to update and hero to knock out
//        yield return new WaitForSeconds(timeToWait);

//        // Substitute UnityService to mock player movement input
//        var unityService = Substitute.For<IUnityService>();
//        unityService.GetKeyDown(KeyCode.F).Returns(true);   // mock player input of key F
//        heroController.unityService = unityService;

//        // Allow test to run for x amount of seconds
//        yield return new WaitForSeconds(timeToWait);

//        // Assert that no interactions have taken place
//        // (ie. item interaction, teammate revival, opening/closing door)
//        Assert.Fail("PLACEHOLDER: Need to update this test once interactions are implemented!");

//        yield return null;
//    }

//    /// <summary>
//    /// Test ST-AI20: Checks that the player is able to unlock new skills if they possess the necessary resources to do so.
//    /// Requirement: FR-14, FR-16
//    /// </summary>
//    [UnityTest]
//    public IEnumerator AttackerInput_UnlockNewSkill()
//    {
//        // TODO: update test once logic is implemented
        
//        // Setup Hero GameObject
//        GameObject heroes = GameObject.Find("Heroes");
//        GameObject hero = heroes.transform.Find("Hero1").gameObject;
//        hero.SetActive(true);
//        HeroController heroController = hero.GetComponent<HeroController>();

//        // Open skills window
//        var unityService = Substitute.For<IUnityService>();
//        unityService.GetKeyDown(KeyCode.K).Returns(true);
//        heroController.unityService = unityService;

//        // Assert that new skill is unlocked
//        Assert.Fail("PLACEHOLDER: Need to update this test once items are implemented!");

//        yield return null;
//    }

//    /// <summary>
//    /// Test ST-AI21: Checks that the player is unable to unlock new skills upon attempting to do so if they do not possess the necessary resources.
//    /// Requirement: FR-16
//    /// </summary>
//    [UnityTest]
//    public IEnumerator AttackerInput_CannotUnlockNewSkill_NoResources()
//    {
//        // TODO: update test once logic is implemented
        
//        // Setup Hero GameObject
//        GameObject heroes = GameObject.Find("Heroes");
//        GameObject hero = heroes.transform.Find("Hero1").gameObject;
//        hero.SetActive(true);
//        HeroController heroController = hero.GetComponent<HeroController>();

//        // Open skills window
//        var unityService = Substitute.For<IUnityService>();
//        unityService.GetKeyDown(KeyCode.K).Returns(true);
//        heroController.unityService = unityService;

//        // Assert that new skill is unlocked
//        Assert.Fail("PLACEHOLDER: Need to update this test once items are implemented!");

//        yield return null;
//    }

//}
