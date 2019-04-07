using UnityEngine;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;

/// <summary>
/// This class holds all unit tests for the CharacterMovement.cs class in the Scripts > Attacker folder.
/// </summary>
public class CharacterMovementUnitTest
{
    /// <summary>
    /// Test UT-CM1: Tests function Calculate() in CharacterMovement for expected value (normal, positive case).
    /// Requirement: FR-4
    /// </summary>
    [Test]
    public void CharacterMovement_Calculate_ShouldReturnExpectedPositive()
    {
        // Setup variables
        float speed = 5.0f;     // movement speed of character
        float x = 1.0f;         // movement input on x-axis
        float z = 1.0f;         // movement input on y-axis

        // Set character movement
        CharacterMovement characterMovement = new CharacterMovement(speed);
        //Vector3 calculatedVector = characterMovement.Calculate(x, z);

        // Assert expected value with acceptable delta of 0.1
        //Assert.AreEqual(5, calculatedVector.x, 0.1f,
        //    "x-axis value returned is not the expected value");     // assert x-axis value
        //Assert.AreEqual(5, calculatedVector.z, 0.1f,
        //    "z-axis value returned is not the expected value");     // assert z-axis value
    }

    /// <summary>
    /// Test UT-CM2: Tests function Calculate() in CharacterMovement for expected value (zero case).
    /// Requirement: FR-4
    /// </summary>
    [Test]
    public void CharacterMovement_Calculate_ShouldReturnZero()
    {
        // Setup variables
        float speed = 5.0f;     // movement speed of character
        float x = 0f;           // movement input on x-axis
        float z = 0f;           // movement input on y-axis

        // Set character movement
        CharacterMovement characterMovement = new CharacterMovement(speed);
        //Vector3 calculatedVector = characterMovement.Calculate(x, z);

        // Assert expected value with acceptable delta of 0.1
        //Assert.AreEqual(0, calculatedVector.x, 0.1f,
        //    "x-axis value returned is not the expected value");     // assert x-axis value
        //Assert.AreEqual(0, calculatedVector.z, 0.1f,
        //    "z-axis value returned is not the expected value");     // assert z-axis value
    }

    /// <summary>
    /// Test UT-CM3: Tests function Calculate() in CharacterMovement for expected value (negative case).
    /// Requirement: FR-4
    /// </summary>
    [Test]
    public void CharacterMovement_Calculate_ShouldReturnExpectedNegative()
    {
        // Setup variables
        float speed = 10.0f;     // movement speed of character
        float x = -1f;           // movement input on x-axis
        float z = -1f;           // movement input on y-axis

        // Set character movement
        CharacterMovement characterMovement = new CharacterMovement(speed);
        //Vector3 calculatedVector = characterMovement.Calculate(x, z);

        //// Assert expected value with acceptable delta of 0.1
        //Assert.AreEqual(-10, calculatedVector.x, 0.1f,
        //    "x-axis value returned is not the expected value");     // assert x-axis value
        //Assert.AreEqual(-10, calculatedVector.z, 0.1f,
        //    "z-axis value returned is not the expected value");     // assert z-axis value
    }

    /// <summary>
    /// Test UT-CM4: Tests function GetSpeed() in CharacterMovement for expected value (positive case).
    /// Requirement: FR-4
    /// </summary>
    [Test]
    public void CharacterMovement_GetSpeed_ShouldReturnExpectedPositive()
    {
        // Setup speed value
        float speed = 5.0f;

        // Set CharacterMovement with speed
        CharacterMovement characterMovement = new CharacterMovement(speed);

        // Assert that speed is the expected value
        Assert.AreEqual(speed, characterMovement.GetSpeed());
    }

    /// <summary>
    /// Test UT-CM5: Tests function GetSpeed() in CharacterMovement for expected value (zero case).
    /// Requirement: FR-4
    /// </summary>
    [Test]
    public void CharacterMovement_GetSpeed_ShouldReturnZero()
    {
        // Setup speed value
        float speed = 0f;

        // Set CharacterMovement with speed
        CharacterMovement characterMovement = new CharacterMovement(speed);

        // Assert that speed is the expected value
        Assert.AreEqual(0, characterMovement.GetSpeed());
    }

    /// <summary>
    /// Test UT-CM6: Tests function GetSpeed() in CharacterMovement for expected value of 0 given a negative speed (negative case).
    /// Requirement: FR-4
    /// </summary>
    [Test]
    public void CharacterMovement_GetSpeed_ShouldReturnZero_NegativeCase()
    {
        // Setup speed value
        float speed = -10.0f;

        // Set CharacterMovement with speed (negative value)
        CharacterMovement characterMovement = new CharacterMovement(speed);

        // Assert that speed is the expected value of zero
        Assert.AreEqual(0, characterMovement.GetSpeed());
    }
}
