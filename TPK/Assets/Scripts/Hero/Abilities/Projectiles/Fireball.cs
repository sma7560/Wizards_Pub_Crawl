using UnityEngine;
using UnityEngine.Networking;

/// <summary>
/// Projectile behaviour for Fireball skill.
/// </summary>
public class Fireball : BaseProjectile
{
    public GameObject explosion;
    private readonly float explosionRange = 3f;

    /// <summary>
    /// Damage enemies/players upon collision. As well as cause an explosion like effect that also damages enemies.
    /// </summary>
    public override void Behaviour(Collision col)
    {
        base.Behaviour(col);

        switch (col.collider.tag)
        {
            case "Enemy":
                // Damage the enemy
                if (col.collider.GetComponent<EnemyModel>() != null)
                {
                    col.collider.GetComponent<EnemyModel>().CmdTakeDamage(damage);
                }
                Explode();
                break;
            case "Player":
                // Damage the player
                if (col.collider.GetComponent<HeroModel>() != null)
                {
                    col.collider.GetComponent<HeroModel>().CmdTakeDamage(damage);
                }
                Explode();
                break;
            default:
                Explode();
                break;
        }
    }

    /// <summary>
    /// Explosion bits also damage enemies and players.
    /// </summary>
    private void Explode()
    {
        CmdEffect();
        Collider[] aroundMe = Physics.OverlapSphere(transform.position, explosionRange);
        foreach (Collider hit in aroundMe)
        {
            if (hit.tag == "Enemy" || hit.tag == "Player")
            {
                if (hit.transform.root != transform)
                {
                    switch (hit.tag)
                    {
                        case "Enemy":
                            // Damage the enemy
                            if (hit.GetComponent<EnemyModel>() != null)
                            {
                                hit.GetComponent<EnemyModel>().CmdTakeDamage(damage);
                            }
                            break;
                        case "Player":
                            // Damage the player
                            if (hit.GetComponent<HeroModel>() != null)
                            {
                                hit.GetComponent<HeroModel>().CmdTakeDamage(damage);
                            }
                            break;
                    }
                }
            }
        }

        Destroy(gameObject);

    }

    /// <summary>
    /// Plays the explosion effect.
    /// </summary>
    [Command]
    private void CmdEffect()
    {
        GameObject effect = Instantiate(explosion);
        effect.transform.position = transform.position;
        NetworkServer.Spawn(effect);
    }

}
