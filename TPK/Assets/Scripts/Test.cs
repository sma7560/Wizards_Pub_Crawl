using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class Test : MonoBehaviour {

    public string leveltoload;
	// Use this for initialization
	void Start () {
        Resources.FindObjectsOfTypeAll<GameObject>();
        string wow = GameObject.Find("wow").name;
        if (wow == null) Debug.Log("Not here!");
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    public void ReloadScene() {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
