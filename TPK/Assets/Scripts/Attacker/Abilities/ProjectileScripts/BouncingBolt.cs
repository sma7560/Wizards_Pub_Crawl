using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BouncingBolt : BaseProjectile {

    public override void behaviour(Collision col)
    {
        base.behaviour(col);
        switch (col.collider.tag)
        {
            case "Enemy":
                Destroy(gameObject);
                break;
            case "Player":
                if (col.collider.GetComponent<NetworkHeroManager>())
                {
                    col.collider.GetComponent<NetworkHeroManager>().CmdTakeDamage(damage, damageType);
                }
                Destroy(gameObject);
                // This means you can shoot yourself... so far.
                break;
        }
    }
}
