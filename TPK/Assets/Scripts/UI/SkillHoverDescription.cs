using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SkillHoverDescription : EventTrigger
{
    /// <summary>
    /// Enable skill description when skill is hovered over.
    /// </summary>
    public override void OnPointerEnter(PointerEventData eventData)
    {
        GameObject skillDescription = null;
        PrephaseManager prephaseManager = GameObject.FindGameObjectWithTag("MatchManager").GetComponent<PrephaseManager>();
        
        if (prephaseManager.IsCurrentlyInPrephase())
        {
            // Logic for skill hover on prephase screen
            PrephaseUI prephaseUI = GameObject.FindGameObjectWithTag("PrephaseUI").GetComponent<PrephaseUI>();
            skillDescription = prephaseUI.skillDescription;

            if (transform.name == "EquipSkill1" && !prephaseUI.skill1)
            {
                return;
            }
            else if (transform.name == "EquipSkill2" && !prephaseUI.skill2)
            {
                return;
            }
            else if (transform.name == "EquipSkill3" && !prephaseUI.skill3)
            {
                return;
            }
            else if (transform.name == "EquipSkill4" && !prephaseUI.skill4)
            {
                return;
            }
        }
        else
        {
            StatWindowUI statWindow = GameObject.Find("StatWindow").GetComponent<StatWindowUI>();
            skillDescription = statWindow.skillDescription;
        }

        skillDescription.SetActive(true);
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

        skillDescription.SetActive(false);
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

        PrephaseUI prephaseUI = GameObject.FindGameObjectWithTag("PrephaseUI").GetComponent<PrephaseUI>();

        // Skill in skill bank is clicked
        if (!transform.name.Contains("Equip"))
        {
            // Cycle through all equipped skills and add it to equipped skill if a slot is empty
            if (!prephaseUI.skill1)
            {
                prephaseUI.skill1 = true;
                Image equipSkill1 = GameObject.Find("EquipSkill1").GetComponent<Image>();
                equipSkill1.sprite = GetComponent<Image>().sprite;
            }
            else if (!prephaseUI.skill2)
            {
                prephaseUI.skill2 = true;
                Image equipSkill2 = GameObject.Find("EquipSkill2").GetComponent<Image>();
                equipSkill2.sprite = GetComponent<Image>().sprite;
            }
            else if (!prephaseUI.skill3)
            {
                prephaseUI.skill3 = true;
                Image equipSkill3 = GameObject.Find("EquipSkill3").GetComponent<Image>();
                equipSkill3.sprite = GetComponent<Image>().sprite;
            }
            else if (!prephaseUI.skill4)
            {
                prephaseUI.skill4 = true;
                Image equipSkill4 = GameObject.Find("EquipSkill4").GetComponent<Image>();
                equipSkill4.sprite = GetComponent<Image>().sprite;
            }
        }
        else
        {
            // Unequip the clicked equipped skill
            GetComponent<Image>().sprite = null;

            if (transform.name == "EquipSkill1")
            {
                prephaseUI.skill1 = false;
            }
            else if (transform.name == "EquipSkill2")
            {
                prephaseUI.skill2 = false;
            }
            else if (transform.name == "EquipSkill3")
            {
                prephaseUI.skill3 = false;
            }
            else if (transform.name == "EquipSkill4")
            {
                prephaseUI.skill4 = false;
            }
        }
    }

}
