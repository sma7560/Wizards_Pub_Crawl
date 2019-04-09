using UnityEngine;

/// <summary>
/// Projectile behaviour for Bouncing Bolt skill.
/// </summary>
public class BouncingBolt : BaseProjectile
{

    /// <summary>
    /// Determine what happens at each bounce (Collision).
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
                Destroy(gameObject);
                break;
            case "Player":
                // Damage the player
                if (col.collider.GetComponent<HeroModel>() != null)
                {
                    col.collider.GetComponent<HeroModel>().CmdTakeDamage(damage);
                }
                Destroy(gameObject);
                break;
            case "Safezone":
                // No projectiles allowed in spawn rooms
                Destroy(gameObject);
                break;
            default:
                // Collision with an inanimate object
                // Every bounce increases damage and speed
                damage += 10;
                Vector3 v = GetComponent<Rigidbody>().velocity;
                // cap the speed
                if (v.magnitude <= 20)
                {
                    v.x *= 1.20f;
                    v.z *= 1.20f;
                    this.GetComponent<Rigidbody>().velocity = v;
                }
                break;
        }
    }
}
