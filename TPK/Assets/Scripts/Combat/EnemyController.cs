using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Networking;

[RequireComponent(typeof(CharacterCombat))]
[RequireComponent(typeof(NavMeshAgent))]
public class EnemyController : NetworkBehaviour
{

    public float lookRadius = 10f;
    Transform[] targets;
    NavMeshAgent agent;
    int numPlayers;
    CharacterCombat enemyCombat;

    // Use this for initialization
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        enemyCombat = GetComponent<CharacterCombat>();
    }

    // Update is called once per frame
    void Update()
    {
        MakePlayerTargetList();
        TargetClosestPlayer();
    }
    
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, lookRadius);
    }

    // Targets the closest player
    private void TargetClosestPlayer()
    {
        float shortestDistance = int.MaxValue;  // distance to the closest player
        int playerIndex = -1;

        // Get distance of player closest to the enemy
        for (int i = 0; i < numPlayers; i++)
        {
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

            // If player is within attacking range, attack the player
            if (shortestDistance <= agent.stoppingDistance)
            {
                FaceTarget(targets[playerIndex]);
                enemyCombat.Attack(targets[playerIndex]);
            }
        }
    }

    // Make target list containing all players
    private void MakePlayerTargetList()
    {
        GameObject[] playerObjects = GameObject.FindGameObjectsWithTag("Player");
        numPlayers = playerObjects.Length;
        targets = new Transform[numPlayers];
        for (int i = 0; i < numPlayers; i++)
        {
            targets[i] = playerObjects[i].GetComponent<Transform>();
        }
    }

    // Rotate enemy to face the player it is currently attacking
    private void FaceTarget(Transform target)
    {
        float rotationSpeed = 5f;
        Vector3 direction = (target.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * rotationSpeed);
    }

    public void KillMe() {
        CmdKillme();
        Destroy(gameObject);
    }
    [Command]
    private void CmdKillme() {
        NetworkServer.Destroy(gameObject);
    }
}
