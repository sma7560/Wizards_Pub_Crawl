using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Networking;

/// <summary>
/// Contains all logic regarding enemy AI for movement, player targetting & attacking.
/// </summary>
[RequireComponent(typeof(EnemyModel))]
[RequireComponent(typeof(Animator))]
[NetworkSettings(channel = 0, sendInterval = 0.05f)]
public class EnemyController : NetworkBehaviour
{
    // Managers
    private MatchManager matchManager;
    protected HeroManager heroManager;

    private NavMeshAgent agent;
    protected EnemyModel stats;
    protected Animator animator;

    // Idle movement
    private Vector3 currentRandomLocation;
    private bool isIdleMovement;

    // Attack
    private float attackCooldown;

    /// <summary>
    /// Use this for initialization.
    /// </summary>
    protected virtual void Start()
    {
        matchManager = GameObject.FindGameObjectWithTag("MatchManager").GetComponent<MatchManager>();
        heroManager = GameObject.FindGameObjectWithTag("MatchManager").GetComponent<HeroManager>();
        animator = GetComponent<Animator>();
        stats = GetComponent<EnemyModel>();
        agent = GetComponent<NavMeshAgent>();
        if (agent != null)
        {
            agent.speed = stats.GetMovementSpeed();
        }

        attackCooldown = 0;
    }

    void Update()
    {
        if (!isServer) return;

        // Stop movement if match has ended
        if (matchManager.HasMatchEnded())
        {
            if (agent != null)
            {
                agent.isStopped = true;
            }
            return;
        }

        // If enemy is currently dying, disable agent to prevent it from moving
        if (stats.IsDying())
        {
            agent.speed = 0;
            return;
        }

        // Perform targetting AI
        TargetClosestPlayer();

        attackCooldown -= Time.deltaTime;
    }

    protected float[] getDistanceToClosestPlayer(Transform[] targets)
    {
        float shortestDistance = int.MaxValue;                      // distance to the closest player
        int playerIndex = -1;                                       // index of the player in targets array
        // Get distance of player closest to the enemy
        for (int i = 0; i < targets.Length; i++)
        {
            // Do not target knocked out players
            if (targets[i].GetComponent<HeroModel>().IsKnockedOut()) continue;

            // Find the index of the closest player
            float distance = Vector3.Distance(targets[i].position, transform.position);
            if (distance < shortestDistance)
            {
                shortestDistance = distance;
                playerIndex = i;
            }
        }
        float[] playerAndDistance = { playerIndex, shortestDistance };
        return playerAndDistance;
    }

    /// <summary>
    /// Sets the target of this enemy to the closest player.
    /// </summary>
    protected virtual void TargetClosestPlayer()
    {
        Transform[] targets = heroManager.GetAllPlayerTransforms(); // list of all player transforms

        float[] playerAndDistance = getDistanceToClosestPlayer(targets);

        // If player is within lookRadius of enemy, follow the player
        if (playerAndDistance[1] <= stats.GetLookRadius() && (int)playerAndDistance[0] >= 0)
        {
            // Stop idle movement
            isIdleMovement = false;
            StopCoroutine(RandomNavSphere(transform.position, stats.GetIdleRange(), stats.GetIdleHowOftenDirectionChanged(), -1));

            // Follow player
            agent.SetDestination(targets[(int)playerAndDistance[0]].position);
            FaceTarget(targets[(int)playerAndDistance[0]].position);
            animator.SetBool("isWalking", true);

            // If player is within attacking range, attack the player
            if (playerAndDistance[1] - stats.GetAttackRange() <= agent.stoppingDistance)
            {
                animator.SetBool("isWalking", false);
                Attack(targets[(int)playerAndDistance[0]]);
            }
        }
        else
        {
            IdleMovement();
        }
    }

    /// <summary>
    /// Rotates this enemy to face its current target.
    /// </summary>
    /// <param name="target">Transform of this enemy's current target.</param>
    private void FaceTarget(Vector3 target)
    {
        float rotationSpeed = 5f;
        Vector3 direction = (target - transform.position).normalized;

        if ((direction.x != 0 || direction.z != 0) &&
            !float.IsNaN(direction.x) &&
            !float.IsNaN(direction.z))
        {
            Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * rotationSpeed);
        }
    }

    /// <summary>
    /// Idle movement for enemy.
    /// </summary>
    private void IdleMovement()
    {
        animator.SetBool("isWalking", true);

        // If not currently idle movement, start random location couroutine
        if (!isIdleMovement)
        {
            isIdleMovement = true;
            StartCoroutine(RandomNavSphere(transform.position, stats.GetIdleRange(), stats.GetIdleHowOftenDirectionChanged(), -1));
        }
        agent.SetDestination(currentRandomLocation);
        FaceTarget(currentRandomLocation);

        // Check if we've reached the destination
        if (!agent.pathPending)
        {
            if (agent.remainingDistance <= agent.stoppingDistance)
            {
                if (!agent.hasPath || agent.velocity.sqrMagnitude == 0f)
                {
                    animator.SetBool("isWalking", false);
                }
            }
        }
    }

    /// <summary>
    /// Generate random location near specified navmesh agent.
    /// </summary>
    private IEnumerator RandomNavSphere(Vector3 origin, float distance, float howOften, int layermask)
    {
        while (true)
        {
            Vector3 randomDirection = Random.insideUnitSphere * distance;
            randomDirection += origin;
            NavMeshHit navHit;
            NavMesh.SamplePosition(randomDirection, out navHit, distance, layermask);
            currentRandomLocation = navHit.position;

            yield return new WaitForSeconds(Random.Range(howOften - 1, howOften + 1));
        }
    }

    /// <summary>
    /// Performs an attack on a specified player.
    /// </summary>
    /// <param name="attackedPlayer">Player whom is being attacked by this enemy.</param>
    protected void Attack(Transform attackedPlayer)
    {
        if (!isServer) return;

        HeroModel heroStats = attackedPlayer.GetComponent<HeroModel>();
        if (heroStats != null && attackCooldown <= 0)
        {
            CmdTriggerAttackAnim();
            StartCoroutine(damageDelayForAnimationSync(heroStats));
            attackCooldown = 1f / stats.GetAttackSpeed();
        }
    }

    /// <summary>
    /// Delay for hero taking damage to sync with animation
    /// </summary>
    public IEnumerator damageDelayForAnimationSync(HeroModel heroStats)
    {
        yield return new WaitForSeconds(0.2f);
        heroStats.CmdTakeDamage(stats.GetDamage());
    }

    /// <summary>
    /// Plays the attack animation on the server.
    /// </summary>
    [Command]
    private void CmdTriggerAttackAnim()
    {
        RpcTriggerAttackAnim();
        StartCoroutine(stats.soundDelayForAnimationSync());
    }

    /// <summary>
    /// Server notifies all clients to play this enemy's attack animation.
    /// </summary>
    [ClientRpc]
    private void RpcTriggerAttackAnim()
    {
        animator.SetTrigger("attack");
    }
}
