using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System.Net;
using System.Net.Sockets;
using System;

public class NetworkManagerExtension : NetworkManager{

    // Setting up the host via getting the local IP address and using that as host address.
    public void StartUpHost() {
        SetPort();
        networkAddress = GetLocalIPAddress();
        Debug.Log("Hosting on " + networkAddress);
        NetworkServer.Reset();
        NetworkManager.singleton.StartHost();
    }

    // Join a game based on a designated IP address.
    public void JoinGame() {
        SetIPAddress();
        SetPort();
        NetworkManager.singleton.StartClient();
    }
    //Sets up the IP address via looking for the input text. If none was submitted it defaults to localhost.
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

    //Setting up the port for the game.
    private void SetPort()
    {
        NetworkManager.singleton.networkPort = 7777;
    }
}
