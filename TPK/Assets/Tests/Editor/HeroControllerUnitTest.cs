using UnityEngine;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;

/// <summary>
/// This class contains unit tests for HeroController.cs in the Attacker Scripts folder.
/// These tests run on Edit Mode in Unity's Test Runner, and utilize the NUnit test framework.
/// </summary>

public class HeroControllerUnitTest
{

    /// <summary>
    /// Test UT-HC1: Checks that Start() function in HeroController initializes all values correctly.
    /// </summary>
    [Test]
    public void HeroController_CharacterMovement_ShouldReturnExpected()
    {
        // Create the HeroController
        HeroController heroController = new HeroController();

        // Assert that movement is the expected default value

    }

}
