using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Fireball : BaseProjectile {
    public GameObject explosion;
    public float explosionRange = 3f;


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
                Explode();
                break;
            case "Player":
                if (col.collider.GetComponent<HeroModel>())
                {
                    col.collider.GetComponent<HeroModel>().CmdTakeDamage(damage, damageType);
                }
                Explode();
                break;
            default:
                Explode();
                break;
        }
    }

    private void Explode() {
        CmdEffect();
        Collider[] aroundMe = Physics.OverlapSphere(transform.position, explosionRange);
        foreach (Collider hit in aroundMe)
        {
            if (hit.tag == "Enemy" || hit.tag == "Player")
            {
                if (hit.transform.root != transform)
                {
                    // Doing stuff in here.
                    switch (hit.tag)
                    {
                        case "Enemy":
                            // Get enemy component for dealing damage to it.
                            if (hit.GetComponent<EnemyStats>())
                            {
                                // This will change.
                                hit.GetComponent<EnemyStats>().CmdTakeDamage(damage);
                            }
                            break;
                        case "Player":
                            // Get other players component for taking damage.
                            hit.GetComponent<HeroModel>().CmdTakeDamage(damage, damageType);
                            break;
                    }
                }
            }
        }
        Destroy(gameObject);

    }

    [Command]
    private void CmdEffect() {
        Debug.Log("Playing Explosion Effect");
        if (explosion == null) Debug.Log("Effects not instantiated. on server.");
        GameObject effect = Instantiate(explosion);
        effect.transform.position = transform.position;
        NetworkServer.Spawn(effect);
        //Destroy(effect, 2);
    }

}
