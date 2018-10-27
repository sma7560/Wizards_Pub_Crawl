using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class NetworkManagerExtension : NetworkManager{

    public void StartUpHost() {
        SetPort();
        NetworkServer.Reset();
        NetworkManager.singleton.StartHost();
    }

    public void JoinGame() {
        SetIPAddress();
        SetPort();
        NetworkManager.singleton.StartClient();
    }

    private void SetIPAddress() {
        //Defaulting it to local host.
        string ipAddress = GameObject.Find("IPText").GetComponent<Text>().text;
        if (ipAddress == null) ipAddress = "localhost";

        NetworkManager.singleton.networkAddress = ipAddress;

    }
    private void SetPort()
    {
        NetworkManager.singleton.networkPort = 7777;
    }
}
