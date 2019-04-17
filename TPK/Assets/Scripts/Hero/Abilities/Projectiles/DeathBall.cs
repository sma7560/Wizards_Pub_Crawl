using UnityEngine;
using UnityEngine.Networking;

/// <summary>
/// Projectile behaviour for Death Ball skill.
/// </summary>
public class DeathBall : BaseProjectile
{
	public GameObject impactFX;

    /// <summary>
    /// Basic collision and destroy self.
    /// </summary>
    public override void Behaviour(Collision col)
    {
        base.Behaviour(col);

        switch (col.collider.tag)
        {
            case "Enemy":
                // Damage the enemy
                if (col.collider.GetComponent<EnemyModel>() != null)
                {
                    col.collider.GetComponent<EnemyModel>().CmdTakeDamage(damage);
                }
				CmdEffect ();
                Destroy(gameObject);
                break;
            case "Player":
                // Damage the player
                if (col.collider.GetComponent<HeroModel>() != null)
                {
                    col.collider.GetComponent<HeroModel>().CmdTakeDamage(damage);
                }
				CmdEffect ();
                Destroy(gameObject);
                break;
            default:
                Destroy(gameObject);
                break;
        }
    }

	/// <summary>
	/// Plays the impact effect.
	/// </summary>
	[Command]
	private void CmdEffect ()
	{
		GameObject effect = Instantiate(impactFX);
		effect.transform.position = transform.position;
		NetworkServer.Spawn(effect);
	}
}
