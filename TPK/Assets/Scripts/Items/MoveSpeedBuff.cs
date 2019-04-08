using System.Collections;
using UnityEngine;

/// <summary>
/// Speed item drop from monsters.
/// Attached to the MovementItem prefab.
/// </summary>
public class MoveSpeedBuff : Item
{
    public readonly int buffAmount = 3;     // how much speed this buff will give
    public readonly int buffTime = 30;      // seconds that temporary buff lasts for

    /// <summary>
    /// Called upon consumption of the speed item.
    /// </summary>
    /// <param name="other">Player's collider.</param>
    protected override void ItemConsume(Collider other)
    {
        base.ItemConsume(other);

        HeroModel stats = other.gameObject.GetComponent<HeroModel>();
        StartCoroutine(TempBuff(stats));
    }

    /// <summary>
    /// Temporarily buffs the player's speed stat.
    /// NOTE: only one speed buff can be active at a time.
    /// </summary>
    /// <param name="stats">Player's stat data.</param>
    protected override IEnumerator TempBuff(HeroModel stats)
    {
        if (stats.IsSpeedBuffed())
        {
            // Do nothing if player is already speed buffed
            Destroy(gameObject);
        }
        else
        {
            // Apply speed buff
            yield return new WaitForSeconds(buffTime);
            stats.SetCurrentMoveSpeed(stats.GetBaseMoveSpeed());
            Destroy(gameObject);
        }
    }
}
