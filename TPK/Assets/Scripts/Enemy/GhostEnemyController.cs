using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Contains all logic regarding enemy AI for ghost (fast) monsters for 
/// movement, player targetting & attacking.
/// </summary>
public class GhostEnemyController : EnemyController
{

    private float moveSpeed = 0;

    protected override void Start()
    {
        base.Start();
        moveSpeed = stats.GetMovementSpeed();
    }

    //ghost specific algorithm for chasing/attacking players
    protected override void TargetClosestPlayer()
    {
        Transform[] targets = heroManager.GetAllPlayerTransforms(); // list of all player transforms

        float[] playerAndDistance = getDistanceToClosestPlayer(targets);

        // If player is within lookRadius of enemy, follow the player
        if ((int)playerAndDistance[0] >= 0)
        {
            // If player is within attacking range, attack the player
            if (playerAndDistance[1] <= stats.GetAttackRange() * 4)
            {
                animator.SetBool("isWalking", false);
                Attack(targets[(int)playerAndDistance[0]]);
            }
            else if (playerAndDistance[1] <= stats.GetLookRadius())
            {
                // Follow player
                animator.SetBool("isWalking", true);
                transform.LookAt(targets[(int)playerAndDistance[0]].transform);
                transform.position += transform.forward * moveSpeed * Time.deltaTime;
            }

        }
    }
}
