using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealingPotion : BaseProjectile
{
    public override void Behaviour(Collider col)
    {
        base.Behaviour(col);

        if (col.tag == "Player")
        {
            Debug.Log("Healing Player");
            HeroModel stats = col.gameObject.GetComponent<HeroModel>();
            int currentHp = stats.GetCurrentHealth();
            int maxHealth = stats.GetMaxHealth();
            float healAmount = maxHealth * 0.2f;
            if (currentHp < maxHealth)
            {
                stats.CmdHeal((int)healAmount);
            }
            Destroy(gameObject);
            Debug.Log("Healed");
        }

    }
}
