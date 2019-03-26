using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathBall : BaseProjectile
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
                Destroy(gameObject);
                break;
            case "Player":
                if (col.collider.GetComponent<HeroModel>())
                {
                    col.collider.GetComponent<HeroModel>().CmdTakeDamage(damage, damageType);
                }
                Destroy(gameObject);
                break;
            default:
                Destroy(gameObject);
                break;
        }
    }
}
