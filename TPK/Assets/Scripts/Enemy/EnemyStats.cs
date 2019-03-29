using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

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
    private bool isDying;
    private IUnityService unityService;

    private void Start()
    {
        if (unityService == null)
        {
            unityService = new UnityService();
        }
    }

    //mosnter death overrides default die method
    protected override void Die()
    {
        //don't die again if currently dying
        if (!isDying)
        {
            StartCoroutine(deathSequence());
        }
    }

    //death sequence for enemy
    private IEnumerator deathSequence()
    {
        isDying = true;
        animation = GetComponent<AnimationEnemyController>();
        animation.deathAnimation();
        //wait 4 seconds for animation to finish before deleting gameobject
        yield return new WaitForSeconds(3);
        dropItem();
        Destroy(gameObject);
    }

    //function to drop items upon death
    private void dropItem()
    {
        if (!isServer) return;
        int randChance = Random.Range(0, 101);

        //random chance to drop health pickup
        if (randChance > 100 - totalDropRate)
        {
            Debug.Log("Item dropping");
            GameObject monsterDrop = determineItemDrop();
            Vector3 itemPosition = transform.position;
            itemPosition.y = itemPosition.y + 0.7f;
            GameObject temp = unityService.Instantiate(monsterDrop, itemPosition, Quaternion.Euler(0, 0, 0));
            NetworkServer.Spawn(temp);
            Debug.Log(monsterDrop.name + " dropped succesfully");
        }
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

    public bool getIsDying()
    {
        return isDying;
    }
}
