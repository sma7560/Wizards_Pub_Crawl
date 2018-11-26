using UnityEngine;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;
using NSubstitute;

/// <summary>
/// This class holds all unit tests for the CharacterCombat.cs class in the Scripts > Combat folder.
/// Note: Only public methods are unit tested.
/// </summary>
public class CharacterCombatUnitTest
{

    /// <summary>
    /// Test UT-CC1: Checks the functionality of the Attack(target) method, and that the targeted object takes damage.
    /// Requirement: FR-27, FR-28, FR-55
    /// </summary>
    [UnityTest]
    public IEnumerator CharacterCombat_Attack_AttacksTarget_TargetTakesDamage()
    {
        // Setup variables
        GameObject gameObject = new GameObject("TestGameObject");
        GameObject targetObject = new GameObject("TestTargetObject");
        CharacterStats targetStats = targetObject.AddComponent<CharacterStats>();
        CharacterStats characterStats = gameObject.AddComponent<CharacterStats>();
        CharacterCombat characterCombat = gameObject.AddComponent<CharacterCombat>();
        characterCombat.runInEditMode = true;

        yield return null;

        // Setup CharacterStats
        targetStats.localTest = true;
        targetStats.maxHealth = 100;
        targetStats.SetCurrentHealth(100);

        // Mock damage stat
        var damage = Substitute.ForPartsOf<Stat>();
        damage.When(x => x.GetValue()).DoNotCallBase();
        damage.GetValue().Returns(10);
        characterStats.damage = damage;

        // Mock defence stat
        var defence = Substitute.ForPartsOf<Stat>();
        defence.When(x => x.GetValue()).DoNotCallBase();
        defence.GetValue().Returns(0);
        targetStats.defence = defence;

        // Mock attackSpeed stat
        var attackSpeed = Substitute.ForPartsOf<Stat>();
        attackSpeed.When(x => x.GetValue()).DoNotCallBase();
        attackSpeed.GetValue().Returns(1);
        characterStats.attackSpeed = attackSpeed;

        // Attack target object
        characterCombat.Attack(targetObject.transform);

        // Assert target object has taken some damage
        Assert.Less(targetStats.GetCurrentHealth(), 100, "Target did not take damage upon being attacked!");
    }

}
