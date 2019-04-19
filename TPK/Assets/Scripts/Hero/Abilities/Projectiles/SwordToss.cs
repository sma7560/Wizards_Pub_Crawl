using UnityEngine;
using UnityEngine.Networking;

/// <summary>
/// Projectile behaviour for Sword Toss skill.
/// </summary>
public class SwordToss : BaseProjectile
{
	public GameObject impactFX;
	public GameObject SwordToss2;
	public Skill ST;

    /// <summary>
    /// Destroy self after collision and damage is dealt.
    /// </summary>
    public override void Behaviour(Collision col)
    {
        base.Behaviour(col);
		Vector3 colPos = col.transform.position;

        switch (col.collider.tag)
        {
            case "Enemy":
                // Damage enemy
                if (col.transform.GetComponent<EnemyModel>() != null)
                {
                    col.transform.GetComponent<EnemyModel>().CmdTakeDamage(damage);
                }
				CmdEffect ();

				CmdCastProjectile(ST.skillRange, damage, ST.projectileSpeed, ST.projectilePrefabIndex, colPos);
				//CmdCastProjectile(ST.skillRange, damage, ST.projectileSpeed, ST.projectilePrefabIndex, colPos);
				break;
            case "Player":
                // Damage player
                if (col.transform.GetComponent<HeroModel>() != null)
                {
                    col.transform.GetComponent<HeroModel>().CmdTakeDamage(damage);
                }
				CmdEffect ();

				CmdCastProjectile(ST.skillRange, damage, ST.projectileSpeed, ST.projectilePrefabIndex, colPos);
				//CmdCastProjectile(ST.skillRange, damage, ST.projectileSpeed, ST.projectilePrefabIndex, colPos);
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

	/// <summary>
	/// Casts the projectile.
	/// </summary>
	/// <param name="pindex">Index of the desired projectile in the list of projectiles.</param>
	[Command]
	private void CmdCastProjectile(float range, int damage, float speed, int pindex, Vector3 target)
	{

		Vector3 position = new Vector3(target.x + transform.forward.x * 2f, transform.position.y, target.z + transform.forward.z * 2f);
		GameObject bolt = Instantiate(SwordToss2, position, Quaternion.Euler(0, Random.Range(0, 360), 0));

		NetworkServer.Spawn(bolt);

		bolt.GetComponent<Rigidbody>().velocity = bolt.transform.forward * speed;
		bolt.GetComponent<BaseProjectile>().SetProjectileParams(range, damage);

		Destroy(bolt, range);
	}
}
