using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

/// <summary>
/// Base class for all item drops.
/// </summary>
public class Item : NetworkBehaviour
{
    private bool isUsed = false;        // if this item has already been used by a player

    /// <summary>
    /// Checks for player collisions with items.
    /// </summary>
    /// <param name="other">Collider colliding with this item.</param>
    private void OnTriggerEnter(Collider other)
    {
        // Don't trigger item use if item has already been used
        if (isUsed) return;

        if (other.gameObject.tag == "Player")
        {
            isUsed = true;
            ItemConsume(other);

            // Disable mesh after player interacts with
            gameObject.transform.GetChild(0).gameObject.SetActive(false);
        }
    }

    /// <summary>
    /// Called upon consumption of the item.
    /// Only players may consume items.
    /// </summary>
    protected virtual void ItemConsume(Collider other)
    {
        if (other.gameObject.tag != "Player") return;
    }

    /// <summary>
    /// Temporary buff effects of the item.
    /// </summary>
    /// <param name="stats">Player's stat data.</param>
    protected virtual IEnumerator TempBuff(HeroModel stats)
    {
        yield return null;
    }
}
