using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DefenderHudMoney : MonoBehaviour {


	TextMeshProUGUI textmesh;

	// Use this for initialization
	void Start () {
		textmesh = GetComponent<TextMeshProUGUI> ();
	}

	// Update is called once per frame
	void Update () {
		try{
			GameObject defender = GameObject.FindGameObjectWithTag("MainCamera");
			textmesh.text = defender.GetComponent<DefenderBehaviour>().money + " GP";
		}catch(System.NullReferenceException){
			gameObject.SetActive(false);
		}
	}
}
