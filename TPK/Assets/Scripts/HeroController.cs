using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

//Attacker Controller Script for controlling character.
public class HeroController : NetworkBehaviour {


    public GameObject heroCam;
    private GameObject cam;

	// Use this for initialization
	void Start () {
        if (!hasAuthority) return;
        startCamera();
    }
	
	// Update is called once per frame
	void Update () {
        ////This function runs on all hero's
        if (!hasAuthority) return;
        var x = Input.GetAxis("Horizontal") * Time.deltaTime * 5.0f;
        var z = Input.GetAxis("Vertical") * Time.deltaTime * 5.0f;
        transform.Translate(x, 0, z);

    }

    private void startCamera() {
        GameObject.FindGameObjectWithTag("MainCamera").SetActive(false);
        cam = Instantiate(heroCam);
        cam.GetComponent<HeroCameraController>().setTarget(this.transform);
    }
}
