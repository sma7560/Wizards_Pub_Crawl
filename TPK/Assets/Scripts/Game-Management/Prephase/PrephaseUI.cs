using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PrephaseUI : MonoBehaviour
{
    // Skill bank description
    public GameObject skillDescription;

    // Managers
    private PrephaseManager prephaseManager;
    private NetworkHeroManager heroManager;

    // Text elements
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
    private Button leftCharacterSelection;
    private Button rightCharacterSelection;

    // Default heroes
    private Hero knight;
    private Hero witch;
    private Hero rogue;

    // Currently selected hero
    private Hero selectedHero;

    // Currently equipped skills; TODO: change this later when Skills are finished
    public bool skill1;
    public bool skill2;
    public bool skill3;
    public bool skill4;

    /// <summary>
    /// Initialize variables.
    /// </summary>
    void Start()
    {
        prephaseManager = GameObject.Find("MatchManager(Clone)").GetComponent<PrephaseManager>();
        heroManager = GameObject.Find("Hero(Clone)").GetComponent<NetworkHeroManager>();    // TODO: update this to appropriate current player and not just "Hero(Clone)"
        skillDescription = GameObject.Find("SkillBankDescription");

        // Initialize text
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

        // Initialize buttons
        leftCharacterSelection = GameObject.Find("CharSelectLeftButton").GetComponent<Button>();
        rightCharacterSelection = GameObject.Find("CharSelectRightButton").GetComponent<Button>();

        // Setup default values for default heroes
        knight = new Hero
        {
            heroType = HeroType.melee,
            heroName = "Knight",
            childIndex = 0
        };

        witch = new Hero
        {
            heroType = HeroType.magic,
            heroName = "Witch",
            childIndex = 1
        };

        rogue = new Hero
        {
            heroType = HeroType.melee,
            heroName = "Rogue",
            childIndex = 2
        };

        // Set default currently selected character
        selectedHero = knight;
        heroManager.SetModel(selectedHero);

        // Update UI elements
        UpdateStats();
        UpdateCharacterSelectedName();
        UpdateHostIP();
        UpdateNumOfPlayers();

        skillDescription.SetActive(false);

        skill1 = false;
        skill2 = false;
        skill3 = false;
        skill4 = false;
    }

    void Update()
    {
        UpdateNumOfPlayers();
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
    }

    /// <summary>
    /// Updates the "Number of the players connected" text in UI.
    /// </summary>
    public void UpdateNumOfPlayers()
    {
        MatchManager matchManager = GameObject.Find("MatchManager(Clone)").GetComponent<MatchManager>();
        numOfPlayers.text = matchManager.GetNumOfPlayers().ToString();
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
        physicalDmg.text = heroManager.GetPAttack().ToString();
        magicalDmg.text = heroManager.GetMAttack().ToString();
        physicalDef.text = heroManager.GetPDefence().ToString();
        magicalDef.text = heroManager.GetMDefence().ToString();
        atkSpd.text = heroManager.GetAtkSpeed().ToString();
    }

    /// <summary>
    /// Updates the host IP text in UI.
    /// </summary>
    private void UpdateHostIP()
    {
        NetworkManagerExtension networkManagerExtension = GameObject.Find("NetworkManagerV2").GetComponent<NetworkManagerExtension>();
        hostIP.text = networkManagerExtension.networkAddress;
    }

    /// <summary>
    /// Functionality for changing character selected when the corresponding button is pressed.
    /// Order is knight < witch < rogue.
    /// </summary>
    public void ChangeCharacterSelectionLeft()
    {
        if (selectedHero == knight)
        {
            selectedHero = rogue;
        }
        else if (selectedHero == witch)
        {
            selectedHero = knight;
        }
        else if (selectedHero == rogue)
        {
            selectedHero = witch;
        }

        heroManager.SetModel(selectedHero);
        UpdateCharacterSelectedName();
    }

    /// <summary>
    /// Functionality for changing character selected when the corresponding button is pressed.
    /// Order is knight > witch > rogue.
    /// </summary>
    public void ChangeCharacterSelectionRight()
    {
        if (selectedHero == knight)
        {
            selectedHero = witch;
        }
        else if (selectedHero == witch)
        {
            selectedHero = rogue;
        }
        else if (selectedHero == rogue)
        {
            selectedHero = knight;
        }

        heroManager.SetModel(selectedHero);
        UpdateCharacterSelectedName();
    }

    /// <summary>
    /// Functionality for decreasing physical damage when the corresponding button is pressed.
    /// </summary>
    public void DecreasePhysicalDamage()
    {
        int physDmg = heroManager.GetPAttack();
        int pointsLeft = int.Parse(attributePointsLeft.text);
        
        if (physDmg > 0)
        {
            heroManager.SetPAttack(physDmg-1);                      // decrement physical damage stat
            attributePointsLeft.text = (pointsLeft + 1).ToString(); // increment attribute points left by 1
            UpdateStats();
        }
    }

    /// <summary>
    /// Functionality for increasing physical damage when the corresponding button is pressed.
    /// </summary>
    public void IncreasePhysicalDamage()
    {
        int physDmg = heroManager.GetPAttack();
        int pointsLeft = int.Parse(attributePointsLeft.text);

        if (pointsLeft > 0)
        {
            heroManager.SetPAttack(physDmg + 1);                    // increment physical damage stat
            attributePointsLeft.text = (pointsLeft - 1).ToString(); // decrement attribute points left by 1
            UpdateStats();
        }
    }

    /// <summary>
    /// Functionality for decreasing magical damage when the corresponding button is pressed.
    /// </summary>
    public void DecreaseMagicalDamage()
    {
        int magicDmg = heroManager.GetMAttack();
        int pointsLeft = int.Parse(attributePointsLeft.text);

        if (magicDmg > 0)
        {
            heroManager.SetMAttack(magicDmg - 1);
            attributePointsLeft.text = (pointsLeft + 1).ToString();
            UpdateStats();
        }
    }

    /// <summary>
    /// Functionality for increasing magical damage when the corresponding button is pressed.
    /// </summary>
    public void IncreaseMagicalDamage()
    {
        int magicDmg = heroManager.GetMAttack();
        int pointsLeft = int.Parse(attributePointsLeft.text);

        if (pointsLeft > 0)
        {
            heroManager.SetMAttack(magicDmg + 1);
            attributePointsLeft.text = (pointsLeft - 1).ToString();
            UpdateStats();
        }
    }

    /// <summary>
    /// Functionality for decreasing physical defence when the corresponding button is pressed.
    /// </summary>
    public void DecreasePhysicalDefence()
    {
        int physDef = heroManager.GetPDefence();
        int pointsLeft = int.Parse(attributePointsLeft.text);

        if (physDef > 0)
        {
            heroManager.SetPDefence(physDef - 1);
            attributePointsLeft.text = (pointsLeft + 1).ToString();
            UpdateStats();
        }
    }

    /// <summary>
    /// Functionality for increasing physical defence when the corresponding button is pressed.
    /// </summary>
    public void IncreasePhysicalDefence()
    {
        int physDef = heroManager.GetPDefence();
        int pointsLeft = int.Parse(attributePointsLeft.text);

        if (pointsLeft > 0)
        {
            heroManager.SetPDefence(physDef + 1);
            attributePointsLeft.text = (pointsLeft - 1).ToString();
            UpdateStats();
        }
    }

    /// <summary>
    /// Functionality for decreasing magical defence when the corresponding button is pressed.
    /// </summary>
    public void DecreaseMagicalDefence()
    {
        int magicDef = heroManager.GetMDefence();
        int pointsLeft = int.Parse(attributePointsLeft.text);

        if (magicDef > 0)
        {
            heroManager.SetMDefence(magicDef - 1);
            attributePointsLeft.text = (pointsLeft + 1).ToString();
            UpdateStats();
        }
    }

    /// <summary>
    /// Functionality for increasing magical defence when the corresponding button is pressed.
    /// </summary>
    public void IncreaseMagicalDefence()
    {
        int magicDef = heroManager.GetMDefence();
        int pointsLeft = int.Parse(attributePointsLeft.text);

        if (pointsLeft > 0)
        {
            heroManager.SetMDefence(magicDef + 1);
            attributePointsLeft.text = (pointsLeft - 1).ToString();
            UpdateStats();
        }
    }

    /// <summary>
    /// Functionality for decreasing attack speed when the corresponding button is pressed.
    /// </summary>
    public void DecreaseAttackSpeed()
    {
        int atkSpd = heroManager.GetAtkSpeed();
        int pointsLeft = int.Parse(attributePointsLeft.text);

        if (atkSpd > 0)
        {
            heroManager.SetAtkSpeed(atkSpd - 1);
            attributePointsLeft.text = (pointsLeft + 1).ToString();
            UpdateStats();
        }
    }

    /// <summary>
    /// Functionality for increasing attack speed when the corresponding button is pressed.
    /// </summary>
    public void IncreaseAttackSpeed()
    {
        int atkSpd = heroManager.GetAtkSpeed();
        int pointsLeft = int.Parse(attributePointsLeft.text);

        if (pointsLeft > 0)
        {
            heroManager.SetAtkSpeed(atkSpd + 1);
            attributePointsLeft.text = (pointsLeft - 1).ToString();
            UpdateStats();
        }
    }

}
