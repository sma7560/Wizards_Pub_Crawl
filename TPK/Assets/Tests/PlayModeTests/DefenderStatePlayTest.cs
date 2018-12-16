using UnityEngine;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// This class contains system tests for Defender State.
/// These tests run on Play Mode in Unity's Test Runner.
/// </summary>
public class DefenderStatePlayTest
{
    readonly int timeToWait = 2;    // number of seconds to wait things to load

    [UnitySetUp]
    public IEnumerator UnitySetUp()
    {
        // Load Test scene
        SceneManager.LoadSceneAsync("TestScene", LoadSceneMode.Single);

        // Wait for test scene to be loaded
        yield return new WaitForSeconds(timeToWait);

        // Setup Defender status
        GameObject networkManagerV2 = GameObject.Find("NetworkManagerV2");
        NetworkManagerExtension networkManagerExtension = networkManagerV2.GetComponent<NetworkManagerExtension>();
        //networkManagerExtension.StartUpHost();

        // Wait for defender status to be initialized
        yield return new WaitForSeconds(timeToWait);
    }

    /// <summary>
    /// Test ST-DS1: Checks that the defender camera is instantiated when defender mode starts.
    /// Requirement: FR-41, FR-46
    /// </summary>
    [UnityTest]
    public IEnumerator DefenderState_Initialization_DefenderCameraInstantiated()
    {
        // Find Defender Camera object
        Camera defenderCamera = null;

        Object[] gameObjects = GameObject.FindObjectsOfType<GameObject>();
        foreach (GameObject gameObject in gameObjects)
        {
            if (gameObject.name.Contains("DefenderCamera"))
            {
                defenderCamera = gameObject.GetComponent<Camera>();
            }
        }

        // Assert that Defender Camera is not null
        Assert.IsNotNull(defenderCamera);

        yield return null;
    }

    /// <summary>
    /// Test ST-DS2: Checks that the Defender UI is instantiated when defender mode starts.
    /// Requirement: FR-43, FR-44
    /// </summary>
    [UnityTest]
    public IEnumerator DefenderState_Initialization_DefenderUIInstantiated()
    {
        // Find Defender UI object
        GameObject defenderUI = null;

        Object[] gameObjects = GameObject.FindObjectsOfType<GameObject>();
        foreach (GameObject gameObject in gameObjects)
        {
            if (gameObject.name.Contains("DefenderUI"))
            {
                defenderUI = gameObject;
            }
        }

        // Assert that Defender UI is not null
        Assert.IsNotNull(defenderUI);

        yield return null;
    }

    /// <summary>
    /// Test ST-DS3: Checks that all elements (currency, energy, cards, minimap) are present in the Defender UI.
    /// Requirement: FR-43, FR-44, FR-57
    /// </summary>
    [UnityTest]
    public IEnumerator DefenderState_DefenderUI_ElementsAreActive()
    {
        // Find Defender UI object
        GameObject defenderUI = null;

        Object[] gameObjects = GameObject.FindObjectsOfType<GameObject>();
        foreach (GameObject gameObject in gameObjects)
        {
            if (gameObject.name.Contains("DefenderUI"))
            {
                defenderUI = gameObject;
            }
        }

        // Find all important elements
        Transform currency = defenderUI.transform.Find("Money");
        Transform energy = defenderUI.transform.Find("Energy");
        Transform cards = defenderUI.transform.Find("Cards Panel");
        Transform minimap = defenderUI.transform.Find("Minimap Panel");

        // Assert that all elements are not null
        Assert.IsNotNull(currency, "Currency is null!");
        Assert.IsNotNull(energy, "Energy is null!");
        Assert.IsNotNull(cards, "Cards are null!");
        Assert.IsNotNull(minimap, "Minimap is null!");

        yield return null;
    }

    /// <summary>
    /// Test ST-DS4: Checks that the starting energy is equal to the max value when first launching defender mode.
    /// Requirement: FR-44
    /// </summary>
    [UnityTest]
    public IEnumerator DefenderState_DefenderUI_Energy_StartingEnergyIsMax()
    {
        // Setup variables
        int maxEnergy = 100;    // TODO: NEED TO CHANGE ONCE ENERGY IS IMPLEMENTED
        GameObject energy = GameObject.Find("Energy");

        // Get current energy value written on Defender UI
        string value = energy.GetComponent<TextMeshProUGUI>().text;

        // Assert that starting currency is equal to the expected
        Assert.AreEqual(maxEnergy, int.Parse(value),
            "Starting energy is not equal to max energy!");

        yield return null;
    }

    /// <summary>
    /// Test ST-DS5: Checks that there are 8 cards present in the Defender UI.
    /// Requirement: FR-31
    /// </summary>
    [UnityTest]
    public IEnumerator DefenderState_DefenderUI_Cards_8CardsPresent()
    {
        // Setup variables
        GameObject cards = GameObject.Find("Cards Panel");

        // Get individual card elements from card panel
        Transform card1 = cards.transform.Find("Card1");
        Transform card2 = cards.transform.Find("Card2");
        Transform card3 = cards.transform.Find("Card3");
        Transform card4 = cards.transform.Find("Card4");
        Transform card5 = cards.transform.Find("Card5");
        Transform card6 = cards.transform.Find("Card6");
        Transform card7 = cards.transform.Find("Card7");
        Transform card8 = cards.transform.Find("Card8");

        // Assert that all 8 cards are not null
        Assert.IsNotNull(card1, "Card1 is null!");
        Assert.IsNotNull(card2, "Card2 is null!");
        Assert.IsNotNull(card3, "Card3 is null!");
        Assert.IsNotNull(card4, "Card4 is null!");
        Assert.IsNotNull(card5, "Card5 is null!");
        Assert.IsNotNull(card6, "Card6 is null!");
        Assert.IsNotNull(card7, "Card7 is null!");
        Assert.IsNotNull(card8, "Card8 is null!");

        yield return null;
    }

    /// <summary>
    /// Test ST-DS6: Checks that health is present on all characters (enemies, heroes).
    /// Requirement: FR-58
    /// </summary>
    [UnityTest]
    public IEnumerator DefenderState_HealthPresentOnAllCharacters()
    {
        // TODO: add test for hero health bars once they are implemented

        // Spawn monsters
        GameObject defender = GameObject.Find("Defender(Clone)");
        DefenderController defenderController = defender.GetComponent<DefenderController>();
        Assert.IsNotNull(defenderController, "DefenderController is null!");
        defenderController.SpawnMonster(new Vector3(0, 0, 0), new Quaternion(0, 0, 0, 0));

        // Allow monster to spawn
        yield return new WaitForSeconds(timeToWait);

        // Get health element on monster prefab
        GameObject monster = null;
        GameObject[] gameObjects = GameObject.FindObjectsOfType<GameObject>();
        foreach (GameObject gameObject in gameObjects)
        {
            if (gameObject.name.Contains("Monster"))
            {
                monster = gameObject;
                break;
            }
        }
        Transform monsterhealthBar = monster.transform.Find("HealthBar");
        Assert.IsNotNull(monsterhealthBar, "HealthBar on monster is null!");

        // Get health element on hero prefab
        // TODO

        // Get all health elements within health bar
        Transform monsterHealthBackground = monsterhealthBar.Find("HealthBackground");
        Transform monsterHealthFillImage = monsterhealthBar.Find("Health");
        Transform monsterHealthText = monsterhealthBar.Find("HealthText");
        //Transform heroHealthBackground = heroHealthBar.Find("HealthBackground");
        //Transform heroHealthFillImage = heroHealthBar.Find("Health");
        //Transform heroHealthText = heroHealthBar.Find("HealthText");

        // Assert that all health elements are not null
        Assert.IsNotNull(monsterHealthBackground, "HealthBar Background on monster is null!");
        Assert.IsNotNull(monsterHealthFillImage, "HealthBar Fill Image on monster is null!");
        Assert.IsNotNull(monsterHealthText, "HealthBar Text on monster is null!");
        //Assert.IsNotNull(heroHealthBackground, "HealthBar Background on hero is null!");
        //Assert.IsNotNull(heroHealthFillImage, "HealthBar Fill Image on hero is null!");
        //Assert.IsNotNull(heroHealthText, "HealthBar Text on hero is null!");

        yield return null;
    }

    [UnityTearDown]
    public IEnumerator UnityTearDown()
    {
        // Exit network
        GameObject networkManagerV2 = GameObject.Find("NetworkManagerV2");
        NetworkManagerExtension networkManagerExtension = networkManagerV2.GetComponent<NetworkManagerExtension>();
        networkManagerExtension.StopHost();

        yield return null;
    }
}
