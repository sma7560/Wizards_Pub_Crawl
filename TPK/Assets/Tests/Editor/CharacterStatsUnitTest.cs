using UnityEngine;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;
using NSubstitute;

/// <summary>
/// This class holds all unit tests for the CharacterStats.cs class in the Scripts > Combat folder.
/// </summary>
public class CharacterStatsUnitTest {

    /// <summary>
    /// Test UT-CS1: Tests the GetCurrentHealth() function, and that hero has the expected value of HP.
    /// Requirement: FR-20
    /// </summary>
    [Test]
    public void CharacterStats_GetCurrentHealth_ShouldReturnExpectedPositive()
    {
        // Setup variables
        int expectedHealth = 100;
        CharacterStats characterStats = new CharacterStats();
        characterStats.maxHealth = expectedHealth;

        // Set the current health
        characterStats.SetCurrentHealth(100);

        // Assert current health is the expected value
        Assert.AreEqual(expectedHealth, characterStats.GetCurrentHealth(), "Current health is not equal to the expected health!");
    }

    /// <summary>
    /// Test UT-CS2: Tests the Set and GetCurrentHealth() function, and that setting a negative value for health will result in current health of 0.
    /// Requirement: N/A
    /// </summary>
    [Test]
    public void CharacterStats_SetGetCurrentHealth_ShouldReturnZero()
    {
        // Setup variables
        CharacterStats characterStats = new CharacterStats();

        // Set the current health
        characterStats.SetCurrentHealth(-50);

        // Assert current health is the expected value
        Assert.AreEqual(0, characterStats.GetCurrentHealth(), "Current health is not 0!");
    }

    /// <summary>
    /// Test UT-CS3: Tests the TakeDamage() function, and that the hero's health after taking damage is as expected.
    /// Requirement: FR-27, FR-54, FR-55
    /// </summary>
    [Test]
    public void CharacterStats_TakeDamage_NoDefence_ShouldReturnExpected()
    {
        // Setup variables
        GameObject gameObject = new GameObject("TestGameObject");
        CharacterStats characterStats = gameObject.AddComponent<CharacterStats>();
        characterStats.maxHealth = 100;
        characterStats.SetCurrentHealth(100);
        characterStats.localTest = true;

        // Mock defence stat
        var defence = Substitute.ForPartsOf<Stat>();
        defence.When(x => x.GetValue()).DoNotCallBase();
        defence.GetValue().Returns(0);
        characterStats.defence = defence;

        // Make hero take damage
        characterStats.TakeDamage(10);

        // Assert current health is the expected value
        Assert.AreEqual(90, characterStats.GetCurrentHealth(), "Current health is not the expected value!");
    }

    /// <summary>
    /// Test UT-CS4: Tests the TakeDamage() function with some defence, and that the hero's health after taking damage is as expected.
    /// Requirement: FR-27, FR-54, FR-55
    /// </summary>
    [Test]
    public void CharacterStats_TakeDamage_PositiveDefence_ShouldReturnExpected()
    {
        // Setup variables
        GameObject gameObject = new GameObject("TestGameObject");
        CharacterStats characterStats = gameObject.AddComponent<CharacterStats>();
        characterStats.maxHealth = 100;
        characterStats.SetCurrentHealth(100);
        characterStats.localTest = true;

        // Mock defence stat
        var defence = Substitute.ForPartsOf<Stat>();
        defence.When(x => x.GetValue()).DoNotCallBase();
        defence.GetValue().Returns(10);
        characterStats.defence = defence;

        // Make hero take damage
        characterStats.TakeDamage(15);

        // Assert current health is the expected value
        Assert.AreEqual(95, characterStats.GetCurrentHealth(), "Current health is not the expected value!");
    }

}
