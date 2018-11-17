using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

[RequireComponent(typeof(CharacterStats))]
public class CharacterCombat : NetworkBehaviour
{

    private CharacterStats myStats;
    private float attackCooldown = 0;

    // Use this for initialization
    void Start()
    {
        myStats = GetComponent<CharacterStats>();
    }

    // Update is called once per frame
    void Update()
    {
        attackCooldown -= Time.deltaTime;
    }

    // Performs basic attack
    [Command] //For hero attack to register on server
    public void CmdAttack()
    {
        Debug.Log(transform.name + " performs a basic attack.");

        // Attack logic for the player (hero)
        if (gameObject.tag == "Player")
        {
            // Find all enemy objects

            //If you are doing it this way you can just go overlap, unity has a built in physics overlap so we wont have to
            //Cycle through as many.
            GameObject[] enemyObjects = GameObject.FindGameObjectsWithTag("Enemy");

            for (int i = 0; i < enemyObjects.Length; i++)
            {
                // Find the distance between player and enemies
                float distance = Vector3.Distance(transform.position, enemyObjects[i].transform.position);
                Interactable enemy = enemyObjects[i].GetComponent<Interactable>();

                // If enemy is within interactable radius and we are ready to attack, attack it
                if (distance < enemy.interactRadius && attackCooldown <= 0)
                {
                    EnemyStats enemyStats = enemyObjects[i].GetComponent<EnemyStats>();
                    //enemyStats.TakeDamage(myStats.damage.GetValue());
                    attackCooldown = 1f / myStats.attackSpeed.GetValue();
                }
            }
        }
    }

    // Performs an attack on an specific object (ie. a hero)
    public void Attack(Transform attackedObject)
    {
        CharacterStats attackedStats = attackedObject.GetComponent<CharacterStats>();
        if (attackedStats != null && attackCooldown <= 0)
        {
            attackedStats.TakeDamage(myStats.damage.GetValue());
            Debug.Log(attackedStats.currentHealth);
            attackCooldown = 1f / myStats.attackSpeed.GetValue();
        }
    }
}
