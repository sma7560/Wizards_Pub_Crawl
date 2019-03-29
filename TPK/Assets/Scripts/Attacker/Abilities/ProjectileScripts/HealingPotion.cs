using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealingPotion : BaseProjectile {

    public override void Behaviour(Collider col)
    {
        HeroModel stats = col.gameObject.GetComponent<HeroModel>();
        int currentHp = stats.GetCurrentHealth();
        base.Behaviour(col);

        Debug.Log("Healing Player");
        if (col.tag == "Player") {
            if (currentHp < stats.GetMaxHealth())
            {
                stats.CmdHeal(damage);
            }
            Destroy(gameObject);
        }
        Debug.Log("Healed");

    }
}
