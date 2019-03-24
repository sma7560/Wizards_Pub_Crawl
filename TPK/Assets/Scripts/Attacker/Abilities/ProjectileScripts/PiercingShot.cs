using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PiercingShot : MonoBehaviour {

	public float range = 2f;
	public int damage = 20;
	public DamageType damageType = DamageType.magical;

	public void SetProjectileParams(float r, int dmg, DamageType dtype)
	{
		range = r;
		damage = dmg;
		damageType = dtype;
	}

	void OnTriggerEnter(Collider col)
	{

		switch (col.transform.tag)
		{
			case "Enemy":
				if (col.transform.GetComponent<EnemyStats>())
					{
						// This will change.
					col.transform.GetComponent<EnemyStats>().CmdTakeDamage(damage);
					}
				break;
			case "Player":
				if (col.transform.GetComponent<HeroModel>())
					{
					col.transform.GetComponent<HeroModel>().CmdTakeDamage(damage, damageType);
					}
				// This means you can shoot yourself... so far.
				break;
			default:
				Destroy (gameObject);
				break;
		}
	}
}
