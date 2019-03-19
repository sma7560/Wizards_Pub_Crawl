using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStats : CharacterStats
{
    protected float dropRate = 100f;
    protected Stat movementSpeed = new Stat();
    protected float idleRange;
    protected float idleHowOftenDirectionChanged;

    //mosnter death overrides default die method
    protected override void Die()
    {
        float randChance = Random.Range(0f, 100f);

        //random chance to drop health pickup
        if(randChance <= dropRate)
        {
            Debug.Log("Item dropped");
            GameObject monsterDrop = Resources.Load("Items/HealthPickup") as GameObject;
            Vector3 itemPosition = transform.position;
            itemPosition.y = itemPosition.y + 0.7f;
            Instantiate(monsterDrop, itemPosition, Quaternion.Euler(90, 0, 0));
            Debug.Log("Item dropped succesfully");
        }
        // Add enemy death animation here
        Destroy(gameObject);
    }

    public float getDropRate()
    {
        return dropRate;
    }

    public float getIdleRange()
    {
        return idleRange;
    }

    public float getIdleHowOftenDirectionChanged()
    {
        return idleHowOftenDirectionChanged;
    }

    public Stat getMovementSpeed()
    {
        return movementSpeed;
    }
}
