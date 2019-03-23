using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Networking;

/// <summary>
/// Contains all logic regarding enemy AI for player targetting & movement.
/// </summary>
[RequireComponent(typeof(CharacterCombat))]
[RequireComponent(typeof(NavMeshAgent))]
public class EnemyController : NetworkBehaviour
{
    // For unit testing purposes
    public bool localTest;
    public IUnityService unityService;

    private readonly float lookRadius = 20f;    // radius where enemy can detect players
    private NavMeshAgent agent;                 // required to navigate the map
    private CharacterCombat enemyCombat;
    private MatchManager matchManager;
    private HeroManager heroManager;
    private AnimationEnemyController animation;
    private EnemyStats myStats;

    private Vector3 currentRandomLocation;
    private bool isIdleMovement;
    private bool isAttacking;
    private float idleRange;
    private float idleHowOftenDirectionChanged;

    /// <summary>
    /// Use this for initialization.
    /// </summary>
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        enemyCombat = GetComponent<CharacterCombat>();
        matchManager = GameObject.FindGameObjectWithTag("MatchManager").GetComponent<MatchManager>();
        heroManager = GameObject.FindGameObjectWithTag("MatchManager").GetComponent<HeroManager>();
        animation = GetComponent<AnimationEnemyController>();
        myStats = GetComponent<EnemyStats>();

        isAttacking = false;
        agent.speed = myStats.getMovementSpeed().GetValue();
        idleRange = myStats.getIdleRange();
        idleHowOftenDirectionChanged = myStats.getIdleHowOftenDirectionChanged();

        //StartCoroutine(RandomNavSphere(transform.position, 6, -1));

        if (unityService == null)
        {
            unityService = new UnityService();
        }
    }

    /// <summary>
    /// Called once per frame.
    /// </summary>
    void Update()
    {
        // Enemy behaviour should only occur on the server
        if (!localTest && !isServer) return;

        // Stop movement if match has ended
        if (matchManager.HasMatchEnded())
        {
            agent.isStopped = true;
            return;
        }

        // Perform targetting AI
        TargetClosestPlayer();
    }

    /// <summary>
    /// Sets the target of this enemy to the closest player.
    /// </summary>
    private void TargetClosestPlayer()
    {
        Transform[] targets = heroManager.GetAllPlayerTransforms(); // list of all player transforms
        float shortestDistance = int.MaxValue;  // distance to the closest player
        int playerIndex = -1;   // index of the player in targets array whom is currently targetted

        // Get distance of player closest to the enemy
        for (int i = 0; i < targets.Length; i++)
        {
            // Do not target knocked out players
            if (targets[i].GetComponent<HeroModel>().IsKnockedOut())
            {
                continue;
            }

            // Find the index of the closest player
            float distance = Vector3.Distance(targets[i].position, transform.position);
            if (distance < shortestDistance)
            {
                shortestDistance = distance;
                playerIndex = i;
            }
        }

        // If player is within lookRadius of enemy, follow the player
        if (shortestDistance <= lookRadius && playerIndex >= 0)
        {
            //stop idle movement
            isIdleMovement = false;
            StopCoroutine(RandomNavSphere(transform.position, idleRange, idleHowOftenDirectionChanged, -1));

            agent.SetDestination(targets[playerIndex].position);
            FaceTarget(targets[playerIndex].position);
            animation.animationWalk(true);

            // If player is within attacking range, attack the player
            if (shortestDistance <= agent.stoppingDistance)
            {
                animation.animationWalk(false);
                //if (!isAttacking)
                //{
                //    //start attacking animation
                //    isAttacking = true;
                //    StartCoroutine(attacking());
                //}
                animation.playAttack();
                enemyCombat.Attack(targets[playerIndex]);
            }
            //stop attacking animation if not in range and was currently attacking
            //else
            //{
            //    if (isAttacking)
            //    {
            //        isAttacking = false;
            //        Debug.Log("Stopped");
            //        StopCoroutine(attacking());
            //    }
            //}
        }
        //otherwise random direction
        else
        {
            idleMovement();
        }
    }

    /// <summary>
    /// Enum for attack animation loop
    /// </summary>
    public IEnumerator attacking()
    {
        while (isAttacking)
        {
            //Debug.Log("attacked");
            animation.playAttack();
            yield return new WaitForSeconds(2);
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
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * rotationSpeed);
    }

    /// <summary>
    /// Destroys this enemy on the server.
    /// </summary>
    public void KillMe()
    {
        CmdKillMe();
        unityService.Destroy(gameObject);
    }

    /// <summary>
    /// Destroys this enemy on all clients.
    /// </summary>
    [Command]
    private void CmdKillMe()
    {
        NetworkServer.Destroy(gameObject);
    }

    /// <summary>
    /// Idle movement for enemy.
    /// </summary>
    private void idleMovement()
    {
        animation.animationWalk(true);
        //if not currently idle movement, start random location couroutine
        if (!isIdleMovement)
        {
            StartCoroutine(RandomNavSphere(transform.position, idleRange, idleHowOftenDirectionChanged, -1));
            isIdleMovement = true;
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
                    animation.animationWalk(false);
                }
            }
        }
    }

    /// <summary>
    /// Generate random location near specified navmesh agent
    /// </summary>
    IEnumerator RandomNavSphere(Vector3 origin, float distance, float howOften, int layermask)
    {
        while (true)
        {
            Vector3 randomDirection = UnityEngine.Random.insideUnitSphere * distance;
            randomDirection += origin;
            NavMeshHit navHit;
            NavMesh.SamplePosition(randomDirection, out navHit, distance, layermask);
            currentRandomLocation = navHit.position;
            //Debug.Log("New location at " + currentRandomLocation);

            yield return new WaitForSeconds(Random.Range(howOften-1, howOften+1));
        }
    }
}
