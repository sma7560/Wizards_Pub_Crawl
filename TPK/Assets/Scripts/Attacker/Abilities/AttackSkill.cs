using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "Skill/AttackSkill")]
public class AttackSkill : Skill {
    public int damage;
    public DamageType damageType;
    public CastType castType;
    public DamageTime damageTime;
}
