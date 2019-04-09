using UnityEngine;

/// <summary>
/// Behaviour for Stone Wall skill.
/// </summary>
public class StoneWall : BaseProjectile
{
    private readonly int maxHits = 5;   // max number of hits before wall gets destroyed
    private int currentHits = 0;

    /// <summary>
    /// The stone wall is designed to block a certain number of projectiles until expiring.
    /// Making it strong but not overpowered. This allows other players to trade time to break down the blockade.
    /// </summary>
    public override void Behaviour(Collision col)
    {
        base.Behaviour(col);

        switch (col.collider.tag)
        {
            case "Projectile":
                // Takes a hit from projectile
                if (++currentHits == maxHits)
                {
                    Destroy(gameObject);
                }
                break;
            case "Safezone":
                // Cannot create stone wall in spawn room
                Destroy(gameObject);
                break;
        }

    }
}
