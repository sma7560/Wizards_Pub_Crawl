using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlameWall : BaseProjectile {

    private float dmgDelay = 0.5f;
    private float nextActiveTime = 0;
	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		if (Time.time > nextActiveTime)
            nextActiveTime = Time.time + dmgDelay;
    }
    void OnTriggerStay(Collider col)
    {
        if (Time.time < nextActiveTime) return;
        switch (col.transform.tag)
        {
            case "Enemy":
                if (col.transform.GetComponent<EnemyModel>())
                {
                    // This will change.
                    col.transform.GetComponent<EnemyModel>().CmdTakeDamage(damage);
                }
                break;
            case "Player":
                if (col.transform.GetComponent<HeroModel>())
                {
                    col.transform.GetComponent<HeroModel>().CmdTakeDamage(damage, damageType);
                }
                // This means you can shoot yourself... so far.
                break;
        }
    }

    public override void Behaviour(Collider col)
    {

        switch (col.transform.tag)
        {
            case "Enemy":
                if (col.transform.GetComponent<EnemyModel>())
                {
                    // This will change.
                    col.transform.GetComponent<EnemyModel>().CmdTakeDamage(damage);
                }
                break;
            case "Player":
                if (col.transform.GetComponent<HeroModel>())
                {
                    col.transform.GetComponent<HeroModel>().CmdTakeDamage(damage, damageType);
                }
                // This means you can shoot yourself... so far.
                break;
        }
    }
}
