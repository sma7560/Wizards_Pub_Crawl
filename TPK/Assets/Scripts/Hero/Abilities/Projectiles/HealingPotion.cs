using UnityEngine;

/// <summary>
/// Behaviour for Conjure Health skill.
/// </summary>
public class HealingPotion : BaseProjectile
{
    private readonly float percentageHeal = 0.2f;   // Percentage the potion will heal the player by

    public override void Behaviour(Collider col)
    {
        base.Behaviour(col);

        if (col.tag == "Player")
        {
            HeroModel stats = col.gameObject.GetComponent<HeroModel>();
            stats.CmdHeal((int)(stats.GetMaxHealth() * percentageHeal));
            Destroy(gameObject);
        }
    }
}
