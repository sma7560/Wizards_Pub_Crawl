using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

/*
 * health pick-up class, inherits from Item
 */
public class HealthItem : Item {

    public int hpHealed = 20;

    //override item function with what health consumable does to player
    protected override void ItemConsume(Collider other)
    {
        HeroModel stats = other.gameObject.GetComponent<HeroModel>();
        int currentHp = stats.GetCurrentHealth();

        //if health is less than max, heal player
        if (currentHp < stats.GetMaxHealth())
        {
            stats.CmdHeal(hpHealed);
        }
        Destroy(gameObject);
    }
}
