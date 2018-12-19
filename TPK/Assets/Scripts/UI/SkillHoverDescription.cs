using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SkillHoverDescription : EventTrigger
{
    public Skill skill; // the current skill that this script is attached to

    /// <summary>
    /// Enable skill description when skill is hovered over.
    /// </summary>
    public override void OnPointerEnter(PointerEventData eventData)
    {
        GameObject skillDescription = null;
        TextMeshProUGUI skillDescriptionText = null;
        TextMeshProUGUI skillTitleText = null;
        PrephaseManager prephaseManager = GameObject.FindGameObjectWithTag("MatchManager").GetComponent<PrephaseManager>();
        AbilityManager abilityManager = GameObject.FindGameObjectWithTag("Player").GetComponent<AbilityManager>();

        if (prephaseManager.IsCurrentlyInPrephase())
        {
            // Logic for skill hover on prephase screen
            PrephaseUI prephaseUI = GameObject.FindGameObjectWithTag("PrephaseUI").GetComponent<PrephaseUI>();
            skillDescription = prephaseUI.skillDescription;
            skillDescriptionText = prephaseUI.skillDescriptionText;
            skillTitleText = prephaseUI.skillTitleText;

            // Do not display skill description for empty equipped skill slots
            if (transform.name == "EquipSkill1" && abilityManager.equippedSkills[0] == null)
            {
                return;
            }
            else if (transform.name == "EquipSkill2" && abilityManager.equippedSkills[1] == null)
            {
                return;
            }
            else if (transform.name == "EquipSkill3" && abilityManager.equippedSkills[2] == null)
            {
                return;
            }
            else if (transform.name == "EquipSkill4" && abilityManager.equippedSkills[3] == null)
            {
                return;
            }
        }
        else if (skill != null)
        {
            // Show skill description in stat window if skill is not null
            StatWindowUI statWindow = GameObject.Find("StatWindow").GetComponent<StatWindowUI>();
            skillDescription = statWindow.skillDescription;
            skillDescriptionText = statWindow.skillDescriptionText;
            skillTitleText = statWindow.skillDescriptionTitleText;
        }

        // Show skill description
        if (skillTitleText != null && skillDescriptionText != null && skillDescription != null)
        {
            skillTitleText.text = skill.skillName;
            skillDescriptionText.text = skill.skillDescription;
            skillDescription.SetActive(true);
        }
    }

    /// <summary>
    /// Disable skill description when skill is no longer hovered over.
    /// </summary>
    public override void OnPointerExit(PointerEventData eventData)
    {
        GameObject skillDescription = null;
        PrephaseManager prephaseManager = GameObject.FindGameObjectWithTag("MatchManager").GetComponent<PrephaseManager>();

        if (prephaseManager.IsCurrentlyInPrephase())
        {
            PrephaseUI prephaseUI = GameObject.FindGameObjectWithTag("PrephaseUI").GetComponent<PrephaseUI>();
            skillDescription = prephaseUI.skillDescription;
        }
        else
        {
            StatWindowUI statWindow = GameObject.Find("StatWindow").GetComponent<StatWindowUI>();
            skillDescription = statWindow.skillDescription;
        }

        if (skillDescription != null)
        {
            skillDescription.SetActive(false);
        }
    }

    /// <summary>
    /// When skill in skill bank is clicked, adds it to equipped skills. When equipped skill is clicked, unequips the skill.
    /// </summary>
    public override void OnPointerClick(PointerEventData data)
    {
        PrephaseManager prephaseManager = GameObject.FindGameObjectWithTag("MatchManager").GetComponent<PrephaseManager>();

        if (!prephaseManager.IsCurrentlyInPrephase())
        {
            // Do nothing if this is not the prephase screen
            return;
        }

        HeroManager heroManager = GameObject.FindGameObjectWithTag("MatchManager").GetComponent<HeroManager>();
        MatchManager matchManager = GameObject.FindGameObjectWithTag("MatchManager").GetComponent<MatchManager>();
        AbilityManager abilityManager = heroManager.GetHeroObject(matchManager.GetPlayerId()).GetComponent<AbilityManager>();
        PrephaseUI prephaseUI = GameObject.FindGameObjectWithTag("PrephaseUI").GetComponent<PrephaseUI>();

        if (!transform.name.Contains("Equip"))
        {
            // Skill in skill bank is clicked, attempt to equip the skill
            abilityManager.EquipSkill(skill);
        }
        else
        {
            // Unequip the clicked equipped skill
            abilityManager.UnequipSkill(skill);
        }

        // Reflect the equipped skills change in the UI
        prephaseUI.UpdateEquippedSkills();
    }
}
