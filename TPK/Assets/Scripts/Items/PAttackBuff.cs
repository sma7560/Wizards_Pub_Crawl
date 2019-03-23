﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

/*
 * health pick-up class, inherits from Item
 */ 
public class PAttackBuff : Item
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
        int origStat = currentStat.GetPAttack();
        Debug.Log("Buff start, " + origStat);
        //set stat to include buffs
        currentStat.SetPAttack(origStat + buffAmount);
        Debug.Log("Buff execute, " + currentStat.GetPAttack());

        //buff lasts for 30 seconds
        yield return new WaitForSeconds(30);
        //set stat back to original stat
        //Debug.Log("Buff end, " + currentStat.GetPAttack());
        currentStat.SetPAttack(origStat);
        //Debug.Log("Buff end, " + currentStat.GetPAttack());
        Destroy(gameObject);
    }

    protected override void rotateAnimation()
    {
        transform.RotateAround(transform.position, transform.up, Time.deltaTime * 90f);
    }
}