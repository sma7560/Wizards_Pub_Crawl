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

    private readonly float lookRadius = 10f;    // radius where enemy can detect players
    private NavMeshAgent agent;                 // required to navigate the map
    private CharacterCombat enemyCombat;
    private MatchManager matchManager;
    private HeroManager heroManager;

    /// <summary>
    /// Use this for initialization.
    /// </summary>
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        enemyCombat = GetComponent<CharacterCombat>();
        matchManager = GameObject.FindGameObjectWithTag("MatchManager").GetComponent<MatchManager>();
        heroManager = GameObject.FindGameObjectWithTag("MatchManager").GetComponent<HeroManager>();

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
            agent.SetDestination(targets[playerIndex].position);
            FaceTarget(targets[playerIndex]);

            // If player is within attacking range, attack the player
            if (shortestDistance <= agent.stoppingDistance)
            {
                enemyCombat.Attack(targets[playerIndex]);
            }
        }
    }

    /// <summary>
    /// Rotates this enemy to face its current target.
    /// </summary>
    /// <param name="target">Transform of this enemy's current target.</param>
    private void FaceTarget(Transform target)
    {
        float rotationSpeed = 5f;
        Vector3 direction = (target.position - transform.position).normalized;
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
}
