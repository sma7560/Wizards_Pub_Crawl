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
        rotateAnimation();
    }

    //check if player comes in contact with item
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            ItemConsume(other);
        }
        //disable mesh after player interacts with
        gameObject.transform.GetChild(0).gameObject.SetActive(false);
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

    protected virtual void rotateAnimation()
    {
        //to be implemented by children
    }
}
