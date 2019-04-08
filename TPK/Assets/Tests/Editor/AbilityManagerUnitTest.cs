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
    /// Testing function: EquipSkill(Skill)
    /// Checks that there are a max of 4 skills equipped.
    /// </summary>
    [Test]
    public void AbilityManager_EquippedSkills_ExpectedMax4Equipped()
    {
        // Setup variables
        GameObject gameObject = new GameObject();
        AbilityManager abilityManager = gameObject.AddComponent<AbilityManager>();
        abilityManager.equippedSkills = new Skill[4];
        Skill skill1 = ScriptableObject.CreateInstance<Skill>();
        Skill skill2 = ScriptableObject.CreateInstance<Skill>();
        Skill skill3 = ScriptableObject.CreateInstance<Skill>();
        Skill skill4 = ScriptableObject.CreateInstance<Skill>();
        Skill skill5 = ScriptableObject.CreateInstance<Skill>();

        // Attempt to add equip more than 4 skills
        abilityManager.EquipSkill(skill1);
        abilityManager.EquipSkill(skill2);
        abilityManager.EquipSkill(skill3);
        abilityManager.EquipSkill(skill4);
        abilityManager.EquipSkill(skill5);

        // Assert that there are a max of 4 equipped skills
        Assert.LessOrEqual(abilityManager.equippedSkills.Length, 4, "There are more than 4 skills equipped!");
    }

    /// <summary>
    /// Testing function: EquipSkill(Skill)
    /// Checks that skills are equipped as expected. Checks each skill equip slot.
    /// </summary>
    [Test]
    public void AbilityManager_EquipSkill_EquippedSkillsChangeAsExpected()
    {
        // Setup variables
        GameObject gameObject = new GameObject();
        AbilityManager abilityManager = gameObject.AddComponent<AbilityManager>();
        abilityManager.equippedSkills = new Skill[4];
        Skill skill1 = ScriptableObject.CreateInstance<Skill>();
        Skill skill2 = ScriptableObject.CreateInstance<Skill>();
        Skill skill3 = ScriptableObject.CreateInstance<Skill>();
        Skill skill4 = ScriptableObject.CreateInstance<Skill>();

        // Equip skills
        abilityManager.EquipSkill(skill1);
        abilityManager.EquipSkill(skill2);
        abilityManager.EquipSkill(skill3);
        abilityManager.EquipSkill(skill4);

        // Assert that correct skills are now equipped
        Assert.AreEqual(skill1, abilityManager.equippedSkills[0], "Equipped skill is not the expected skill!");
        Assert.AreEqual(skill2, abilityManager.equippedSkills[1], "Equipped skill is not the expected skill!");
        Assert.AreEqual(skill3, abilityManager.equippedSkills[2], "Equipped skill is not the expected skill!");
        Assert.AreEqual(skill4, abilityManager.equippedSkills[3], "Equipped skill is not the expected skill!");
    }

    /// <summary>
    /// Testing function: EquipSkill(Skill, int)
    /// Checks that the equipped skill is at the expected index. Checks each skill equip slot.
    /// </summary>
    [Test]
    public void AbilityManager_EquipSkill_EquippedSkillsAtIndexExpected()
    {
        // Setup variables
        GameObject gameObject = new GameObject();
        AbilityManager abilityManager = gameObject.AddComponent<AbilityManager>();
        abilityManager.equippedSkills = new Skill[4];
        Skill skill1 = ScriptableObject.CreateInstance<Skill>();
        Skill skill2 = ScriptableObject.CreateInstance<Skill>();
        Skill skill3 = ScriptableObject.CreateInstance<Skill>();
        Skill skill4 = ScriptableObject.CreateInstance<Skill>();

        // Equip skills
        abilityManager.EquipSkill(skill1, 1);
        abilityManager.EquipSkill(skill2, 2);
        abilityManager.EquipSkill(skill3, 0);
        abilityManager.EquipSkill(skill4, 3);

        // Assert that correct skills are now equipped
        Assert.AreEqual(skill1, abilityManager.equippedSkills[1], "Equipped skill is not the expected skill!");
        Assert.AreEqual(skill2, abilityManager.equippedSkills[2], "Equipped skill is not the expected skill!");
        Assert.AreEqual(skill3, abilityManager.equippedSkills[0], "Equipped skill is not the expected skill!");
        Assert.AreEqual(skill4, abilityManager.equippedSkills[3], "Equipped skill is not the expected skill!");
    }

    /// <summary>
    /// Testing function: UnequipSkill(Skill)
    /// Checks that the skill is properly unequipped. Checks for each skill equip slot.
    /// </summary>
    [Test]
    public void AbilityManager_UnequipSkill_UnequipAsExpected()
    {
        // Setup variables
        GameObject gameObject = new GameObject();
        AbilityManager abilityManager = gameObject.AddComponent<AbilityManager>();
        abilityManager.equippedSkills = new Skill[4];
        Skill skill1 = ScriptableObject.CreateInstance<Skill>();
        Skill skill2 = ScriptableObject.CreateInstance<Skill>();
        Skill skill3 = ScriptableObject.CreateInstance<Skill>();
        Skill skill4 = ScriptableObject.CreateInstance<Skill>();

        // Equip skills
        abilityManager.EquipSkill(skill1);
        abilityManager.EquipSkill(skill2);
        abilityManager.EquipSkill(skill3);
        abilityManager.EquipSkill(skill4);

        // Unequip skills
        abilityManager.UnequipSkill(skill1);
        abilityManager.UnequipSkill(skill2);
        abilityManager.UnequipSkill(skill3);
        abilityManager.UnequipSkill(skill4);

        // Assert that correct skills are now equipped
        Assert.AreEqual(null, abilityManager.equippedSkills[0], "A skill is equipped when it should not be!");
        Assert.AreEqual(null, abilityManager.equippedSkills[1], "A skill is equipped when it should not be!");
        Assert.AreEqual(null, abilityManager.equippedSkills[2], "A skill is equipped when it should not be!");
        Assert.AreEqual(null, abilityManager.equippedSkills[3], "A skill is equipped when it should not be!");
    }

    /// <summary>
    /// Testing function: IsEquipped(Skill)
    /// Checks that the function returns true as expected.
    /// </summary>
    [Test]
    public void AbilityManager_IsEquipped_ReturnExpected_True()
    {
        // Setup variables
        GameObject gameObject = new GameObject();
        AbilityManager abilityManager = gameObject.AddComponent<AbilityManager>();
        abilityManager.equippedSkills = new Skill[4];
        Skill skill = ScriptableObject.CreateInstance<Skill>();

        // Equip skills
        abilityManager.EquipSkill(skill);

        // Assert that correct skills are now equipped
        Assert.IsTrue(abilityManager.IsEquipped(skill), "Skill is said to not be equipped when it is!");
    }

    /// <summary>
    /// Testing function: IsEquipped(Skill)
    /// Checks that the function returns false as expected.
    /// </summary>
    [Test]
    public void AbilityManager_IsEquipped_ReturnExpected_False()
    {
        // Setup variables
        GameObject gameObject = new GameObject();
        AbilityManager abilityManager = gameObject.AddComponent<AbilityManager>();
        abilityManager.equippedSkills = new Skill[4];
        Skill skill = ScriptableObject.CreateInstance<Skill>();

        // Assert that correct skills are now equipped
        Assert.IsFalse(abilityManager.IsEquipped(skill), "Skill is said to be equipped when it is not!");
    }
}
