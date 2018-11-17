using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Skill/Passive")]
public class PassiveSkill : Skill {
    public StatModifier[] statsToModify;

    // Need to figure out how modifiers will be added in.
    public void ApplyModifiers() {

    }

    public void RemoverModifiers()
    {
    }

}
