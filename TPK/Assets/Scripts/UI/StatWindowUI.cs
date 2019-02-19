using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StatWindowUI : MonoBehaviour
{
    public GameObject skillDescription;
    public TextMeshProUGUI skillDescriptionText;
    public TextMeshProUGUI skillDescriptionTitleText;

    private int playerId;
    private NetworkHeroManager networkHeroManager;
    private HeroManager heroManager;

    /// <summary>
    /// Setup UI elements when stat window is active.
    /// </summary>
    void Awake()
    {
        // Get playerId
        MatchManager matchManager = GameObject.FindGameObjectWithTag("MatchManager").GetComponent<MatchManager>();
        playerId = matchManager.GetPlayerId();

        // Get networkHeroManager for hero stats
        heroManager = GameObject.FindGameObjectWithTag("MatchManager").GetComponent<HeroManager>();
        networkHeroManager = heroManager.GetHeroObject(playerId).GetComponent<NetworkHeroManager>();

        // Set UI elements
        skillDescription.SetActive(false);  // set to inactive by default
        SetupSkills();
        SetupStats();
        SetupAvatar();
    }

    /// <summary>
    /// Setup the skill UI elements to match player's equipped skills.
    /// </summary>
    private void SetupSkills()
    {
        AbilityManager abilityManager = heroManager.GetHeroObject(playerId).GetComponent<AbilityManager>();

        for (int i = 0; i < abilityManager.equippedSkills.Length; i++)
        {
            if (abilityManager.equippedSkills[i] != null)
            {
                GameObject skill = GameObject.Find("StatWindowSkill" + (i + 1));
                Image skillImg = skill.GetComponent<Image>();
                skillImg.sprite = abilityManager.equippedSkills[i].skillIcon;   // set sprite image
                skill.GetComponent<SkillHoverDescription>().skill = abilityManager.equippedSkills[i];   // Set skill description
            }
        }
    }

    /// <summary>
    /// Setup the stat UI elements to match player's actual stats.
    /// </summary>
    private void SetupStats()
    {
        // Get text elements
        TextMeshProUGUI physDmg = GameObject.Find("PhysDmgText").GetComponent<TextMeshProUGUI>();
        TextMeshProUGUI magicDmg = GameObject.Find("MagicDmgText").GetComponent<TextMeshProUGUI>();
        TextMeshProUGUI physDef = GameObject.Find("PhysDefText").GetComponent<TextMeshProUGUI>();
        TextMeshProUGUI magicDef = GameObject.Find("MagicDefText").GetComponent<TextMeshProUGUI>();
        TextMeshProUGUI atkSpd = GameObject.Find("AtkSpeedText").GetComponent<TextMeshProUGUI>();

        // Set text elements to appropriate stats
        physDmg.text = networkHeroManager.GetPAttack().ToString();
        magicDmg.text = networkHeroManager.GetMAttack().ToString();
        physDef.text = networkHeroManager.GetPDefence().ToString();
        magicDef.text = networkHeroManager.GetMDefence().ToString();
        atkSpd.text = networkHeroManager.GetAtkSpeed().ToString();
    }

    /// <summary>
    /// Setup the hero avatar in the stat window.
    /// </summary>
    private void SetupAvatar()
    {
        // Initialize variables
        HeroManager heroManager = GameObject.FindGameObjectWithTag("MatchManager").GetComponent<HeroManager>();
        MatchManager matchManager = GameObject.FindGameObjectWithTag("MatchManager").GetComponent<MatchManager>();
        GameObject heroAvatar = heroManager.GetHeroObject(playerId);            // avatar of the player's hero
        GameObject windowAvatar = GameObject.Find("HeroAvatar");                // empty avatar in the stat window
        GameObject heroAvatarCameraObj = GameObject.Find("HeroAvatarCamera");   // camera which will be focused on the hero avatar

        // Setup camera
        Camera heroAvatarCamera = heroAvatarCameraObj.GetComponent<Camera>();
        heroAvatarCamera.transform.position = heroAvatar.transform.position + new Vector3(0.1f, 1.3f, 2f);
        heroAvatarCamera.transform.rotation = Quaternion.Euler(10, 180, 0);

        // Set the stat window avatar to hero avatar
        int activeChildIndex = GetActiveChildIndex(heroAvatar); // Get the active child inside player's hero object (represents the hero avatar)
        heroAvatar = Instantiate(heroAvatar.transform.GetChild(activeChildIndex).gameObject);   // Get the hero avatar object
        heroAvatar.transform.position = heroManager.GetSpawnLocationOfPlayer(matchManager.GetPlayerId());   // Set window avatar to the player's spawn location
        heroAvatar.transform.parent = windowAvatar.transform;   // set the hero avatar to child of stat window avatar
        SetLayerRecursively(heroAvatar, 9);     // Set to StatWindowAvatar layer
    }

    /// <summary>
    /// Sets GameObject and all its children layer to the specified layer.
    /// </summary>
    /// <param name="obj">GameObject to set layer to.</param>
    /// <param name="layer">Layer to be set.</param>
    private void SetLayerRecursively(GameObject obj, int layer)
    {
        obj.layer = layer;

        for (int i = 0; i < obj.transform.childCount; i++)
        {
            GameObject child = obj.transform.GetChild(i).gameObject;
            SetLayerRecursively(child, layer);
        }
    }

    private int GetActiveChildIndex(GameObject parent)
    {
        for (int i = 0; i < parent.transform.childCount; i++)
        {
            if (parent.transform.GetChild(i).gameObject.activeSelf)
            {
                return i;
            }
        }

        return -1;
    }
}
