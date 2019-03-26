using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordToss : BaseProjectile
{

    public override void Behaviour(Collision col)
    {
        base.Behaviour(col);

        switch (col.collider.tag)
        {
            case "Enemy":
                if (col.collider.GetComponent<EnemyStats>())
                {
                    // This will change.
                    col.collider.GetComponent<EnemyStats>().CmdTakeDamage(damage);
                }
                break;
            case "Player":
                if (col.collider.GetComponent<HeroModel>())
                {
                    col.collider.GetComponent<HeroModel>().CmdTakeDamage(damage, damageType);
                }
                // This means you can shoot yourself... so far.
                break;
            //default:
            //    Destroy(gameObject);
            //    break;
        }
        Destroy(gameObject);
    }
}
