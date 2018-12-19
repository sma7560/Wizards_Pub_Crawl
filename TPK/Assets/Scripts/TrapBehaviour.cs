using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapBehaviour : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnTriggerEnter(Collider col){
		if (col.transform.tag == "Player"){
            CharacterStats player = col.GetComponent<CharacterStats>();
            if (player == null) return;

            //col.GetComponent<CharacterStats>().TakeDamage(50);
        }
	}
}
