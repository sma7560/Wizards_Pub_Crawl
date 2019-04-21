using UnityEngine;
using UnityEngine.Networking;

/// <summary>
/// Projectile behaviour for Piercing Shot skill.
/// </summary>
public class FireBreath : BaseProjectile
{
	public GameObject impactFX;

	void OnParticleCollision(GameObject col){
		if (!isServer) return;
		Behaviour (col);
	}

	/// <summary>
	/// Initial damage when enemy/player touches the fire.
	/// </summary>
	public void Behaviour(GameObject col)
	{
		switch (col.transform.tag)
		{
		case "Enemy":
			// Damage enemy
			if (col.transform.GetComponent<EnemyModel>() != null)
			{
				col.transform.GetComponent<EnemyModel>().CmdTakeDamage(damage);
			}
			break;
		case "Player":
			// Damage player
			if (col.transform.GetComponent<HeroModel>() != null)
			{
				col.transform.GetComponent<HeroModel>().CmdTakeDamage(damage);
			}
			break;
		}
	}
}
