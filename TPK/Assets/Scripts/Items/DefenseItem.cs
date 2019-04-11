using System.Collections;
using UnityEngine;

/// <summary>
/// Defense item drop from monsters.
/// Attached to the DefenseItem prefab.
/// </summary>
public class DefenseItem : Item
{
    public readonly int buffAmount = 5;     // how much defense this buff will give
    public readonly int buffTime = 30;      // seconds that temporary buff lasts for

    /// <summary>
    /// Called upon consumption of the defense item.
    /// </summary>
    /// <param name="other">Player's collider.</param>
    protected override void ItemConsume(Collider other)
    {
        base.ItemConsume(other);
        other.gameObject.GetComponent<PlayerSoundController>().RpcPlayItemBuffSound();
        HeroModel stats = other.gameObject.GetComponent<HeroModel>();
        StartCoroutine(TempBuff(stats));
    }

    /// <summary>
    /// Temporarily buffs the player's defense stat.
    /// </summary>
    /// <param name="stats">Player's stat data.</param>
    protected override IEnumerator TempBuff(HeroModel stats)
    {
        int defense = stats.GetCurrentDefense();
        stats.SetCurrentDefense(defense + buffAmount);
        
        yield return new WaitForSeconds(buffTime);

        defense = stats.GetCurrentDefense();
        stats.SetCurrentDefense(defense - buffAmount);

        Destroy(gameObject);
    }
}
