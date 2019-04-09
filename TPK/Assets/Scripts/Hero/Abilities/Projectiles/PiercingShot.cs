using UnityEngine;

/// <summary>
/// Projectile behaviour for Piercing Shot skill.
/// </summary>
public class PiercingShot : BaseProjectile
{
    /// <summary>
    /// The piercing shot only destroys on things that are not walls/environment objects.
    /// </summary>
    public override void Behaviour(Collider col)
    {
        switch (col.transform.tag)
        {
            case "Enemy":
                // Damage enemy
                if (col.transform.GetComponent<EnemyModel>() != null)
                {
                    col.transform.GetComponent<EnemyModel>().CmdTakeDamage(damage);
                }
                break;
            case "Player":
                // Damage player
                if (col.transform.GetComponent<HeroModel>() != null)
                {
                    col.transform.GetComponent<HeroModel>().CmdTakeDamage(damage);
                }
                break;
            case "Projectile":
                // Pierces projectiles and shields
                break;
            default:
                Destroy(gameObject);
                break;
        }
    }
}
