using UnityEngine;
using NUnit.Framework;

/// <summary>
/// This class holds all unit tests for the EnemyModel.cs class in the Scripts > Enemy folder.
/// </summary>
public class EnemyModelUnitTest
{
    /// <summary>
    /// Testing functions: SetCurrentHealth() and GetCurrentHealth()
    /// Tests that enemy returns expected HP value.
    /// </summary>
    [Test]
    public void EnemyModel_SetGetCurrentHealth_ShouldReturnExpectedPositive()
    {
        // Setup variables
        int expectedHealth = 100;
        GameObject gameObject = new GameObject();
        EnemyModel enemyStats = gameObject.AddComponent<EnemyModel>();

        // Set the current health
        enemyStats.SetCurrentHealth(100);

        // Assert current health is the expected value
        Assert.AreEqual(expectedHealth, enemyStats.GetCurrentHealth(), "Current health is not equal to the expected health!");
    }

    /// <summary>
    /// Testing functions: SetCurrentHealth() and GetCurrentHealth()
    /// Tests that setting a negative value for health will result in current health of 0.
    /// </summary>
    [Test]
    public void EnemyModel_SetGetCurrentHealth_ShouldReturnZero()
    {
        // Setup variables
        GameObject gameObject = new GameObject();
        EnemyModel enemyStats = gameObject.AddComponent<EnemyModel>();

        // Set the current health
        enemyStats.SetCurrentHealth(-50);

        // Assert current health is the expected value
        Assert.AreEqual(0, enemyStats.GetCurrentHealth(), "Current health is not 0!");
    }
}
