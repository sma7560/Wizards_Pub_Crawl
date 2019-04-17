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
    private bool isIdle = false;

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
            else
            {
                if(!isIdle)
                {
                    float randomWait = UnityEngine.Random.Range(2f, 4f);
                    float randomDuration = UnityEngine.Random.Range(2f, stats.GetIdleHowOftenDirectionChanged());
                    StartCoroutine(idleTurn(randomWait, randomDuration));
                }
                animator.SetBool("isWalking", false);
            }

        }
    }

    private IEnumerator idleTurn(float wait, float duration)
    {
        isIdle = true;
        float randomDirection = UnityEngine.Random.Range(0, 1);
        float randomRotate = UnityEngine.Random.Range(0, 100);
        yield return new WaitForSeconds(wait);
        //rotate random direction
        if(randomDirection>0)
        {
            transform.rotation = Quaternion.Euler(0, randomRotate, 0);
        }
        else
        {
            transform.rotation = Quaternion.Euler(0, -randomRotate, 0);
        }
        yield return new WaitForSeconds(duration);
        isIdle = false;
    }
}
