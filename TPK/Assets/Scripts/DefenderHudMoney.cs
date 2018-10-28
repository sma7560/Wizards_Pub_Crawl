using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DefenderHudMoney : MonoBehaviour {

	//This script updates Defender UI to show how much gold the defender has

	TextMeshProUGUI textmesh;

	// Use this for initialization
	void Start () {
		textmesh = GetComponent<TextMeshProUGUI> ();
	}

	// Update is called once per frame
	void Update () {
		try{
			GameObject defender = GameObject.FindGameObjectWithTag("DefenderCamera");
			textmesh.text = defender.GetComponent<DefenderBehaviour>().money + " GP";
		}catch(System.NullReferenceException){
			gameObject.SetActive(false);
		}
	}
}
