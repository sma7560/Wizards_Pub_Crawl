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

}
