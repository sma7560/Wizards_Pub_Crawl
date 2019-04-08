using UnityEngine;

/// <summary>
/// Projectile behaviour for Sword Toss skill.
/// </summary>
public class SwordToss : BaseProjectile
{
    public override void Behaviour(Collision col)
    {
        base.Behaviour(col);

        switch (col.collider.tag)
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
        }

        Destroy(gameObject);
    }
}
