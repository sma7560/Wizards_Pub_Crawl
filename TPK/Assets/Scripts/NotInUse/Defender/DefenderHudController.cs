using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DefenderHudController : MonoBehaviour {

	public TextMeshProUGUI textAction;
	public TextMeshProUGUI textMoney;
	public TextMeshProUGUI textEnergy;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		try{
			GameObject defender = GameObject.FindGameObjectWithTag("DefenderCamera");	//finding the main defender script

			if(defender.GetComponent<DefenderBehaviour>().mode == DefenderBehaviour.defenderMode.spawnMonster){		//checking the mode
				textAction.text = "PLACING MONSTER";																	//updating text
			} else if(defender.GetComponent<DefenderBehaviour>().mode == DefenderBehaviour.defenderMode.spawnTrap){	
				textAction.text = "PLACING TRAP";
			}

			//update energy and money
			textEnergy.text = defender.GetComponent<DefenderBehaviour>().energy + " Energy";
			textMoney.text = defender.GetComponent<DefenderBehaviour>().money + " GP";

		}catch(System.NullReferenceException){		//in case it can't find the GameObject
			gameObject.SetActive(false);
		}
	}
}
