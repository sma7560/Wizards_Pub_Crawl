using UnityEngine;

/// <summary>
/// NOTE: Skills created this way are all active skills, meaning they have to be activated.
/// </summary>
[CreateAssetMenu(menuName = "Skill")]
public class Skill : ScriptableObject
{
    public string skillName;
    public string skillDescription;
    public Sprite skillIcon;
    public float skillCoolDown;
    public SkillType skillType;
    public CastType castType;           // Define how skill is cast
    public float skillRange;            // Doubles as radius
    public int damageAmount;
    public int projectilePrefabIndex;
    public int numProjectiles;
    public float projectileSpeed;
    public CcType ccType;
    public MovementType moveType;
    public float movementDistance;      // negative distances for backwards; positive for forward
    public int visualEffectIndex;
}