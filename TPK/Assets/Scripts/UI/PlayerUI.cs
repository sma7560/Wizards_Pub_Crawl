using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUI : MonoBehaviour
{
    // Managers
    private MatchManager matchManager;
    private HeroManager heroManager;
    private AbilityManager abilityManager;

    // Text elements
    private TextMeshProUGUI timeLeft;
    private TextMeshProUGUI cooldown1;
    private TextMeshProUGUI cooldown2;
    private TextMeshProUGUI cooldown3;
    private TextMeshProUGUI cooldown4;
    private TextMeshProUGUI skill1;
    private TextMeshProUGUI skill2;
    private TextMeshProUGUI skill3;
    private TextMeshProUGUI skill4;

    private HeroModel heroModel;    // data of the current player's hero

    /// <summary>
    /// Initialize variables.
    /// </summary>
    void Start()
    {
        // Get managers
        matchManager = GameObject.FindGameObjectWithTag("MatchManager").GetComponent<MatchManager>();
        heroManager = GameObject.FindGameObjectWithTag("MatchManager").GetComponent<HeroManager>();
        abilityManager = heroManager.GetHeroObject(matchManager.GetPlayerId()).GetComponent<AbilityManager>();

        // Get time left text
        timeLeft = GameObject.Find("MatchTimeLeftText").GetComponent<TextMeshProUGUI>();

        // Setup cooldowns
        cooldown1 = GameObject.Find("Cooldown1").GetComponent<TextMeshProUGUI>();
        cooldown2 = GameObject.Find("Cooldown2").GetComponent<TextMeshProUGUI>();
        cooldown3 = GameObject.Find("Cooldown3").GetComponent<TextMeshProUGUI>();
        cooldown4 = GameObject.Find("Cooldown4").GetComponent<TextMeshProUGUI>();
        cooldown1.gameObject.SetActive(false);
        cooldown2.gameObject.SetActive(false);
        cooldown3.gameObject.SetActive(false);
        cooldown4.gameObject.SetActive(false);

        // Get skill text
        skill1 = GameObject.Find("Skill1Text").GetComponent<TextMeshProUGUI>();
        skill2 = GameObject.Find("Skill2Text").GetComponent<TextMeshProUGUI>();
        skill3 = GameObject.Find("Skill3Text").GetComponent<TextMeshProUGUI>();
        skill4 = GameObject.Find("Skill4Text").GetComponent<TextMeshProUGUI>();

        heroModel = heroManager.GetHeroObject(matchManager.GetPlayerId()).GetComponent<HeroModel>();

        SetupSkillIcons();
        SetupHealthBar();
    }

    /// <summary>
    /// Update UI elements every frame.
    /// </summary>
    void Update()
    {
        UpdateTimeLeftUI(matchManager.GetTimeLeftInMatch());
        UpdateCooldowns();
        UpdateHealthBar();
        UpdateScore();
        SetupSkillHotkeyText();
    }

    /// <summary>
    /// Sets the health bar image to full.
    /// </summary>
    private void SetupHealthBar()
    {
        Image healthImage = GameObject.FindGameObjectWithTag("Health").GetComponent<Image>();
        healthImage.fillAmount = 1;
    }

    /// <summary>
    /// Updates the health bar fill amount and health text in player UI based on the player's current health.
    /// </summary>
    private void UpdateHealthBar()
    {
        Image healthImage = GameObject.FindGameObjectWithTag("Health").GetComponent<Image>();
        healthImage.fillAmount = (float)heroModel.GetCurrentHealth() / (float)heroModel.GetMaxHealth();
        TextMeshProUGUI healthText = GameObject.FindGameObjectWithTag("HealthText").GetComponent<TextMeshProUGUI>();
        healthText.text = heroModel.GetCurrentHealth() + "/" + heroModel.GetMaxHealth();
    }

    /// <summary>
    /// Updates the current score in player UI.
    /// </summary>
    private void UpdateScore()
    {
        TextMeshProUGUI scoreText = GameObject.FindGameObjectWithTag("Score").GetComponent<TextMeshProUGUI>();
        scoreText.text = heroModel.GetScore().ToString();
    }

    /// <summary>
    /// Updates the match time left in player UI.
    /// </summary>
    /// <param name="timeLeft">Seconds left in the match.</param>
    private void UpdateTimeLeftUI(float timeLeft)
    {
        int minuteLeft = Mathf.FloorToInt(timeLeft / 60F);
        int secondLeft = Mathf.FloorToInt(timeLeft - minuteLeft * 60);

        if (secondLeft >= 10)
        {
            this.timeLeft.text = minuteLeft.ToString() + " : " + secondLeft.ToString();
        }
        else
        {
            this.timeLeft.text = minuteLeft.ToString() + " : 0" + secondLeft.ToString();
        }
    }

    /// <summary>
    /// Sets the skill icon sprite images in player UI to the appropriate equipped skill's sprite.
    /// </summary>
    private void SetupSkillIcons()
    {
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
    /// Sets text underneath the skills to the appropriate hotkey name.
    /// </summary>
    private void SetupSkillHotkeyText()
    {
        skill1.text = CustomKeyBinding.GetKeyName(CustomKeyBinding.GetSkill1Key());
        skill2.text = CustomKeyBinding.GetKeyName(CustomKeyBinding.GetSkill2Key());
        skill3.text = CustomKeyBinding.GetKeyName(CustomKeyBinding.GetSkill3Key());
        skill4.text = CustomKeyBinding.GetKeyName(CustomKeyBinding.GetSkill4Key());
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

                switch (i)
                {
				case (0):
					if (cooldown > 0) {
						cooldown1.gameObject.SetActive (true);
						cooldown1.text = cooldown.ToString ();
						skillImg.color = new Color (skillImg.color.r, skillImg.color.g, skillImg.color.b, 0.5f);
					} else {
						cooldown1.gameObject.SetActive (false);
						skillImg.color = new Color (skillImg.color.r, skillImg.color.g, skillImg.color.b, 1f);
					}
					break;
                
				case(1):
                
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
					break;
				case(2):
					if (cooldown > 0) {
						cooldown3.gameObject.SetActive (true);
						cooldown3.text = cooldown.ToString ();
						skillImg.color = new Color (skillImg.color.r, skillImg.color.g, skillImg.color.b, 0.5f);
					} else {
						cooldown3.gameObject.SetActive (false);
						skillImg.color = new Color (skillImg.color.r, skillImg.color.g, skillImg.color.b, 1f);
					}
					break;
				case(3):
					if (cooldown > 0) {
						cooldown4.gameObject.SetActive (true);
						cooldown4.text = cooldown.ToString ();
						skillImg.color = new Color (skillImg.color.r, skillImg.color.g, skillImg.color.b, 0.5f);
					} else {
						cooldown4.gameObject.SetActive (false);
						skillImg.color = new Color (skillImg.color.r, skillImg.color.g, skillImg.color.b, 1f);
					}
					break;
                }
            }
        }
    }
}
