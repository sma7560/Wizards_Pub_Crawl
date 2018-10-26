using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefenderBehaviour : MonoBehaviour {

	public enum defenderMode {spawnMonster, spawnTrap};

	public GameObject monster;
	public GameObject trap;
	public int energy;
	public int money;

	public defenderMode mode;

	// Use this for initialization
	void Start () {
		energy = 1000;
		money = 1000;

		mode = defenderMode.spawnMonster;
	}
	
	// Update is called once per frame
	void Update () {

		//Defender Monster spawn script
		//Uses raycasting to select a tile to place a monster

		if (Input.GetMouseButtonUp (0)) {
			RaycastHit hit;
			Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);

			if (Physics.Raycast (ray, out hit, 200.0f) && hit.transform.tag == "Tile") {
				switch (mode) {
				case defenderMode.spawnMonster:
					Instantiate (monster, hit.transform);
					energy -= 100;
					break;
				case defenderMode.spawnTrap:
					Instantiate (trap, hit.transform);
					money -= 100;
					break;
				}
			}
			Debug.Log ("raycast hit " + hit.transform.name);
		}

		//switch spawn modes
		if (Input.GetKeyUp (KeyCode.E)) {
			if (mode == defenderMode.spawnMonster)
				mode = defenderMode.spawnTrap;
			else if (mode == defenderMode.spawnTrap)
				mode = defenderMode.spawnMonster;
		}
	}
}
