using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Note that skill created this way are all active skills, meaning they have to be activated.
[CreateAssetMenu(menuName = "Skill")]
public class Skill : ScriptableObject
{
    public string skillName;
    public string skillDescription;
    public Sprite skillIcon;
    public float skillCoolDown;
    public SkillType skillType;


    //Many of the following attributes act as different descriptors depending on the type of skill cast. 
    // Define how skill is cast.
    public CastType castType;
    public float skillRange; // This doubles as radius

    //Defining Damage Type
    public DamageType damageType;
    public int damageAmount;
    public int projectilePrefabIndex;
    public int numProjectiles;
    public float projectileSpeed;
    public CcType ccType;

    //Define if there is a movement to it.
    // Do we need a direction or always assume forward?
    public MovementType moveType;
    public float movementDistance; //negative distances for backwards and positive for forward.
    //gameobject particle effects.
    public int visualEffectIndex; //These will have to be designed or projectile.
}