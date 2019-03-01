using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

/*
 * Interface class for items
 */
public class Item : NetworkBehaviour
{
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
    public virtual void ItemConsume(Collider other)
    {
        //child classes will define this function
    }
}
