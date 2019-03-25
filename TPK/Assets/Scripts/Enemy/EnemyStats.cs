using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStats : CharacterStats
{
    [SerializeField]
    protected GameObject[] droppableItems = new GameObject[3];
    [SerializeField]
    //for each item in droppableItems. MUST be same length
    protected int totalDropRate;
    [SerializeField]
    protected Stat movementSpeed;
    [SerializeField]
    protected float idleRange;
    [SerializeField]
    protected float idleHowOftenDirectionChanged;
    private AnimationEnemyController animation;

    //mosnter death overrides default die method
    protected override void Die()
    {
        int randChance = Random.Range(0, 101);

        //random chance to drop health pickup
        if(randChance>100-totalDropRate)
        {
            Debug.Log("Item dropping");
            GameObject monsterDrop = determineItemDrop();
            Vector3 itemPosition = transform.position;
            itemPosition.y = itemPosition.y + 0.7f;
            Instantiate(monsterDrop, itemPosition, Quaternion.Euler(0, 0, 0));
            Debug.Log("Item dropped succesfully");
        }
        StartCoroutine(deathSequence());
    }

    //death sequence for enemy
    private IEnumerator deathSequence()
    {
        animation = GetComponent<AnimationEnemyController>();
        animation.deathAnimation();
        //wait 4 seconds for animation to finish before deleting gameobject
        yield return new WaitForSeconds(4);
        Destroy(gameObject);
    }

    private GameObject determineItemDrop()
    {
        int randItem = Random.Range(0, 101);
        if (randItem > 40)
        {
            return droppableItems[0];
        }
        else if (randItem > 20)
        {
            return droppableItems[1];
        }
        else
        {
            return droppableItems[2];
        }
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
