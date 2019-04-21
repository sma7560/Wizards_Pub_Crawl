using UnityEngine;
using UnityEngine.Networking;

/// <summary>
/// Manages all abilities for the player.
/// </summary>
public class AbilityManager : NetworkBehaviour
{
    public IUnityService unityService;

    public Skill[] equippedSkills;
    public Skill[] knownSkills;
    public float[] nextActiveTime;  // cooldowns
    
    private AbilityCaster caster;
    private PrephaseManager prephaseManager;
    private MatchManager matchManager;
    private DungeonController dungeonController;
	private HeroModel heroModel;
    
    void Start()
    {
        if (!isLocalPlayer) return;

        unityService = new UnityService();

        matchManager = GameObject.FindGameObjectWithTag("MatchManager").GetComponent<MatchManager>();
        prephaseManager = GameObject.FindGameObjectWithTag("MatchManager").GetComponent<PrephaseManager>();
        dungeonController = GameObject.Find("EventSystem").GetComponent<DungeonController>();
		heroModel = GetComponent<HeroModel> ();
        caster = GetComponent<AbilityCaster>();

        // Initialize cooldowns
        nextActiveTime = new float[equippedSkills.Length];
        for (int i = 0; i < nextActiveTime.Length; i++)
        {
            nextActiveTime[i] = 0;
        }
    }
    
    void Update()
    {
		if (!isLocalPlayer || prephaseManager.IsCurrentlyInPrephase() || matchManager.HasMatchEnded() || dungeonController.IsMenuOpen() || heroModel.IsKnockedOut()) return;

        // Listens for casting of skills
        if (unityService.GetKeyDown(CustomKeyBinding.GetSkill1Key()) &&
            equippedSkills[0] != null &&
            nextActiveTime[0] < Time.time)
        {
            CastSkill(0);
            nextActiveTime[0] = Time.time + equippedSkills[0].skillCoolDown;
        }

        if (unityService.GetKeyDown(CustomKeyBinding.GetSkill2Key()) &&
            equippedSkills[1] != null &&
            nextActiveTime[1] < Time.time)
        {
            CastSkill(1);
            nextActiveTime[1] = Time.time + equippedSkills[1].skillCoolDown;
        }

        if (unityService.GetKeyDown(CustomKeyBinding.GetSkill3Key()) &&
            equippedSkills[2] != null &&
            nextActiveTime[2] < Time.time)
        {
            CastSkill(2);
            nextActiveTime[2] = Time.time + equippedSkills[2].skillCoolDown;
        }

        if (unityService.GetKeyDown(CustomKeyBinding.GetSkill4Key()) &&
            equippedSkills[3] != null &&
            nextActiveTime[3] < Time.time)
        {
            CastSkill(3);
            nextActiveTime[3] = Time.time + equippedSkills[3].skillCoolDown;
        }
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
                return;
            }
        }
        
        Debug.Log("Can't equip anymore skills.");
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
    }

    /// <summary>
    /// Casts the equipped skill.
    /// </summary>
    /// <param name="index">Index of the equipped skill.</param>
    private void CastSkill(int index)
    {
        caster.CastSkill(equippedSkills[index]);
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
