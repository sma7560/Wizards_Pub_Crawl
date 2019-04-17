using UnityEngine;
using UnityEngine.Networking;

/// <summary>
/// Projectile behaviour for Piercing Shot skill.
/// </summary>
public class PiercingShot : BaseProjectile
{
	public GameObject impactFX;

    /// <summary>
    /// The piercing shot only destroys on things that are not walls/environment objects.
    /// </summary>
    public override void Behaviour(Collider col)
    {
        switch (col.transform.tag)
        {
			case "Enemy":
	                // Damage enemy
				if (col.transform.GetComponent<EnemyModel> () != null)
				{
					col.transform.GetComponent<EnemyModel> ().CmdTakeDamage (damage);
				}
				CmdEffect ();
                break;
			case "Player":
	                // Damage player
				if (col.transform.GetComponent<HeroModel> () != null)
				{
					col.transform.GetComponent<HeroModel> ().CmdTakeDamage (damage);
				}
				CmdEffect ();
                break;
            case "Projectile":
                // Pierces projectiles and shields
                break;
            default:
                Destroy(gameObject);
                break;
        }
    }

	/// <summary>
	/// Plays the explosion effect.
	/// </summary>
	[Command]
	private void CmdEffect()
	{
		GameObject effect = Instantiate(impactFX);
		effect.transform.position = transform.position;
		NetworkServer.Spawn(effect);
	}
}
