using UnityEngine;
using UnityEngine.Networking;


// This script disables all uneeded components from the local player.
public class HeroSetup : NetworkBehaviour {

    [SerializeField]
    Behaviour[] compsToDisable;
	// Use this for initialization
	void Start () {
        Debug.Log("Local Player: " + hasAuthority);
        if (!isLocalPlayer)
        {
            for (int i = 0; i < compsToDisable.Length; i++) {
                compsToDisable[i].enabled = false;
            }
        }
        else
        {

        }

		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
