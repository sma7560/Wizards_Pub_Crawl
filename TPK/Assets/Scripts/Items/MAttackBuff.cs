using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

/*
 * health pick-up class, inherits from Item
 */
public class MAttackBuff : Item
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
        //get original stat
        int origStat = currentStat.GetMAttack();
        //set stat to include buffs
        currentStat.SetMAttack(origStat + buffAmount);

        //buff lasts for 30 seconds
        yield return new WaitForSeconds(30);
        //set stat back to original stat
        currentStat.SetMAttack(origStat);
    }
}
