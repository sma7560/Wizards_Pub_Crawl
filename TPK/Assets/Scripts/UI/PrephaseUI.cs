using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Updates elements in the pre-phase screen.
/// Attached to the PrephaseScreen prefab.
/// </summary>
public class PrephaseUI : MonoBehaviour
{
    // Skill description
    [System.NonSerialized] public GameObject skillDescription;
    [System.NonSerialized] public TextMeshProUGUI skillDescriptionText;
    [System.NonSerialized] public TextMeshProUGUI skillTitleText;
	[System.NonSerialized] public TextMeshProUGUI skillDamageText;
	[System.NonSerialized] public TextMeshProUGUI skillCDText;

    // Managers and current player's hero data
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
    private TextMeshProUGUI health;
	private TextMeshProUGUI attackBoost;
	private TextMeshProUGUI defBoost;

    // Default heroes
    public Hero king;
    public Hero wizard;
    public Hero rogue;
    public Hero armored;

    // Stat graphs
    private Image statGraph;
    private Sprite kingStat;
    private Sprite wizardStat;
    private Sprite rogueStat;
    private Sprite knightStat;

    // Currently selected hero
    private Hero selectedHero;

    /// <summary>
    /// Initialize variables.
    /// </summary>
    void Start()
    {
        // Get skill description elements
        skillDescription = GameObject.FindGameObjectWithTag("SkillDescription");
        skillDescriptionText = GameObject.Find("SkillDescriptionText").GetComponent<TextMeshProUGUI>();
		skillDamageText = GameObject.Find("SkillDamageText").GetComponent<TextMeshProUGUI>();
		skillCDText = GameObject.Find("SkillCDText").GetComponent<TextMeshProUGUI>();
        skillTitleText = GameObject.Find("SkillDescriptionTitleText").GetComponent<TextMeshProUGUI>();
        heroManager = GameObject.FindGameObjectWithTag("MatchManager").GetComponent<HeroManager>();

        // Get managers and player's hero data
        prephaseManager = GameObject.FindGameObjectWithTag("MatchManager").GetComponent<PrephaseManager>();
        matchManager = GameObject.FindGameObjectWithTag("MatchManager").GetComponent<MatchManager>();
        heroModel = heroManager.GetHeroObject(matchManager.GetPlayerId()).GetComponent<HeroModel>();

        // Get text elements
        playerName = GameObject.Find("PlayerNameText").GetComponent<TextMeshProUGUI>();
        numOfPlayers = GameObject.Find("CurrentNumOfPlayersConnectedText").GetComponent<TextMeshProUGUI>();
        timeLeft = GameObject.Find("TimeLeftText").GetComponent<TextMeshProUGUI>();
        characterSelectedName = GameObject.Find("CharacterNameText").GetComponent<TextMeshProUGUI>();
        attack = GameObject.Find("AttackText").GetComponent<TextMeshProUGUI>();
        defense = GameObject.Find("DefText").GetComponent<TextMeshProUGUI>();
        moveSpeed = GameObject.Find("SpeedText").GetComponent<TextMeshProUGUI>();
        health = GameObject.Find("HealthStatGraphText").GetComponent<TextMeshProUGUI>();
		attackBoost = GameObject.Find("AttackBoostText").GetComponent<TextMeshProUGUI>();
		defBoost = GameObject.Find("DefBoostText").GetComponent<TextMeshProUGUI>();

        // Initialize stat graphs
        statGraph = GameObject.Find("StatGraph").GetComponent<Image>();
        kingStat = Resources.Load<Sprite>("UI Resources/stat graphs/king stat");
        wizardStat = Resources.Load<Sprite>("UI Resources/stat graphs/classic stat");
        rogueStat = Resources.Load<Sprite>("UI Resources/stat graphs/sneaky stat");
        knightStat = Resources.Load<Sprite>("UI Resources/stat graphs/tank stat");

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
		UpdateStats ();
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
    /// Functionality for changing character selected when the left button is pressed.
    /// Order is king < wizard < rogue < armoured.
    /// </summary>
    public void ChangeCharacterSelectionLeft()
    {
        if (selectedHero == king)
        {
            selectedHero = armored;
            statGraph.sprite = knightStat;
        }
        else if (selectedHero == wizard)
        {
            selectedHero = king;
            statGraph.sprite = kingStat;
        }
        else if (selectedHero == rogue)
        {
            selectedHero = wizard;
            statGraph.sprite = wizardStat;
        }
        else if (selectedHero == armored)
        {
            selectedHero = rogue;
            statGraph.sprite = rogueStat;
        }

        SetupDefaultStats();
        heroModel.SetModel(selectedHero);
        UpdateCharacterSelectedName();
		UpdateStats();
    }

    /// <summary>
    /// Functionality for changing character selected when the right button is pressed.
    /// Order is king > wizard > rogue > armoured.
    /// </summary>
    public void ChangeCharacterSelectionRight()
    {
        if (selectedHero == king)
        {
            selectedHero = wizard;
            statGraph.sprite = wizardStat;
        }
        else if (selectedHero == wizard)
        {
            selectedHero = rogue;
            statGraph.sprite = rogueStat;
        }
        else if (selectedHero == rogue)
        {
            selectedHero = armored;
            statGraph.sprite = knightStat;
        }
        else if (selectedHero == armored)
        {
            selectedHero = king;
            statGraph.sprite = kingStat;
        }

        SetupDefaultStats();
        heroModel.SetModel(selectedHero);
        UpdateCharacterSelectedName();
		UpdateStats();
    }

    /// <summary>
    /// Returns the default hero based on its child index.
    /// </summary>
    /// <param name="index">Child index of the desired hero.</param>
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
                Debug.Log("ERROR: invalid child index!");
                return null;
        }
    }

    /// <summary>
    /// Sets the player's hero stats based on their selected hero type.
    /// </summary>
    private void SetupDefaultStats()
    {
        if (selectedHero == king)
        {
            heroModel.SetBaseAttack(11);
            heroModel.SetBaseDefense(11);
            heroModel.SetBaseMoveSpeed(11);
            heroModel.SetMaxHealth(111);
        }
        else if (selectedHero == wizard)
        {
            heroModel.SetBaseAttack(20);
            heroModel.SetBaseDefense(5);
            heroModel.SetBaseMoveSpeed(10);
            heroModel.SetMaxHealth(80);
        }
        else if (selectedHero == rogue)
        {
            heroModel.SetBaseAttack(8);
            heroModel.SetBaseDefense(0);
            heroModel.SetBaseMoveSpeed(12);
            heroModel.SetMaxHealth(50);
        }
        else if (selectedHero == armored)
        {
            heroModel.SetBaseAttack(12);
            heroModel.SetBaseDefense(18);
            heroModel.SetBaseMoveSpeed(8);
            heroModel.SetMaxHealth(150);
        }
        else
        {
            Debug.Log("ERROR: selected hero is not a valid hero!");
        }

        heroModel.SetFullHealth();

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
        attack.text = heroModel.GetCurrentAttack().ToString();
        defense.text = heroModel.GetCurrentDefense().ToString();
        moveSpeed.text = heroModel.GetCurrentMoveSpeed().ToString();
        health.text = heroModel.GetMaxHealth().ToString();

		attackBoost.text = "+" + heroModel.GetCurrentAttack () * 2 + "% DMG";
		defBoost.text = "-" + heroModel.GetCurrentDefense () * 2 + "% DMG";
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
        // NOTE: assuming max of 10 skills at the moment
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
