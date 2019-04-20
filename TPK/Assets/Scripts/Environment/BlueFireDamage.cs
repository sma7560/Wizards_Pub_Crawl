using UnityEngine;
using UnityEngine.Networking;

public class BlueFireDamage : NetworkBehaviour 
{
	private int damage = 30;
	private readonly float dmgDelay = 0.1f;     // cooldown of damage done by fire
	private float nextActiveTime;               // the next time where flame will do damage

	void Awake()
	{
		damage = 30;
		nextActiveTime = 0;
	}

	void Update()
	{
		// Update next active time
		if (Time.time > nextActiveTime)
		{
			nextActiveTime = Time.time + dmgDelay;
		}
	}

	//Damage for the first time they enter
	void OnTriggerEnter(Collider col)
	{
		Behaviour (col);
	}

	/// <summary>
	/// Damage over time if enemy/player stays within the firejet.
	/// </summary>
	void OnTriggerStay(Collider col)
	{
		if (Time.time < nextActiveTime) return;

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

	/// <summary>
	/// Initial damage when enemy/player touches flame wall.
	/// </summary>
	public void Behaviour(Collider col)
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
