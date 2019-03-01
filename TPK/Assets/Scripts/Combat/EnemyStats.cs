using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStats : CharacterStats
{
    public float dropRate = 30f;

    //mosnter death overrides default die method
    protected override void Die()
    {
        float randChance = Random.Range(0f, 100f);

        //random chance to drop health pickup
        if(randChance >= dropRate)
        {
            GameObject monsterDrop = Resources.Load("Items/HealthPickup") as GameObject;
            Instantiate(monsterDrop, transform.position, Quaternion.Euler(0, 0, 0));
        }
        // Add enemy death animation here
        Destroy(gameObject);
    }
}
