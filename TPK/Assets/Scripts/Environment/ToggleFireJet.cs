using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

public class ToggleFireJet : NetworkBehaviour 
{
	private int damage;
	private readonly float dmgDelay = 0.1f;     // cooldown of damage done by fire
	private float nextActiveTime;               // the next time where flame will do damage

	private ParticleSystem ps;
	private Light l;
	ParticleSystem.EmissionModule em;
	private AudioSource a;


	private bool firing = true;

	void Start()
	{
		ps = GetComponent<ParticleSystem> ();
		l = GetComponent<Light> ();
		em = ps.emission;
		a = GetComponent<AudioSource> ();

		damage = 1;
		nextActiveTime = 0;

		//50/50 chance it's enabled at startup
		if (Random.Range (0, 2) == 0)
			firing = true;
		else 
			firing = false;
		
		fire (firing);
			
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

	void OnParticleCollision(GameObject col){
		
		Behaviour (col);
	}

	/// <summary>
	/// Initial damage when enemy/player touches flame wall.
	/// </summary>
	public void Behaviour(GameObject col)
	{
		if (!isServer)
			return;
		
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

	private void fire(bool on){
		em.enabled = on;
		l.enabled = on;
		a.enabled = on;
	}

	private IEnumerator toggleFire(){

		while (true) 
		{
			yield return new WaitForSeconds (3);
			firing = !firing;

			fire (firing);

		}
	}

}
