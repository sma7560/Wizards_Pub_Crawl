using NUnit.Framework;
using UnityEngine;

/// <summary>
/// This class holds all unit tests for the CharacterMovement.cs class in the Scripts > Hero folder.
/// </summary>
public class CharacterMovementUnitTest
{
    /// <summary>
    /// Testing function: Calculate(bool, bool, bool, bool)
    /// Tests for expected value (normal, positive case).
    /// </summary>
    [Test]
    public void CharacterMovement_Calculate_ShouldReturnExpectedPositive()
    {
        // Set character movement
        CharacterMovement characterMovement = new CharacterMovement(5.0f);
        Vector3 calculatedVector = characterMovement.Calculate(true, false, false, true);

        // Assert expected value with acceptable delta of 0.1
        Assert.AreEqual(5, calculatedVector.x, 0.1f, "x-axis value returned is not the expected value");
        Assert.AreEqual(0, calculatedVector.z, 0.1f, "z-axis value returned is not the expected value");
        Assert.AreEqual(0, calculatedVector.y, 0.1f, "y-axis value returned is not the expected value");
    }

    /// <summary>
    /// Testing function: Calculate(bool, bool, bool, bool)
    /// Tests for expected value (zero case).
    /// </summary>
    [Test]
    public void CharacterMovement_Calculate_ShouldReturnZero()
    {
        // Set character movement
        CharacterMovement characterMovement = new CharacterMovement(5.0f);
        Vector3 calculatedVector = characterMovement.Calculate(false, false, false, false);

        // Assert expected value with acceptable delta of 0.1
        Assert.AreEqual(0, calculatedVector.x, 0.1f, "x-axis value returned is not the expected value");
        Assert.AreEqual(0, calculatedVector.z, 0.1f, "z-axis value returned is not the expected value");
        Assert.AreEqual(0, calculatedVector.y, 0.1f, "y-axis value returned is not the expected value");
    }

    /// <summary>
    /// Testing function: Calculate(bool, bool, bool, bool)
    /// Tests for expected value (negative case).
    /// </summary>
    [Test]
    public void CharacterMovement_Calculate_ShouldReturnExpectedNegative()
    {
        // Set character movement
        CharacterMovement characterMovement = new CharacterMovement(10.0f);
        Vector3 calculatedVector = characterMovement.Calculate(false, true, true, false);

        //// Assert expected value with acceptable delta of 0.1
        Assert.AreEqual(-10, calculatedVector.x, 0.1f, "x-axis value returned is not the expected value");
        Assert.AreEqual(0, calculatedVector.z, 0.1f, "z-axis value returned is not the expected value");
        Assert.AreEqual(0, calculatedVector.y, 0.1f, "y-axis value returned is not the expected value");
    }
}
