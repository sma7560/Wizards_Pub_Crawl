using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DefenderHudAction : MonoBehaviour {

	//This script updates Defender UI to show what action the defender can perform

	TextMeshProUGUI textmesh;

	// Use this for initialization
	void Start () {
		textmesh = GetComponent<TextMeshProUGUI> ();
	}
	
	// Update is called once per frame
	void Update () {
		try{
			GameObject defender = GameObject.FindGameObjectWithTag("MainCamera");	//finding the main defender script

			if(defender.GetComponent<DefenderBehaviour>().mode == DefenderBehaviour.defenderMode.spawnMonster){		//checking the mode
				textmesh.text = "PLACING MONSTER";																	//updating text
			} else if(defender.GetComponent<DefenderBehaviour>().mode == DefenderBehaviour.defenderMode.spawnTrap){	
				textmesh.text = "PLACING TRAP";
			}
				
		}catch(System.NullReferenceException){		//in case it can't find the GameObject
			gameObject.SetActive(false);
		}
	}
}
