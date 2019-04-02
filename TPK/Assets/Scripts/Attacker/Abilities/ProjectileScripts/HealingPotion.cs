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
            int maxHealth = stats.GetMaxHealth();
            float healAmount = maxHealth * 0.2f;
            if (currentHp < maxHealth)
            {
                stats.CmdHeal((int)healAmount);
            }
            Destroy(gameObject);
        }
        Debug.Log("Healed");

    }
}
