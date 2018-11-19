using UnityEngine;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;

/// <summary>
/// This class holds all unit tests for the AbilityManager.cs class in the Scripts > Attacker folder.
/// </summary>
public class AbilityManagerUnitTest
{
    /// <summary>
    /// Test UT-AM1: Checks that there are a max of 4 skills equipped.
    /// Requirement: FR-17
    /// </summary>
    [Test]
    public void AbilityManager_EquippedSkills_ExpectedMax4Equipped()
    {
        // Setup variables
        GameObject gameObject = new GameObject("TestGameObject");
        AbilityManager abilityManager = gameObject.AddComponent<AbilityManager>();

        // Attempt to add equip more than 4 skills
        abilityManager.EquipSkill(0);
        abilityManager.EquipSkill(1);
        abilityManager.EquipSkill(2);
        abilityManager.EquipSkill(3);
        abilityManager.EquipSkill(4);

        // Assert that there are a max of 4 equipped skills
        Assert.LessOrEqual(abilityManager.equippedSkills.Length, 4, "There are more than 4 skills equipped!");
    }
}
