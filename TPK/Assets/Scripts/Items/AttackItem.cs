using System.Collections;
using UnityEngine;

/// <summary>
/// Attack item drop from monsters.
/// Attached to the AttackItem prefab.
/// </summary>
public class AttackItem : Item
{
    public readonly int buffAmount = 5;     // how much attack this buff will give
    public readonly int buffTime = 30;      // seconds that temporary buff lasts for

    /// <summary>
    /// Called upon consumption of the attack item.
    /// </summary>
    /// <param name="other">Player's collider.</param>
    protected override void ItemConsume(Collider other)
    {
        base.ItemConsume(other);
        other.gameObject.GetComponent<PlayerSoundController>().PlayItemBuffSound();
        HeroModel stats = other.gameObject.GetComponent<HeroModel>();
        StartCoroutine(TempBuff(stats));
		transform.position = new Vector3 (-80, 0, -80);
    }

    /// <summary>
    /// Temporarily buffs the player's attack stat.
    /// </summary>
    /// <param name="stats">Player's stat data.</param>
    protected override IEnumerator TempBuff(HeroModel stats)
    {
        int attack = stats.GetCurrentAttack();
        stats.SetCurrentAttack(attack + buffAmount);

        yield return new WaitForSeconds(buffTime);
        
        attack = stats.GetCurrentAttack();
        stats.SetCurrentAttack(attack - buffAmount);

        Destroy(gameObject);
    }
}
