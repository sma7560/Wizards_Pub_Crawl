using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DefenderHudAction : MonoBehaviour {
		

	TextMeshProUGUI textmesh;

	// Use this for initialization
	void Start () {
		textmesh = GetComponent<TextMeshProUGUI> ();
	}
	
	// Update is called once per frame
	void Update () {
		try{
			GameObject defender = GameObject.FindGameObjectWithTag("MainCamera");

			if(defender.GetComponent<DefenderBehaviour>().mode == DefenderBehaviour.defenderMode.spawnMonster){
				textmesh.text = "PLACING MONSTER";
			} else if(defender.GetComponent<DefenderBehaviour>().mode == DefenderBehaviour.defenderMode.spawnTrap){
				textmesh.text = "PLACING TRAP";
			}
				
		}catch(System.NullReferenceException){
			gameObject.SetActive(false);
		}
	}
}
