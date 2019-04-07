using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

// This script will have to be reworked.
public class AbilityManager : NetworkBehaviour
{
    public Skill[] equippedSkills;
    public float[] nextActiveTime;  // Keep tracks of cooldowns.
    public Skill[] knownSkills;     // This should probably be a list.

    //private Transform origin;
    //private Rigidbody playerRigid;
    //private int currMarker;
    private AbilityCaster caster;
    private PrephaseManager prephaseManager;
    private MatchManager matchManager;

    // Use this for initialization
    void Start()
    {
        if (!isLocalPlayer) return;

        matchManager = GameObject.FindGameObjectWithTag("MatchManager").GetComponent<MatchManager>();
        prephaseManager = GameObject.FindGameObjectWithTag("MatchManager").GetComponent<PrephaseManager>();

        caster = GetComponent<AbilityCaster>();
        nextActiveTime = new float[equippedSkills.Length];
        for (int i = 0; i < equippedSkills.Length; i++)
        {
            //if (equippedSkills[i] != null)
            //equippedSkills[i].Initialize(origin, playerRigid);
            nextActiveTime[i] = 0;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!isLocalPlayer || prephaseManager.IsCurrentlyInPrephase() || matchManager.HasMatchEnded()) return;

        if (Input.GetKeyDown(CustomKeyBinding.GetSkill1Key()) && equippedSkills[0] != null && nextActiveTime[0] < Time.time)
        {
            CastSkill(0);
            nextActiveTime[0] = Time.time + equippedSkills[0].skillCoolDown;
        }

        if (Input.GetKeyDown(CustomKeyBinding.GetSkill2Key()) && equippedSkills[1] != null && nextActiveTime[1] < Time.time)
        {
            CastSkill(1);
            nextActiveTime[1] = Time.time + equippedSkills[1].skillCoolDown;
        }

        if (Input.GetKeyDown(CustomKeyBinding.GetSkill3Key()) && equippedSkills[2] != null && nextActiveTime[2] < Time.time)
        {
            CastSkill(2);
            nextActiveTime[2] = Time.time + equippedSkills[2].skillCoolDown;
        }

        if (Input.GetKeyDown(CustomKeyBinding.GetSkill4Key()) && equippedSkills[3] != null && nextActiveTime[3] < Time.time)
        {
            CastSkill(3);
            nextActiveTime[3] = Time.time + equippedSkills[3].skillCoolDown;
        }

    }

    // Removes the skill at the denoted equipIndex and replaces it with the skill denoted by the knownSkill index number
    public void SwapSkills(int equipIndex, int knownIndex)
    {
        equippedSkills[equipIndex] = knownSkills[knownIndex];
    }

    /// <summary>
    /// Equips the specified skill into the first available slot.
    /// </summary>
    /// <param name="skill">Skill to be equipped.</param>
    public void EquipSkill(Skill skill)
    {
        for (int i = 0; i < equippedSkills.Length; i++)
        {
            if (equippedSkills[i] == null && !IsEquipped(skill))
            {
                equippedSkills[i] = skill;
                Debug.Log("Skill " + equippedSkills[i].skillName + " has been equipped.");
                return;
            }
        }

        //Probably do things here to make it better for the players.
        Debug.Log("Can't equip anymore skills");
    }

    /// <summary>
    /// Equips the specified skill into the specified slot.
    /// </summary>
    /// <param name="skill">Skill to be equipped.</param>
    /// <param name="index">Index of slot to equip the skill into.</param>
    public void EquipSkill(Skill skill, int index)
    {
        if (index > equippedSkills.Length - 1 || index < 0)
        {
            Debug.Log("Failed attempt to equip skill in invalid slot; attempted at slot index " + index);
        }

        if (IsEquipped(skill))
        {
            Debug.Log(skill.name + " has already been equipped.");
            return;
        }

        equippedSkills[index] = skill;
        Debug.Log("Skill " + equippedSkills[index].skillName + " has been equipped into slot " + index);
    }

    /// <returns>
    /// Returns the number of skills currently equipped.
    /// </returns>
    public int GetNumOfSkillsEquipped()
    {
        for (int i = 0; i < equippedSkills.Length; i++)
        {
            if (equippedSkills[i] == null)
            {
                return i;
            }
        }

        return equippedSkills.Length;
    }

    public void CastSkill(int index)
    {
        Debug.Log("Casting skill " + equippedSkills[index].skillName);
        caster.CastSkill(equippedSkills[index]);
        //equippedSkills[index].Cast();
    }

    /// <summary>
    /// Unequip the specified skill.
    /// </summary>
    /// <param name="skill">Skill to unequip.</param>
    public void UnequipSkill(Skill skill)
    {
        for (int i = 0; i < equippedSkills.Length; i++)
        {
            if (equippedSkills[i] != null && equippedSkills[i] == skill)
            {
                equippedSkills[i] = null;
                Debug.Log("Skill " + skill.skillName + " has been unequipped.");
                return;
            }
        }
    }

    /// <summary>
    /// Returns whether or not the specified skill is already equipped.
    /// </summary>
    /// <param name="skill">Skill to check if equipped.</param>
    /// <returns>True if specified skill is already equipped, else returns false.</returns>
    public bool IsEquipped(Skill skill)
    {
        for (int i = 0; i < equippedSkills.Length; i++)
        {
            if (equippedSkills[i] == skill)
            {
                return true;
            }
        }

        return false;
    }
}
