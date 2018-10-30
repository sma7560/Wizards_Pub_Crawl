using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System.Net;
using System.Net.Sockets;
using System;

public class NetworkManagerExtension : NetworkManager{

    public void StartUpHost() {
        SetPort();
        networkAddress = GetLocalIPAddress();
        Debug.Log("Hosting on " + networkAddress);
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

    //get host IP Address
    public static string GetLocalIPAddress()
    {
        var host = Dns.GetHostEntry(Dns.GetHostName());
        foreach (var ip in host.AddressList)
        {
            if (ip.AddressFamily == AddressFamily.InterNetwork)
            {
                return ip.ToString();
            }
        }
        throw new Exception("Local IP Address Not Found!");
    }

    private void SetPort()
    {
        NetworkManager.singleton.networkPort = 7777;
    }
}
