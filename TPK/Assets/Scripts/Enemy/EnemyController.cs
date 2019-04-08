using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Networking;

/// <summary>
/// Contains all logic regarding enemy AI for movement, player targetting & attacking.
/// </summary>
[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(EnemyModel))]
[RequireComponent(typeof(Animator))]
public class EnemyController : NetworkBehaviour
{
    public IUnityService unityService;

    // Managers
    private MatchManager matchManager;
    private HeroManager heroManager;

    private NavMeshAgent agent;
    private EnemyModel stats;
    private Animator animator;

    // Idle movement
    private Vector3 currentRandomLocation;
    private bool isIdleMovement;

    // Attack
    private float attackCooldown;

    /// <summary>
    /// Use this for initialization.
    /// </summary>
    void Start()
    {
        if (unityService == null)
        {
            unityService = new UnityService();
        }

        matchManager = GameObject.FindGameObjectWithTag("MatchManager").GetComponent<MatchManager>();
        heroManager = GameObject.FindGameObjectWithTag("MatchManager").GetComponent<HeroManager>();

        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        stats = GetComponent<EnemyModel>();

        attackCooldown = 0;
        agent.speed = stats.GetMovementSpeed();
    }

    void Update()
    {
        if (!isServer) return;

        // Stop movement if match has ended
        if (matchManager.HasMatchEnded())
        {
            agent.isStopped = true;
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

    /// <summary>
    /// Sets the target of this enemy to the closest player.
    /// </summary>
    private void TargetClosestPlayer()
    {
        Transform[] targets = heroManager.GetAllPlayerTransforms(); // list of all player transforms
        float shortestDistance = int.MaxValue;                      // distance to the closest player
        int playerIndex = -1;                                       // index of the player in targets array whom is currently targetted

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

        // If player is within lookRadius of enemy, follow the player
        if (shortestDistance <= stats.GetLookRadius() && playerIndex >= 0)
        {
            // Stop idle movement
            isIdleMovement = false;
            StopCoroutine(RandomNavSphere(transform.position, stats.GetIdleRange(), stats.GetIdleHowOftenDirectionChanged(), -1));

            // Follow player
            agent.SetDestination(targets[playerIndex].position);
            FaceTarget(targets[playerIndex].position);
            animator.SetBool("isWalking", true);

            // If player is within attacking range, attack the player
            if (shortestDistance <= agent.stoppingDistance)
            {
                animator.SetBool("isWalking", false);
                animator.SetTrigger("attack");
                Attack(targets[playerIndex]);
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

        if (direction != Vector3.zero && !float.IsNaN(direction.x) && !float.IsNaN(direction.z))
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
    private void Attack(Transform attackedPlayer)
    {
        if (!isServer) return;

        HeroModel heroStats = attackedPlayer.GetComponent<HeroModel>();
        if (heroStats != null && attackCooldown <= 0)
        {
            heroStats.CmdTakeDamage(stats.GetDamage());
            attackCooldown = 1f / stats.GetAttackSpeed();
        }
    }
}
