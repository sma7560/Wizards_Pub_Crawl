using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PrephaseUI : MonoBehaviour
{
    // SkillDescription GameObject
    public GameObject skillDescription;
    public TextMeshProUGUI skillDescriptionText;
    public TextMeshProUGUI skillTitleText;

    // Default stat values
    private readonly int defaultAtkSpd = 1;
    private readonly int defaultPhyDmg = 10;
    private readonly int defaultMagDmg = 10;
    private readonly int defaultPhyDef = 10;
    private readonly int defaultMagDef = 10;
    private readonly int defaultAttributePts = 20;

    // Managers
    private PrephaseManager prephaseManager;
    private NetworkHeroManager networkHeroManager;
    private MatchManager matchManager;
    private HeroManager heroManager;

    // Text elements
    private TextMeshProUGUI playerName;
    private TextMeshProUGUI attributePointsLeft;
    private TextMeshProUGUI characterSelectedName;
    private TextMeshProUGUI hostIP;
    private TextMeshProUGUI numOfPlayers;
    private TextMeshProUGUI timeLeft;
    private TextMeshProUGUI physicalDmg;
    private TextMeshProUGUI magicalDmg;
    private TextMeshProUGUI physicalDef;
    private TextMeshProUGUI magicalDef;
    private TextMeshProUGUI atkSpd;

    // Button elements
    private Button decreasePhysicalDmg;
    private Button increasePhysicalDmg;
    private Button decreaseMagicalDmg;
    private Button increaseMagicalDmg;
    private Button decreasePhysicalDef;
    private Button increasePhysicalDef;
    private Button decreaseMagicalDef;
    private Button increaseMagicalDef;
    private Button decreaseAtkSpd;
    private Button increaseAtkSpd;

    // Default heroes
    private Hero king;
    private Hero wizard;
    private Hero rogue;

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
        networkHeroManager = heroManager.GetHeroObject(matchManager.GetPlayerId()).GetComponent<NetworkHeroManager>();

        // Initialize text
        playerName = GameObject.Find("PlayerNameText").GetComponent<TextMeshProUGUI>();
        attributePointsLeft = GameObject.Find("PointsLeftText").GetComponent<TextMeshProUGUI>();
        hostIP = GameObject.Find("HostIPText").GetComponent<TextMeshProUGUI>();
        numOfPlayers = GameObject.Find("CurrentNumOfPlayersText").GetComponent<TextMeshProUGUI>();
        timeLeft = GameObject.Find("TimeLeftText").GetComponent<TextMeshProUGUI>();
        characterSelectedName = GameObject.Find("CharacterNameText").GetComponent<TextMeshProUGUI>();
        physicalDmg = GameObject.Find("PhysDmgText").GetComponent<TextMeshProUGUI>();
        magicalDmg = GameObject.Find("MagicDmgText").GetComponent<TextMeshProUGUI>();
        physicalDef = GameObject.Find("PhysDefText").GetComponent<TextMeshProUGUI>();
        magicalDef = GameObject.Find("MagicDefText").GetComponent<TextMeshProUGUI>();
        atkSpd = GameObject.Find("AtkSpdText").GetComponent<TextMeshProUGUI>();

        // Setup default values for default heroes
        SetupDefaultHeroes();

        // Set default currently selected character
        selectedHero = king;
        networkHeroManager.SetModel(selectedHero);

        // Set skill description to inactive by default
        skillDescription.SetActive(false);

        // Update UI elements
        UpdateStats();
        UpdateCharacterSelectedName();
        UpdateHostIP();
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
            selectedHero = rogue;
        }
        else if (selectedHero == wizard)
        {
            selectedHero = king;
        }
        else if (selectedHero == rogue)
        {
            selectedHero = wizard;
        }

        networkHeroManager.SetModel(selectedHero);
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
            selectedHero = king;
        }

        networkHeroManager.SetModel(selectedHero);
        UpdateCharacterSelectedName();
    }

    /// <summary>
    /// Functionality for decreasing physical damage when the corresponding button is pressed.
    /// </summary>
    public void DecreasePhysicalDamage()
    {
        int physDmg = networkHeroManager.GetPAttack();
        int pointsLeft = int.Parse(attributePointsLeft.text);

        if (physDmg > defaultPhyDmg)
        {
            networkHeroManager.SetPAttack(physDmg - 1);                      // decrement physical damage stat
            attributePointsLeft.text = (pointsLeft + 1).ToString(); // increment attribute points left by 1
            UpdateStats();
        }
    }

    /// <summary>
    /// Functionality for increasing physical damage when the corresponding button is pressed.
    /// </summary>
    public void IncreasePhysicalDamage()
    {
        int physDmg = networkHeroManager.GetPAttack();
        int pointsLeft = int.Parse(attributePointsLeft.text);

        if (pointsLeft > 0)
        {
            networkHeroManager.SetPAttack(physDmg + 1);                    // increment physical damage stat
            attributePointsLeft.text = (pointsLeft - 1).ToString(); // decrement attribute points left by 1
            UpdateStats();
        }
    }

    /// <summary>
    /// Functionality for decreasing magical damage when the corresponding button is pressed.
    /// </summary>
    public void DecreaseMagicalDamage()
    {
        int magicDmg = networkHeroManager.GetMAttack();
        int pointsLeft = int.Parse(attributePointsLeft.text);

        if (magicDmg > defaultMagDmg)
        {
            networkHeroManager.SetMAttack(magicDmg - 1);
            attributePointsLeft.text = (pointsLeft + 1).ToString();
            UpdateStats();
        }
    }

    /// <summary>
    /// Functionality for increasing magical damage when the corresponding button is pressed.
    /// </summary>
    public void IncreaseMagicalDamage()
    {
        int magicDmg = networkHeroManager.GetMAttack();
        int pointsLeft = int.Parse(attributePointsLeft.text);

        if (pointsLeft > 0)
        {
            networkHeroManager.SetMAttack(magicDmg + 1);
            attributePointsLeft.text = (pointsLeft - 1).ToString();
            UpdateStats();
        }
    }

    /// <summary>
    /// Functionality for decreasing physical defence when the corresponding button is pressed.
    /// </summary>
    public void DecreasePhysicalDefence()
    {
        int physDef = networkHeroManager.GetPDefence();
        int pointsLeft = int.Parse(attributePointsLeft.text);

        if (physDef > defaultPhyDef)
        {
            networkHeroManager.SetPDefence(physDef - 1);
            attributePointsLeft.text = (pointsLeft + 1).ToString();
            UpdateStats();
        }
    }

    /// <summary>
    /// Functionality for increasing physical defence when the corresponding button is pressed.
    /// </summary>
    public void IncreasePhysicalDefence()
    {
        int physDef = networkHeroManager.GetPDefence();
        int pointsLeft = int.Parse(attributePointsLeft.text);

        if (pointsLeft > 0)
        {
            networkHeroManager.SetPDefence(physDef + 1);
            attributePointsLeft.text = (pointsLeft - 1).ToString();
            UpdateStats();
        }
    }

    /// <summary>
    /// Functionality for decreasing magical defence when the corresponding button is pressed.
    /// </summary>
    public void DecreaseMagicalDefence()
    {
        int magicDef = networkHeroManager.GetMDefence();
        int pointsLeft = int.Parse(attributePointsLeft.text);

        if (magicDef > defaultMagDef)
        {
            networkHeroManager.SetMDefence(magicDef - 1);
            attributePointsLeft.text = (pointsLeft + 1).ToString();
            UpdateStats();
        }
    }

    /// <summary>
    /// Functionality for increasing magical defence when the corresponding button is pressed.
    /// </summary>
    public void IncreaseMagicalDefence()
    {
        int magicDef = networkHeroManager.GetMDefence();
        int pointsLeft = int.Parse(attributePointsLeft.text);

        if (pointsLeft > 0)
        {
            networkHeroManager.SetMDefence(magicDef + 1);
            attributePointsLeft.text = (pointsLeft - 1).ToString();
            UpdateStats();
        }
    }

    /// <summary>
    /// Functionality for decreasing attack speed when the corresponding button is pressed.
    /// </summary>
    public void DecreaseAttackSpeed()
    {
        int atkSpd = networkHeroManager.GetAtkSpeed();
        int pointsLeft = int.Parse(attributePointsLeft.text);

        if (atkSpd > defaultAtkSpd)
        {
            networkHeroManager.SetAtkSpeed(atkSpd - 1);
            attributePointsLeft.text = (pointsLeft + 1).ToString();
            UpdateStats();
        }
    }

    /// <summary>
    /// Functionality for increasing attack speed when the corresponding button is pressed.
    /// </summary>
    public void IncreaseAttackSpeed()
    {
        int atkSpd = networkHeroManager.GetAtkSpeed();
        int pointsLeft = int.Parse(attributePointsLeft.text);

        if (pointsLeft > 0)
        {
            networkHeroManager.SetAtkSpeed(atkSpd + 1);
            attributePointsLeft.text = (pointsLeft - 1).ToString();
            UpdateStats();
        }
    }

    /// <summary>
    /// Sets up the default hero types of king, wizard, and rogue.
    /// </summary>
    private void SetupDefaultHeroes()
    {
        // Setup default heroes
        king = new Hero
        {
            heroType = HeroType.melee,
            heroName = "King",
            childIndex = 0
        };

        wizard = new Hero
        {
            heroType = HeroType.magic,
            heroName = "Wizard",
            childIndex = 2
        };

        rogue = new Hero
        {
            heroType = HeroType.melee,
            heroName = "Rogue",
            childIndex = 1
        };

        SetupDefaultStats();
    }

    /// <summary>
    /// Sets the default stats of the hero.
    /// </summary>
    private void SetupDefaultStats()
    {
        // Set default stat values
        networkHeroManager.SetAtkSpeed(defaultAtkSpd);
        networkHeroManager.SetPAttack(defaultPhyDmg);
        networkHeroManager.SetMAttack(defaultMagDmg);
        networkHeroManager.SetPDefence(defaultPhyDef);
        networkHeroManager.SetMDefence(defaultMagDef);
        attributePointsLeft.text = defaultAttributePts.ToString();
    }

    /// <summary>
    /// Updates the "Number of the players connected" text in UI.
    /// </summary>
    private void UpdateNumOfPlayers()
    {
        numOfPlayers.text = matchManager.GetNumOfPlayers().ToString();
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
        physicalDmg.text = networkHeroManager.GetPAttack().ToString();
        magicalDmg.text = networkHeroManager.GetMAttack().ToString();
        physicalDef.text = networkHeroManager.GetPDefence().ToString();
        magicalDef.text = networkHeroManager.GetMDefence().ToString();
        atkSpd.text = networkHeroManager.GetAtkSpeed().ToString();
    }

    /// <summary>
    /// Updates the host IP text in UI.
    /// </summary>
    private void UpdateHostIP()
    {
        hostIP.text = NetworkManagerExtension.GetLocalIPAddress();
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
}
