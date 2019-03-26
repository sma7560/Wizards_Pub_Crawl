using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PiercingShot : BaseProjectile {

	public override void Behaviour(Collider col)
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
