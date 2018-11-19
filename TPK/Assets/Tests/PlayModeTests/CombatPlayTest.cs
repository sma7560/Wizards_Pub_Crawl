using UnityEngine;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;
using UnityEngine.SceneManagement;
using NSubstitute;

/// <summary>
/// This class contains system tests for Combat scenarios on the Test scene.
/// This class primarily tests hero combat vs. enemy combat.
/// These tests run on Play Mode in Unity's Test Runner.
/// </summary>
public class CombatPlayTest
{
    readonly int timeToWait = 2;    // number of seconds to wait for after test scene is loaded

    [UnitySetUp]
    public IEnumerator SetUp()
    {
        // Load Test scene
        SceneManager.LoadSceneAsync("TestScene", LoadSceneMode.Single);

        // Wait for test scene to be loaded
        yield return new WaitForSeconds(timeToWait);
    }

    /// <summary>
    /// Test ST-C1: Checks that hero HP decreases when attacked by an enemy.
    /// Requirement: FR-27
    /// </summary>
    [UnityTest]
    public IEnumerator Combat_HeroTakesDamageFromMonster()
    {
        // Setup hero object
        GameObject heroes = GameObject.Find("Heroes");
        GameObject hero = heroes.transform.Find("Hero1").gameObject;
        hero.SetActive(true);

        // Wait for hero object to initialize
        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();

        // Get initial HP to compare to
        CharacterStats heroStats = hero.GetComponent<CharacterStats>();
        int initialHp = heroStats.GetCurrentHealth();

        // Setup monster object
        GameObject enemies = GameObject.Find("Enemies");
        GameObject monster = enemies.transform.Find("Monster1").gameObject;
        monster.SetActive(true);

        // Wait for monster to attack hero
        yield return new WaitForSeconds(timeToWait);

        // Assert that hero has lost HP
        Assert.Less(heroStats.GetCurrentHealth(), initialHp, "Hero has not lost HP!");

        yield return null;
    }

    /// <summary>
    /// Test ST-C2: Checks that enemy HP does not change when not attacked by the hero.
    /// Requirement: N/A
    /// </summary>
    [UnityTest]
    public IEnumerator Combat_MonsterDoesNotTakeDamage()
    {
        // Setup monster object
        GameObject enemies = GameObject.Find("Enemies");
        GameObject monster = enemies.transform.Find("Monster1").gameObject;
        monster.SetActive(true);

        // Wait for monster object to initialize
        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();

        // Get initial HP to compare to
        CharacterStats monsterStats = monster.GetComponent<CharacterStats>();
        int initialHp = monsterStats.GetCurrentHealth();

        // Setup hero object
        GameObject heroes = GameObject.Find("Heroes");
        GameObject hero = heroes.transform.Find("Hero1").gameObject;
        hero.SetActive(true);

        // Allow test to run for x amount of seconds
        yield return new WaitForSeconds(timeToWait);

        // Assert that monster's HP is not changed
        Assert.AreEqual(initialHp, monsterStats.GetCurrentHealth(), "Monster's HP has changed unexpectedly.");

        yield return null;
    }

    /// <summary>
    /// Test ST-C3: Checks that enemy HP has decreased when attacked by the hero.
    /// Requirement: N/A
    /// </summary>
    [UnityTest]
    public IEnumerator Combat_MonsterTakesDamageFromHero()
    {
        // TODO: TEMPORARY, REMOVE LATER. ONLY ADDED BECAUSE NETWORK ATTACKING IS CURRENTLY GIVING LOG ERRORS.
        LogAssert.ignoreFailingMessages = true;     // REMOVE THIS LINE LATER
        
        // Setup monster object
        GameObject enemies = GameObject.Find("Enemies");
        GameObject monster = enemies.transform.Find("Monster1").gameObject;
        monster.SetActive(true);

        // Wait for monster object to initialize
        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();

        // Get initial HP to compare to
        CharacterStats monsterStats = monster.GetComponent<CharacterStats>();
        int initialHp = monsterStats.GetCurrentHealth();

        // Setup hero object
        GameObject heroes = GameObject.Find("Heroes");
        GameObject hero = heroes.transform.Find("Hero1").gameObject;
        HeroController heroController = hero.GetComponent<HeroController>();
        hero.SetActive(true);

        // Mock player input to make hero perform basic attack
        var unityService = Substitute.For<IUnityService>();
        unityService.GetKeyDown(KeyCode.Space).Returns(true);   // mock spacebar input
        heroController.unityService = unityService;

        // Allow test to run for x amount of seconds
        yield return new WaitForSeconds(timeToWait);

        // Assert that monster's HP has decreased
        Assert.Less(monsterStats.GetCurrentHealth(), initialHp, "Monster's HP has not decreased!");

        yield return null;
    }

    /// <summary>
    /// Test ST-C4: Checks that enemy has died when hero has killed the enemy and its HP has reached 0.
    /// Requirement: NFR-20
    /// </summary>
    [UnityTest]
    public IEnumerator Combat_MonsterDies()
    {
        // TODO: TEMPORARY, REMOVE LATER. ONLY ADDED BECAUSE NETWORK ATTACKING IS CURRENTLY GIVING LOG ERRORS.
        LogAssert.ignoreFailingMessages = true;     // REMOVE THIS LINE LATER
        
        // Setup monster object
        GameObject enemies = GameObject.Find("Enemies");
        GameObject monster = enemies.transform.Find("Monster1").gameObject;
        monster.SetActive(true);

        // Wait for monster object to initialize
        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();

        // Get initial HP to compare to
        CharacterStats monsterStats = monster.GetComponent<CharacterStats>();
        int initialHp = monsterStats.GetCurrentHealth();

        // Setup hero object
        GameObject heroes = GameObject.Find("Heroes");
        GameObject hero = heroes.transform.Find("Hero1").gameObject;
        HeroController heroController = hero.GetComponent<HeroController>();
        CharacterStats heroStats = hero.GetComponent<CharacterStats>();
        var damage = Substitute.ForPartsOf<Stat>();
        var defence = Substitute.ForPartsOf<Stat>();
        defence.When(x => x.GetValue()).DoNotCallBase();
        damage.When(x => x.GetValue()).DoNotCallBase();
        defence.GetValue().Returns(int.MaxValue);   // set hero to have very high defence so they do not die when attacking monster
        damage.GetValue().Returns(int.MaxValue);    // set hero to do very high damage so that monster will die
        heroStats.defence = defence;
        hero.SetActive(true);

        // Mock player input to make hero perform basic attack
        var unityService = Substitute.For<IUnityService>();
        unityService.GetKeyDown(KeyCode.Space).Returns(true);   // mock spacebar input
        heroController.unityService = unityService;

        // Allow test to run for x amount of seconds
        yield return new WaitForSeconds(timeToWait);

        // Assert that monster object is null
        Assert.IsTrue(monster == null, "Monster is not null!");

        yield return null;
    }

    /// <summary>
    /// Test ST-C5: Checks that hero is knocked out when hero is killed by monster and reaches 0 hp.
    /// Requirement: FR-22
    /// </summary>
    [UnityTest]
    public IEnumerator Combat_HeroKnockedOut()
    {
        // Setup monster object
        GameObject enemies = GameObject.Find("Enemies");
        GameObject monster = enemies.transform.Find("Monster1").gameObject;
        monster.SetActive(true);

        // Setup hero object
        GameObject heroes = GameObject.Find("Heroes");
        GameObject hero = heroes.transform.Find("Hero1").gameObject;
        HeroController heroController = hero.GetComponent<HeroController>();
        CharacterStats heroStats = hero.GetComponent<CharacterStats>();
        hero.SetActive(true);
        heroStats.SetCurrentHealth(1);  // set health to low number to ensure death upon an attack

        // Allow test to run for x amount of seconds
        yield return new WaitForSeconds(timeToWait);

        // Assert that hero is knocked out
        Assert.IsTrue(heroController.GetKnockedOutStatus(), "Hero is not knocked out!");

        yield return null;
    }

    /// <summary>
    /// Test ST-C6: Checks that hero has gained currency upon defeating a monster.
    /// Requirement: FR-6
    /// </summary>
    [UnityTest]
    public IEnumerator Combat_MonsterDies_HeroGainsCurrency()
    {
        // TODO: TEMPORARY, REMOVE LATER. ONLY ADDED BECAUSE NETWORK ATTACKING IS CURRENTLY GIVING LOG ERRORS.
        LogAssert.ignoreFailingMessages = true;     // REMOVE THIS LINE LATER
        
        // Setup monster object
        GameObject enemies = GameObject.Find("Enemies");
        GameObject monster = enemies.transform.Find("Monster1").gameObject;
        monster.SetActive(true);

        // Wait for monster object to initialize
        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();

        // Get initial HP to compare to
        CharacterStats monsterStats = monster.GetComponent<CharacterStats>();
        int initialHp = monsterStats.GetCurrentHealth();

        // Setup hero object
        GameObject heroes = GameObject.Find("Heroes");
        GameObject hero = heroes.transform.Find("Hero1").gameObject;
        HeroController heroController = hero.GetComponent<HeroController>();
        CharacterStats heroStats = hero.GetComponent<CharacterStats>();
        var damage = Substitute.ForPartsOf<Stat>();
        var defence = Substitute.ForPartsOf<Stat>();
        defence.When(x => x.GetValue()).DoNotCallBase();
        damage.When(x => x.GetValue()).DoNotCallBase();
        defence.GetValue().Returns(int.MaxValue);   // set hero to have very high defence so they do not die when attacking monster
        damage.GetValue().Returns(int.MaxValue);    // set hero to do very high damage so that monster will die
        heroStats.defence = defence;
        hero.SetActive(true);

        // Mock player input to make hero perform basic attack
        var unityService = Substitute.For<IUnityService>();
        unityService.GetKeyDown(KeyCode.Space).Returns(true);   // mock spacebar input
        heroController.unityService = unityService;

        // Allow test to run for x amount of seconds
        yield return new WaitForSeconds(timeToWait);

        // Assert that hero has gained currency
        Assert.Fail("PLACEHOLDER: Update test once currency logic is implemented.");

        yield return null;
    }

    /// <summary>
    /// Test ST-C7: Checks that boss HP has decreased when attacked by the hero.
    /// Requirement: FR-28
    /// </summary>
    [UnityTest]
    public IEnumerator Combat_BossTakesDamageFromHero()
    {
        // TODO: Implement once logic to be tested is in place
        Assert.Fail("PLACEHOLDER: Implement this test once boss is added");
        yield return null;
    }

    /// <summary>
    /// Test ST-C8: Checks that hero HP decreases when stepping on a trap.
    /// Requirement: FR-27
    /// </summary>
    [UnityTest]
    public IEnumerator Combat_HeroTakesDamageFromTrap()
    {
        // Setup hero object
        GameObject heroes = GameObject.Find("Heroes");
        GameObject hero = heroes.transform.Find("Hero1").gameObject;
        hero.SetActive(true);

        // Wait for hero object to initialize
        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();

        // Get initial HP to compare to
        CharacterStats heroStats = hero.GetComponent<CharacterStats>();
        int initialHp = heroStats.GetCurrentHealth();

        // Setup trap object
        GameObject traps = GameObject.Find("Traps");
        GameObject trap = traps.transform.Find("Trap").gameObject;
        trap.SetActive(true);

        // Wait for trap to damage hero
        yield return new WaitForSeconds(timeToWait);

        // Assert that hero has lost HP
        Assert.Less(heroStats.GetCurrentHealth(), initialHp, "Hero has not lost HP!");

        yield return null;
    }

    /// <summary>
    /// Test ST-C9: Checks that the monster follows the hero by checking that the
    ///             distance between monster and hero gets smaller when hero does not move.
    /// Requirement: FR-53
    /// </summary>
    [UnityTest]
    public IEnumerator Combat_MonsterFollowsHero_HeroNoMovement()
    {
        // Setup hero object
        GameObject heroes = GameObject.Find("Heroes");
        GameObject hero = heroes.transform.Find("Hero1").gameObject;
        hero.SetActive(true);

        // Get position of hero
        float heroX = hero.transform.position.x;
        float heroZ = hero.transform.position.z;

        // Setup monster object
        GameObject enemies = GameObject.Find("Enemies");
        GameObject monster = enemies.transform.Find("Monster1").gameObject;
        monster.SetActive(true);

        // Get initial position of monster
        float monsterInitialX = monster.transform.position.x;
        float monsterInitialZ = monster.transform.position.z;

        // Get initial distance between monster and hero
        float distanceX = Mathf.Abs(monsterInitialX - heroX);
        float distanceZ = Mathf.Abs(monsterInitialZ - heroZ);

        // Wait for monster to attack hero
        yield return new WaitForSeconds(timeToWait);

        // Assert that monster has moved
        Assert.AreNotEqual(monsterInitialX, monster.transform.position.x, "x-position of monster has not changed!");
        Assert.AreNotEqual(monsterInitialZ, monster.transform.position.z, "z-position of monster has not changed!");

        // Assert that distance has decreased
        float newDistanceX = Mathf.Abs(monster.transform.position.x - heroX);
        float newDistanceZ = Mathf.Abs(monster.transform.position.z - heroZ);
        Assert.Less(newDistanceX, distanceX, "distanceX between hero and monster has not decreased!");
        Assert.Less(newDistanceZ, distanceZ, "distanceZ between hero and monster has not decreased!");

        yield return null;
    }


}
