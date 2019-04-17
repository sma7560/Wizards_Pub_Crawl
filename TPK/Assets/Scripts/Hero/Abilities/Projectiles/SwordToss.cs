using UnityEngine;
using UnityEngine.Networking;

/// <summary>
/// Projectile behaviour for Sword Toss skill.
/// </summary>
public class SwordToss : BaseProjectile
{
	public GameObject impactFX;

    /// <summary>
    /// Destroy self after collision and damage is dealt.
    /// </summary>
    public override void Behaviour(Collision col)
    {
        base.Behaviour(col);

        switch (col.collider.tag)
        {
            case "Enemy":
                // Damage enemy
                if (col.transform.GetComponent<EnemyModel>() != null)
                {
                    col.transform.GetComponent<EnemyModel>().CmdTakeDamage(damage);
                }
				CmdEffect ();
                break;
            case "Player":
                // Damage player
                if (col.transform.GetComponent<HeroModel>() != null)
                {
                    col.transform.GetComponent<HeroModel>().CmdTakeDamage(damage);
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
	private void CmdEffect ()
	{
		GameObject effect = Instantiate(impactFX);
		effect.transform.position = transform.position;
		NetworkServer.Spawn(effect);
	}
}
