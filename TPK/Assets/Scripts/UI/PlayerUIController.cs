using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class PlayerUIController : MonoBehaviour
{
    public TextMeshProUGUI timeBox;

    private MatchManager matchManager;
    private HeroManager heroManager;
    private AbilityManager abilityManager;

    private TextMeshProUGUI cooldown1;
    private TextMeshProUGUI cooldown2;
    private TextMeshProUGUI cooldown3;
    private TextMeshProUGUI cooldown4;

    /// <summary>
    /// Initialize variables.
    /// </summary>
    void Start()
    {
        // Setup managers
        matchManager = GameObject.FindGameObjectWithTag("MatchManager").GetComponent<MatchManager>();
        heroManager = GameObject.FindGameObjectWithTag("MatchManager").GetComponent<HeroManager>();
        abilityManager = heroManager.GetHeroObject(matchManager.GetPlayerId()).GetComponent<AbilityManager>();

        // Setup cooldowns
        cooldown1 = GameObject.Find("Cooldown1").GetComponent<TextMeshProUGUI>();
        cooldown2 = GameObject.Find("Cooldown2").GetComponent<TextMeshProUGUI>();
        cooldown3 = GameObject.Find("Cooldown3").GetComponent<TextMeshProUGUI>();
        cooldown4 = GameObject.Find("Cooldown4").GetComponent<TextMeshProUGUI>();
        cooldown1.gameObject.SetActive(false);
        cooldown2.gameObject.SetActive(false);
        cooldown3.gameObject.SetActive(false);
        cooldown4.gameObject.SetActive(false);

        SetupSkillIcons();
    }

    /// <summary>
    /// Update UI elements every frame.
    /// </summary>
    void Update()
    {
        UpdateTimeLeftUI(matchManager.GetTimeLeftInMatch());
        UpdateCooldowns();
    }

    /// <summary>
    /// Updates the match time left in player UI.
    /// </summary>
    /// <param name="timeLeft">Seconds left in the match.</param>
    public void UpdateTimeLeftUI(float timeLeft)
    {
        int minuteLeft = Mathf.FloorToInt(timeLeft / 60F);
        int secondLeft = Mathf.FloorToInt(timeLeft - minuteLeft * 60);

        if (secondLeft >= 10)
        {
            timeBox.SetText(minuteLeft.ToString() + " : " + secondLeft.ToString());
        }
        else
        {
            timeBox.SetText(minuteLeft.ToString() + " : 0" + secondLeft.ToString());
        }
    }

    /// <summary>
    /// Sets the icon sprites in player UI.
    /// </summary>
    private void SetupSkillIcons()
    {
        // Set the sprite image
        for (int i = 0; i < abilityManager.equippedSkills.Length; i++)
        {
            if (abilityManager.equippedSkills[i] != null)
            {
                GameObject skill = GameObject.Find("PlayerUISkill" + (i + 1));
                Image skillImg = skill.GetComponent<Image>();
                skillImg.sprite = abilityManager.equippedSkills[i].skillIcon;
            }
        }
    }

    /// <summary>
    /// Update the cooldown counter on each skill.
    /// </summary>
    private void UpdateCooldowns()
    {
        for (int i = 0; i < abilityManager.equippedSkills.Length; i++)
        {
            if (abilityManager.equippedSkills[i] != null)
            {
                int cooldown = (int)(Math.Ceiling(abilityManager.nextActiveTime[i] - Time.time));
                Image skillImg = GameObject.Find("PlayerUISkill" + (i + 1)).GetComponent<Image>();

                if (i == 0)
                {
                    if (cooldown > 0)
                    {
                        cooldown1.gameObject.SetActive(true);
                        cooldown1.text = cooldown.ToString();
                        skillImg.color = new Color(skillImg.color.r, skillImg.color.g, skillImg.color.b, 0.5f);
                    }
                    else
                    {
                        cooldown1.gameObject.SetActive(false);
                        skillImg.color = new Color(skillImg.color.r, skillImg.color.g, skillImg.color.b, 1f);
                    }
                }
                else if (i == 1)
                {
                    if (cooldown > 0)
                    {
                        cooldown2.gameObject.SetActive(true);
                        cooldown2.text = cooldown.ToString();
                        skillImg.color = new Color(skillImg.color.r, skillImg.color.g, skillImg.color.b, 0.5f);
                    }
                    else
                    {
                        cooldown2.gameObject.SetActive(false);
                        skillImg.color = new Color(skillImg.color.r, skillImg.color.g, skillImg.color.b, 1f);
                    }
                }
                else if (i == 2)
                {
                    if (cooldown > 0)
                    {
                        cooldown3.gameObject.SetActive(true);
                        cooldown3.text = cooldown.ToString();
                        skillImg.color = new Color(skillImg.color.r, skillImg.color.g, skillImg.color.b, 0.5f);
                    }
                    else
                    {
                        cooldown3.gameObject.SetActive(false);
                        skillImg.color = new Color(skillImg.color.r, skillImg.color.g, skillImg.color.b, 1f);
                    }
                }
                else if (i == 3)
                {
                    if (cooldown > 0)
                    {
                        cooldown4.gameObject.SetActive(true);
                        cooldown4.text = cooldown.ToString();
                        skillImg.color = new Color(skillImg.color.r, skillImg.color.g, skillImg.color.b, 0.5f);
                    }
                    else
                    {
                        cooldown4.gameObject.SetActive(false);
                        skillImg.color = new Color(skillImg.color.r, skillImg.color.g, skillImg.color.b, 1f);
                    }
                }
            }
        }
    }
}
