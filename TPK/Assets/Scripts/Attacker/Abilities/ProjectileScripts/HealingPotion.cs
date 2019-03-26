using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealingPotion : BaseProjectile {

    public override void Behaviour(Collision col)
    {
       
        base.Behaviour(col);
        if (col.collider.tag == "Player") {
            if (col.collider.GetComponent<HeroModel>())
            {
                col.collider.GetComponent<HeroModel>().Heal(damage);
            }
            Destroy(gameObject);
        }
    }
}
