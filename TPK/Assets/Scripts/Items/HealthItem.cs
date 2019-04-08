using UnityEngine;

/// <summary>
/// Health item drop from monsters.
/// Attached to the HealthPickup prefab.
/// </summary>
public class HealthItem : Item
{
    public readonly float hpPercentHeal = 0.2f; // percentage of hp this item will heal

    /// <summary>
    /// Called upon consumption of the health item.
    /// Heals the player by a percentage amount.
    /// </summary>
    /// <param name="other">Player's collider.</param>
    protected override void ItemConsume(Collider other)
    {
        base.ItemConsume(other);

        HeroModel stats = other.gameObject.GetComponent<HeroModel>();
        stats.CmdHeal((int)(stats.GetMaxHealth() * hpPercentHeal));

        Destroy(gameObject);
    }
}
