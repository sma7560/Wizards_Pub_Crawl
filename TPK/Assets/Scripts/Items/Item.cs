using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

/*
 * Interface class for items
 */
public class Item : NetworkBehaviour
{
    private void Update()
    {
        //rotation animation
        transform.RotateAround(transform.position, transform.forward, Time.deltaTime * 90f);
    }

    //check if player comes in contact with item
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            ItemConsume(other);
        }
        Destroy(gameObject);
    }

    //item effect. Will be override by specific item effects
    protected virtual void ItemConsume(Collider other)
    {
        //child classes will define this function
    }

    protected virtual IEnumerator tempBuff(HeroModel currentStat)
    {
        //to be implemented by children
        yield return new WaitForSeconds(30);
    }
}
