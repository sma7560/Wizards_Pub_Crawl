using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

/*
 * Interface class for items
 */
public class Item : NetworkBehaviour
{
    private bool isUsed = false;

    //check if player comes in contact with item
    private void OnTriggerEnter(Collider other)
    {
        //don't trigger item use if item has already been used
        if (isUsed)
        {
            return;
        }

        if (other.gameObject.tag == "Player")
        {
            isUsed = true;
            ItemConsume(other);
            //disable mesh after player interacts with
            gameObject.transform.GetChild(0).gameObject.SetActive(false);
        }
    }

    //item effect. Will be override by specific item effects
    protected virtual void ItemConsume(Collider other)
    {
        //child classes will define this function
    }

    protected virtual IEnumerator tempBuff(HeroModel currentStat)
    {
        //to be implemented by children
        yield return new WaitForSeconds(5);
    }
}
