using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//This script is for casting skills based on their description from the designated Skill Scriptable object Asset.
public class AbilityCaster : MonoBehaviour {
    Skill currentCastSkill;
    // Probably will need some network synching thing here to command server to do stuff. Maybe Call it server interface.


   // To do the inital cast of the skill.
   // Order Of events: 1. Determine how to cast and what to affect. 2.Determine if there is movement included. 3. play the visuals associated.
    public void CastSkill(Skill skillToCast) {
        currentCastSkill = skillToCast;
        switch (currentCastSkill.castType) {
            case CastType.selfAoe:
                CastSelfAOE();
                break;
            case CastType.projectile:
                CastProjectile();
                break;
            case CastType.raycast:
                CastRayCast();
                break;
            case CastType.self:
                CastSelf();
                break;
        }

        switch (currentCastSkill.moveType)
        {
            case MovementType.dash:
                MoveTeleport();
                break;
            case MovementType.teleport:
                MoveDash();
                break;
        }
        PlayEffect();

    }
    public void CastSelfAOE() {

    }

    public void CastSelf() {

    }

    public void CastProjectile() {

    }

    public void CastRayCast() {

    }
    public void MoveTeleport() {

    }

    //
    public void MoveDash() {
    }

    // To go over and play the particle effect.
    public void PlayEffect() {

    }
}
