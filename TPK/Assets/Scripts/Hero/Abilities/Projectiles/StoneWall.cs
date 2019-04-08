using UnityEngine;

/// <summary>
/// Behaviour for Stone Wall skill.
/// </summary>
public class StoneWall : BaseProjectile
{
    private readonly int maxHits = 3;   // max number of hits before wall gets destroyed
    private int currentHits = 0;

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
