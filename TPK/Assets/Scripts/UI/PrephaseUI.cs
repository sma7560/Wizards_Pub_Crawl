using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PrephaseUI : MonoBehaviour
{
    // SkillDescription GameObject
    [System.NonSerialized] public GameObject skillDescription;
    [System.NonSerialized] public TextMeshProUGUI skillDescriptionText;
    [System.NonSerialized] public TextMeshProUGUI skillTitleText;

    // Managers
    private PrephaseManager prephaseManager;
    private MatchManager matchManager;
    private HeroManager heroManager;
    private HeroModel heroModel;

    // Text elements
    private TextMeshProUGUI playerName;
    private TextMeshProUGUI characterSelectedName;
    private TextMeshProUGUI numOfPlayers;
    private TextMeshProUGUI timeLeft;
    private TextMeshProUGUI attack;
    private TextMeshProUGUI defense;
    private TextMeshProUGUI moveSpeed;

    // Default heroes
    private Hero king;
    private Hero wizard;
    private Hero rogue;
    private Hero armored;

    // Currently selected hero
    private Hero selectedHero;

    /// <summary>
    /// Initialize variables.
    /// </summary>
    void Start()
    {
        // Initialize variables
        skillDescription = GameObject.FindGameObjectWithTag("SkillDescription");
        skillDescriptionText = GameObject.Find("SkillDescriptionText").GetComponent<TextMeshProUGUI>();
        skillTitleText = GameObject.Find("SkillDescriptionTitleText").GetComponent<TextMeshProUGUI>();
        heroManager = GameObject.FindGameObjectWithTag("MatchManager").GetComponent<HeroManager>();
        prephaseManager = GameObject.FindGameObjectWithTag("MatchManager").GetComponent<PrephaseManager>();
        matchManager = GameObject.FindGameObjectWithTag("MatchManager").GetComponent<MatchManager>();
        heroModel = heroManager.GetHeroObject(matchManager.GetPlayerId()).GetComponent<HeroModel>();

        // Initialize text
        playerName = GameObject.Find("PlayerNameText").GetComponent<TextMeshProUGUI>();
        numOfPlayers = GameObject.Find("CurrentNumOfPlayersConnectedText").GetComponent<TextMeshProUGUI>();
        timeLeft = GameObject.Find("TimeLeftText").GetComponent<TextMeshProUGUI>();
        characterSelectedName = GameObject.Find("CharacterNameText").GetComponent<TextMeshProUGUI>();
        attack = GameObject.Find("AttackText").GetComponent<TextMeshProUGUI>();
        defense = GameObject.Find("DefText").GetComponent<TextMeshProUGUI>();
        moveSpeed = GameObject.Find("SpeedText").GetComponent<TextMeshProUGUI>();

        // Setup default values for default heroes
        SetupDefaultHeroes();

        // Set default currently selected character
        selectedHero = king;
        heroModel.SetModel(selectedHero);

        // Set skill description to inactive by default
        skillDescription.SetActive(false);

        // Update UI elements
        SetupDefaultStats();
        UpdateCharacterSelectedName();
        UpdateNumOfPlayers();
        UpdatePlayerName();
        UpdateSkillBank();
        UpdateEquippedSkills();
    }

    /// <summary>
    /// Update some UI elements every frame.
    /// </summary>
    void Update()
    {
        UpdateNumOfPlayers();
        UpdateTimeLeftUI();
        UpdateStats();
    }

    /// <summary>
    /// Updates the equipped skill sprites to reflect the currently equipped skills.
    /// </summary>
    public void UpdateEquippedSkills()
    {
        AbilityManager abilityManager = heroManager.GetHeroObject(matchManager.GetPlayerId()).GetComponent<AbilityManager>();

        // Set the sprite images and skill descriptions to the equipped skills
        for (int i = 0; i < abilityManager.equippedSkills.Length; i++)
        {
            // Get the equip skill object
            GameObject skill = GameObject.Find("EquipSkill" + (i + 1));
            Image skillImg = skill.GetComponent<Image>();

            if (abilityManager.equippedSkills[i] != null)
            {
                skill.GetComponent<SkillHoverDescription>().skill = abilityManager.equippedSkills[i];   // Set skill description
                skillImg.sprite = abilityManager.equippedSkills[i].skillIcon;   // Set sprite images
            }
            else
            {
                skill.GetComponent<SkillHoverDescription>().skill = null;
                skillImg.sprite = null;
            }
        }

        UpdateSkillBankAvailability();
    }

    /// <summary>
    /// Update "Time Left Until Match Start" UI text element.
    /// </summary>
    public void UpdateTimeLeftUI()
    {
        if (prephaseManager.GetState() == PrephaseManager.PrephaseState.WaitingForPlayers)
        {
            timeLeft.text = "Waiting for Players";
        }
        else if (prephaseManager.GetState() == PrephaseManager.PrephaseState.RoomFull)
        {
            timeLeft.text = prephaseManager.GetCountdown().ToString();
        }
        else
        {
            // No longer in prephase stage, therefore disable prephase UI
            gameObject.SetActive(false);
        }
    }

    /// <summary>
    /// Functionality for changing character selected when the corresponding button is pressed.
    /// Order is king < wizard < rogue.
    /// </summary>
    public void ChangeCharacterSelectionLeft()
    {
        if (selectedHero == king)
        {
            selectedHero = armored;
        }
        else if (selectedHero == wizard)
        {
            selectedHero = king;
        }
        else if (selectedHero == rogue)
        {
            selectedHero = wizard;
        }
        else if (selectedHero == armored)
        {
            selectedHero = rogue;
        }

        SetupDefaultStats();
        heroModel.SetModel(selectedHero);
        UpdateCharacterSelectedName();
    }

    /// <summary>
    /// Functionality for changing character selected when the corresponding button is pressed.
    /// Order is king > wizard > rogue.
    /// </summary>
    public void ChangeCharacterSelectionRight()
    {
        if (selectedHero == king)
        {
            selectedHero = wizard;
        }
        else if (selectedHero == wizard)
        {
            selectedHero = rogue;
        }
        else if (selectedHero == rogue)
        {
            selectedHero = armored;
        }
        else if (selectedHero == armored)
        {
            selectedHero = king;
        }

        SetupDefaultStats();
        heroModel.SetModel(selectedHero);
        UpdateCharacterSelectedName();
    }

    /// <summary>
    /// Returns the default hero based on its child index.
    /// </summary>
    /// <param name="index">child index of the wanted hero.</param>
    public Hero GetDefaultHero(int index)
    {
        switch (index)
        {
            case 0:
                return king;
            case 1:
                return rogue;
            case 2:
                return wizard;
            case 3:
                return armored;
            default:
                return null;
        }
    }

    /// <summary>
    /// Sets up the default hero types of king, wizard, and rogue.
    /// </summary>
    private void SetupDefaultHeroes()
    {
        // Setup default heroes
        king = ScriptableObject.CreateInstance<Hero>();
        king.heroType = HeroType.melee;
        king.heroName = "Kingly Wizard";
        king.childIndex = 0;

        wizard = ScriptableObject.CreateInstance<Hero>();
        wizard.heroType = HeroType.magic;
        wizard.heroName = "Classic Wizard";
        wizard.childIndex = 2;

        rogue = ScriptableObject.CreateInstance<Hero>();
        rogue.heroType = HeroType.melee;
        rogue.heroName = "Sneaky Wizard";
        rogue.childIndex = 1;

        armored = ScriptableObject.CreateInstance<Hero>();
        armored.heroType = HeroType.melee;
        armored.heroName = "Armoured Wizard";
        armored.childIndex = 3;
    }

    /// <summary>
    /// Sets the default stats of the hero.
    /// </summary>
    private void SetupDefaultStats()
    {
        // Set default stat values depending on the hero type
        if (selectedHero == king)
        {
            heroModel.SetAttack(10);
            heroModel.SetDefence(10);
            heroModel.SetBaseMoveSpeed(10);
        }
        else if (selectedHero == wizard)
        {
            heroModel.SetAttack(15);
            heroModel.SetDefence(5);
            heroModel.SetBaseMoveSpeed(10);
        }
        else if (selectedHero == rogue)
        {
            heroModel.SetAttack(10);
            heroModel.SetDefence(0);
            heroModel.SetBaseMoveSpeed(12);
        }
        else if (selectedHero == armored)
        {
            heroModel.SetAttack(10);
            heroModel.SetDefence(15);
            heroModel.SetBaseMoveSpeed(8);
        }
        else
        {
            Debug.Log("Selected Hero is not a valid hero.");
        }
    }

    /// <summary>
    /// Updates the "Number of the players connected" text in UI.
    /// </summary>
    private void UpdateNumOfPlayers()
    {
        numOfPlayers.text = matchManager.GetNumOfPlayers().ToString() + " out of " + matchManager.GetMaxPlayers();
    }

    /// <summary>
    /// Sets the name of the player in the UI.
    /// </summary>
    private void UpdatePlayerName()
    {
        playerName.text = "Player " + matchManager.GetPlayerId();
    }

    /// <summary>
    /// Update the "Selected Character" UI text element.
    /// </summary>
    private void UpdateCharacterSelectedName()
    {
        characterSelectedName.text = selectedHero.heroName;
    }

    /// <summary>
    /// Updates all stat text values in the Stats UI window.
    /// </summary>
    private void UpdateStats()
    {
        attack.text = heroModel.GetAttack().ToString();
        defense.text = heroModel.GetDefence().ToString();
        moveSpeed.text = heroModel.GetCurrentMoveSpeed().ToString();
    }

    /// <summary>
    /// Updates the icons for skill bank.
    /// </summary>
    private void UpdateSkillBank()
    {
        AbilityManager abilityManager = heroManager.GetHeroObject(matchManager.GetPlayerId()).GetComponent<AbilityManager>();

        // Set the sprite images and skill descriptions in the skill bank for each known skill
        for (int i = 0; i < abilityManager.knownSkills.Length; i++)
        {
            // Set skill description
            GameObject skill = GameObject.Find("Skill" + (i + 1));
            skill.GetComponent<SkillHoverDescription>().skill = abilityManager.knownSkills[i];

            // Set sprite images
            Image skillImg = skill.GetComponent<Image>();
            skillImg.sprite = abilityManager.knownSkills[i].skillIcon;
        }

        // Remove all empty skills in skill bank
        // NOTE: right now there are 10 max, may need to change this number in the future
        for (int i = 0; i < 10; i++)
        {
            GameObject skill = GameObject.Find("Skill" + (i + 1));
            if (skill.GetComponent<SkillHoverDescription>().skill == null)
            {
                skill.SetActive(false);
            }
        }
    }

    /// <summary>
    /// Updates the skill bank to fade out skills that are already equipped.
    /// </summary>
    private void UpdateSkillBankAvailability()
    {
        AbilityManager abilityManager = heroManager.GetHeroObject(matchManager.GetPlayerId()).GetComponent<AbilityManager>();

        // Set opacity of skill images in the skill bank to match availbility
        for (int i = 0; i < abilityManager.knownSkills.Length; i++)
        {
            GameObject skill = GameObject.Find("Skill" + (i + 1));
            Image skillImg = skill.GetComponent<Image>();
            var tempColor = skillImg.color;

            if (abilityManager.IsEquipped(abilityManager.knownSkills[i]))
            {
                // Set image opacity
                tempColor.a = 0.05f;
                skillImg.color = tempColor;
            }
            else
            {
                tempColor.a = 1.0f;
                skillImg.color = tempColor;
            }
        }
    }
}
