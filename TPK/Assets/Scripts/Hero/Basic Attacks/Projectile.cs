using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

/// <summary>
/// Projectile of the basic attack.
/// </summary>
public class Projectile : NetworkBehaviour
{
    private readonly float s = 0.25f;

    public float range;
    public int damage;
    public int playerID;
    private float hasTimeElapsed;

	public GameObject impactFX;
    
    /// <summary>
    /// Sets the parameters of the projectile.
    /// </summary>
    /// <param name="r">Range of projectile.</param>
    /// <param name="dmg">Damage of the projectile.</param>
    /// <param name="pid">Player ID of the player who spawned the projectile.</param>
    public void SetProjectileParams(float r, int dmg, int pid)
    {
        range = r;
        damage = dmg;
        playerID = pid;
        hasTimeElapsed = Time.time + s;
    }

    private void OnCollisionEnter(Collision col)
    {
        if (!isServer) return;

        switch (col.collider.tag)
        {
			case "Enemy":
	                // Damage enemy
				if (col.collider.GetComponent<EnemyModel> () != null)
				{
					col.collider.GetComponent<EnemyModel> ().CmdTakeDamage (damage);
				}
				CmdEffect ();
                break;
            case "Player":
                // Damage player
                if (col.collider.GetComponent<HeroModel>() != null)
                {
                    if (Time.time < hasTimeElapsed && col.collider.GetComponent<HeroModel>().GetPlayerId() == playerID) return;
                    col.collider.GetComponent<HeroModel>().CmdTakeDamage(damage);
                }
				CmdEffect ();
                break;
        }

        Destroy(gameObject);
    }

	/// <summary>
	/// Plays the impact effect.
	/// </summary>
	[Command]
	private void CmdEffect()
	{
		GameObject effect = Instantiate(impactFX);
		effect.transform.position = transform.position;
		NetworkServer.Spawn(effect);
	}
}
