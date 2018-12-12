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
    public int skillCost; // not sure if this will be used...
    public SkillType skillType;

    public Transform playerOrigin; //This is for getting the relative transform information
    public Rigidbody playerRigidBody;


    //Many of the following attributes act as different descriptors depending on the type of skill cast. 
    // Define how skill is cast.
    public CastType castType;
    //Define the time to cast the ability
    public int castTime;
    public float skillRange; // This doubles as radius

    //Defining Damage Type
    public DamageTime dTimeType;
    public DamageType damageType;
    public int damageAmount;

    //Define if there is a movement to it.
    // Do we need a direction or always assume forward?
    public MovementType moveType;
    public float movementDistance; //negative distances for backwards and positive for forward.
    //gameobject particle effects.
    public GameObject visualEffect; //These will have to be designed or projectile.
}