using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroDeathController : CharacterStats {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public override void Die()
    {
        base.Die();

        // Add enemy death animation here
        Destroy(gameObject);
        gameObject.GetComponent<HeroController>().KillMe();
    }

}
