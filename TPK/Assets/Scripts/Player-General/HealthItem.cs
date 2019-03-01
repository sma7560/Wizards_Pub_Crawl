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
    public override void ItemConsume(Collider other)
    {
        NetworkHeroManager stats = other.gameObject.GetComponent<NetworkHeroManager>();
        int currentHp = stats.currentHealth;

        //if health is less than max, heal player
        if (currentHp < stats.maxHealth)
        {
            stats.Heal(hpHealed);
        }
    }

}
