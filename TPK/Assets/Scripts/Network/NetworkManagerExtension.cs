using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System.Net;
using System.Net.Sockets;
using System;

public class NetworkManagerExtension : NetworkManager
{
    public MatchManager matchManagerPrefab;
    private MatchManager matchManager;

    /// <summary>
    /// Setting up the host via getting the local IP address and using that as host address.
    /// </summary>
    public void StartUpHost()
    {
        // Set networking properties
        SetPort();
        networkAddress = GetLocalIPAddress();
        Debug.Log("Hosting on " + networkAddress);
        NetworkServer.Reset();
        NetworkManager.singleton.StartHost();
        matchManager = Instantiate(matchManagerPrefab).GetComponent<MatchManager>();
        NetworkServer.Spawn(matchManager.gameObject);   // Instantiate MatchManager on the server

        // Update MatchManager with new player
        if (!matchManager.AddPlayerToMatch())
        {
            Debug.Log("ISSUE WITH MATCHMANAGER! Could not add player. Num of players in MatchManager = " + matchManager.GetNumOfPlayers());
            return;
        }

        // Start the waiting room of pre-phase
        StartCoroutine(GameObject.Find("MatchManager(Clone)").GetComponent<PrephaseManager>().StartPrephaseWaitingRoom());
    }

    /// <summary>
    /// Join a game based on a designated IP address.
    /// </summary>
    public void JoinGame()
    {
        SetIPAddress();
        SetPort();
        NetworkManager.singleton.StartClient();
    }

    /// <summary>
    /// Sets up the IP address via looking for the input text. If none was submitted it defaults to localhost.
    /// </summary>
    private void SetIPAddress()
    {
        //Defaulting it to local host.
        string ipAddress = GameObject.Find("IPText").GetComponent<Text>().text;
        if (ipAddress == null) ipAddress = "localhost";

        NetworkManager.singleton.networkAddress = ipAddress;
    }

    /// <summary>
    /// Get host IP Address
    /// </summary>
    /// <returns>Returns local IP address</returns>
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

    /// <summary>
    /// Setting up the port for the game.
    /// </summary>
    private void SetPort()
    {
        NetworkManager.singleton.networkPort = 7777;
    }
}
