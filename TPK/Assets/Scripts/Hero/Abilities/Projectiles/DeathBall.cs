using UnityEngine;

/// <summary>
/// Projectile behaviour for Death Ball skill.
/// </summary>
public class DeathBall : BaseProjectile
{
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
            default:
                Destroy(gameObject);
                break;
        }
    }
}
