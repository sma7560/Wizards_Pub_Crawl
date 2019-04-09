using UnityEngine;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using TMPro;
using UnityEngine.UI;

/// <summary>
/// This class contains system tests for prephase on the TestScene.
/// These tests run on Play Mode in Unity's Test Runner.
/// NOTE: Currently fails because setting up of all components in the TestScene without a network is VERY difficult. However, the idea of system testing is there.
/// </summary>
public class PlayerPrephasePlayTest
{
    private readonly int timeToWait = 2;    // number of seconds to wait for after test scene is loaded

    [UnitySetUp]
    public IEnumerator SetUp()
    {
        // Load Test scene
        SceneManager.LoadSceneAsync("TestScene", LoadSceneMode.Single);

        // Wait for test scene to be loaded
        yield return new WaitForSeconds(timeToWait);

        // Enable necessary components
        GameObject.Find("PrephaseActivate").transform.Find("MatchManager").gameObject.SetActive(true);
        GameObject.Find("PrephaseActivate").transform.Find("PrephaseScreen").gameObject.SetActive(true);

        // Wait for components to become active
        yield return new WaitForSeconds(timeToWait);
    }

    /// <summary>
    /// Checks that players can equip skills from skill bank during the pre-phase.
    /// </summary>
    [UnityTest]
    public IEnumerator PlayerPrephase_EquipSkills()
    {
        // Ensure that skill in equip slot is initially empty
        Assert.IsNull(GameObject.Find("EquipSkill1").GetComponent<SkillHoverDescription>().skill, "Skill in equip slot is not empty!");

        // Click on first skill in skill bank
        PointerEventData eventData = new PointerEventData(EventSystem.current);
        GameObject.Find("Skill1").GetComponent<SkillHoverDescription>().OnPointerClick(eventData);

        // Check that skill in equip slot is now the clicked skill
        Assert.AreEqual(GameObject.Find("Skill1").GetComponent<SkillHoverDescription>().skill,
            GameObject.Find("EquipSkill1").GetComponent<SkillHoverDescription>().skill,
            "Skill in equip slot is not empty!");

        yield return null;
    }

    /// <summary>
    /// Checks that players can unequip their skills during pre-phase.
    /// </summary>
    [UnityTest]
    public IEnumerator PlayerPrephase_UnequipSkills()
    {
        // First equip a skill
        PointerEventData eventData = new PointerEventData(EventSystem.current);
        GameObject.Find("Skill1").GetComponent<SkillHoverDescription>().OnPointerClick(eventData);

        // Unequip the skill
        GameObject.Find("EquipSkill1").GetComponent<SkillHoverDescription>().OnPointerClick(eventData);

        // Ensure that skill in equip slot is now empty
        Assert.IsNull(GameObject.Find("EquipSkill1").GetComponent<SkillHoverDescription>().skill, "Skill in equip slot is not empty!");

        yield return null;
    }

    /// <summary>
    /// Checks that player can select a different hero from a list of presets during the pre-phase.
    /// </summary>
    [UnityTest]
    public IEnumerator PlayerPrephase_SelectHero()
    {
        // Get initially selected hero
        string initialHero = GameObject.Find("CharacterNameText").GetComponent<TextMeshProUGUI>().text;

        // Click on arrow to select next hero
        GameObject.Find("CharSelectLeftButton").GetComponent<Button>().onClick.Invoke();
        string newHero = GameObject.Find("CharacterNameText").GetComponent<TextMeshProUGUI>().text;

        // Assert that selected hero is now different from initially selected hero
        Assert.AreNotEqual(initialHero, newHero, "Selected hero did not change!");

        yield return null;
    }
}
