using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

public class ToggleFireJet : NetworkBehaviour 
{
	private int damage;
	private readonly float dmgDelay = 0.1f;     // cooldown of damage done by fire
	private float nextActiveTime;               // the next time where flame will do damage

	private ParticleSystem ps;
	private Collider c;
	private Light l;
	ParticleSystem.EmissionModule em;


	private bool firing = true;

	void Start()
	{
		ps = GetComponent<ParticleSystem> ();
		c = GetComponent<CapsuleCollider> ();
		l = GetComponent<Light> ();
		em = ps.emission;

		damage = 10;
		nextActiveTime = 0;

		//50/50 chance it's enabled at startup
		if (Random.Range (0, 2) == 0)
			firing = true;
		else {
			firing = false;
			em.enabled = false;
			l.enabled = false;
			c.enabled = false;
		}
			
		StartCoroutine(toggleFire());
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
		Behaviour (col);
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

	private IEnumerator toggleFire(){
		

		while (true) 
		{
			yield return new WaitForSeconds (3);

			firing = !firing;

			if (!firing) {
				em.enabled = false;
				l.enabled = false;
				c.enabled = false;
			} else {
				em.enabled = true;
				l.enabled = true;
				c.enabled = true;
			}

		}
	}

}
