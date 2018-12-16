using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUIController : MonoBehaviour {

    public TextMeshProUGUI ipAddressBox;
    public TextMeshProUGUI timeBox;
    private NetworkManagerExtension networkManager;


    // Use this for initialization
    void Start () {
        networkManager = GameObject.Find("NetworkManagerV2").GetComponent<NetworkManagerExtension>();
        ipAddressBox.SetText(networkManager.getNetworkAddress());
    }
	
	// Update is called once per frame
	void Update () {
       
	}
}
