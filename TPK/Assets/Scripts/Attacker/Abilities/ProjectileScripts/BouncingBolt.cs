using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BouncingBolt : BaseProjectile
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
                // This means you can shoot yourself... so far.
                break;
			default:
				this.damage += 10;
				Vector3 v = this.GetComponent<Rigidbody> ().velocity;
				v.x *= 1.10f;
				v.z *= 1.10f;
				this.GetComponent<Rigidbody> ().velocity = v;
				break;


        }
    }
}
