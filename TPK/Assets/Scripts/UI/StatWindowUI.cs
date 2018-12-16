using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class StatWindowUI : MonoBehaviour
{
    public GameObject skillDescription;

    private int playerId;
    private NetworkHeroManager networkHeroManager;

    /// <summary>
    /// Setup UI elements when stat window is active.
    /// </summary>
    void Awake()
    {
        // Get SkillDescription object
        skillDescription = GameObject.FindGameObjectWithTag("SkillDescription");

        // Get playerId
        MatchManager matchManager = GameObject.FindGameObjectWithTag("MatchManager").GetComponent<MatchManager>();
        playerId = matchManager.GetPlayerId();

        // Get networkHeroManager for hero stats
        HeroManager heroManager = GameObject.FindGameObjectWithTag("MatchManager").GetComponent<HeroManager>();
        networkHeroManager = heroManager.GetHeroObject(playerId).GetComponent<NetworkHeroManager>();

        // Set UI elements
        skillDescription.SetActive(false);  // set to inactive by default
        SetupStats();
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
}
