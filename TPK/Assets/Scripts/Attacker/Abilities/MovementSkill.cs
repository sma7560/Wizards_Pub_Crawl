using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Skill/MovementSkill")]
public class MovementSkill : Skill {
    public float range;
    public MovementType movementType;

    public override void Cast()
    {
        base.Cast();
        //it is broken if nothing happens.
        if (playerOrigin == null || playerRigidBody == null) return;
        switch (movementType) {
            case MovementType.dash:
                Debug.Log("Casting Dash");
                playerRigidBody.velocity = playerOrigin.forward * range;
                //origin.Translate(origin.forward * range);
                break;

            case MovementType.teleport:
                playerOrigin.position = playerOrigin.position + (playerOrigin.forward * range);
                //Have code here to play visual effects.
                break;
        }

    }
}
