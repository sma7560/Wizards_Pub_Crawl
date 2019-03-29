using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

/*
 * health pick-up class, inherits from Item
 */
public class MoveSpeedBuff : Item
{
    public int buffAmount = 5;

    //override item function with what health consumable does to player
    protected override void ItemConsume(Collider other)
    {
        HeroModel stats = other.gameObject.GetComponent<HeroModel>();
        StartCoroutine(tempBuff(stats));
    }

    protected override IEnumerator tempBuff(HeroModel currentStat)
    {
		//max buff amt is base speed + buff amount, can't exceed that
		int maxspeed = currentStat.GetBaseMoveSpeed () + buffAmount;

		//do nothing if player already buffed to maxspeed
		if(currentStat.GetCurrentMoveSpeed() >= maxspeed)
		{
			Destroy(gameObject);
		}

		int potentialBuffAmt = currentStat.GetCurrentMoveSpeed() + buffAmount;

		//if player is debuffed, the buffed speed might still be under the max speed
		if (potentialBuffAmt <= maxspeed) 
		{
			currentStat.SetCurrentMoveSpeed (potentialBuffAmt);

		} 
		//otherwise it may exceed the maxspeed
		else
		{
			//reducing buff amount so that it can bring player to max speed
			buffAmount = maxspeed - currentStat.GetCurrentMoveSpeed ();
			currentStat.SetCurrentMoveSpeed (maxspeed);

		}

		//buff lasts for 30 seconds
		yield return new WaitForSeconds (30);
		//set stat back to original stat
		currentStat.SetCurrentMoveSpeed (currentStat.GetCurrentMoveSpeed () - buffAmount);

        Destroy(gameObject);
    }
}
