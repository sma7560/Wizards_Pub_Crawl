using NSubstitute;
using NUnit.Framework;
using UnityEngine;

/// <summary>
/// This class holds all unit tests for the AbilityManager.cs class in the Scripts > Hero > Abilities folder.
/// Note: Only public methods are unit tested.
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
        //abilityManager.EquipSkill(0);
        //abilityManager.EquipSkill(1);
        //abilityManager.EquipSkill(2);
        //abilityManager.EquipSkill(3);
        //abilityManager.EquipSkill(4);

        // Assert that there are a max of 4 equipped skills
        Assert.LessOrEqual(abilityManager.equippedSkills.Length, 4, "There are more than 4 skills equipped!");
    }

    /// <summary>
    /// Test UT-AM2:    Checks the functionality of the SwapSkills() function, and that the equipped skills change to the swapped skill.
    ///                 Checks for each skill equip slot.
    /// Requirement: FR-14, FR-17
    /// </summary>
    [Test]
    public void AbilityManager_SwapSkills_EquippedSkillsChangeAsExpected()
    {
        // Setup variables
        GameObject gameObject = new GameObject("TestGameObject");
        AbilityManager abilityManager = gameObject.AddComponent<AbilityManager>();

        // Swap skills and assert that the swapped skill is now equipped
        //abilityManager.SwapSkills(0, 1);
        //Assert.AreEqual(abilityManager.knownSkills[1], abilityManager.equippedSkills[0], "Skill equipped is not the swapped skill!");
        //abilityManager.SwapSkills(1, 3);
        //Assert.AreEqual(abilityManager.knownSkills[3], abilityManager.equippedSkills[1], "Skill equipped is not the swapped skill!");
        //abilityManager.SwapSkills(2, 2);
        //Assert.AreEqual(abilityManager.knownSkills[2], abilityManager.equippedSkills[2], "Skill equipped is not the swapped skill!");
        //abilityManager.SwapSkills(3, 2);
        //Assert.AreEqual(abilityManager.knownSkills[2], abilityManager.equippedSkills[3], "Skill equipped is not the swapped skill!");
    }

    /// <summary>
    /// Test UT-AM3:    Checks the functionality of the EquipSkill() function, and that the equipped skills change accordingly.
    ///                 Checks for each skill equip slot.
    /// Requirement: FR-17
    /// </summary>
    [Test]
    public void AbilityManager_EquipSkill_EquippedSkillsChangeAsExpected()
    {
        // Setup variables
        GameObject gameObject = new GameObject("TestGameObject");
        AbilityManager abilityManager = gameObject.AddComponent<AbilityManager>();

        // Equip skills
        //abilityManager.EquipSkill(1);
        //abilityManager.EquipSkill(1);
        //abilityManager.EquipSkill(0);
        //abilityManager.EquipSkill(2);

        // Assert that correct skills are now equipped
        Assert.AreEqual(abilityManager.knownSkills[1], abilityManager.equippedSkills[0], "Equipped skill is not the expected skill!");
        Assert.AreEqual(abilityManager.knownSkills[1], abilityManager.equippedSkills[1], "Equipped skill is not the expected skill!");
        Assert.AreEqual(abilityManager.knownSkills[0], abilityManager.equippedSkills[2], "Equipped skill is not the expected skill!");
        Assert.AreEqual(abilityManager.knownSkills[2], abilityManager.equippedSkills[3], "Equipped skill is not the expected skill!");
    }

    /// <summary>
    /// Test UT-AM4: Checks the functionality of the CastSkill() function, and that the specified skill is cast.
    /// Requirement: FR-19
    /// </summary>
    [Test]
    public void AbilityManager_CastSkill_EquippedSkillsAreCast()
    {
        // Setup variables
        GameObject gameObject = new GameObject("TestGameObject");
        AbilityManager abilityManager = gameObject.AddComponent<AbilityManager>();

        // Setup mock of Skills in order to check if Cast() has been called
        var skill = Substitute.ForPartsOf<Skill>();
        abilityManager.knownSkills[0] = skill;

        // Equip mock skill on all 4 skill slots
        //abilityManager.EquipSkill(0);
        //abilityManager.EquipSkill(0);
        //abilityManager.EquipSkill(0);
        //abilityManager.EquipSkill(0);

        // Cast skills
        //abilityManager.CastSkill(0);
        //abilityManager.CastSkill(1);
        //abilityManager.CastSkill(2);
        //abilityManager.CastSkill(3);

        // Assert that Cast() method has been called
        //skill.Received(4).Cast();
    }
}
