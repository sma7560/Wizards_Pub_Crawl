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
    public GameObject matchManagerPrefab;
    private MatchManager matchManager;
    private PrephaseManager prephaseManager;

    /// <summary>
    /// Setting up the host via getting the local IP address and using that as host address.
    /// </summary>
    public void StartUpHost()
    {
        // Create MatchManager object on server
        GameObject managerPrefab = Instantiate(matchManagerPrefab);
        matchManager = managerPrefab.GetComponent<MatchManager>();
        prephaseManager = managerPrefab.GetComponent<PrephaseManager>();

        // Set networking properties
        SetPort();
        networkAddress = GetLocalIPAddress();
        Debug.Log("Hosting on " + networkAddress);
        NetworkServer.Reset();
        NetworkClient client = NetworkManager.singleton.StartHost();
        NetworkServer.Spawn(matchManager.gameObject);   // Instantiate MatchManager on the server
        Debug.Log(client.connection);
        // Update MatchManager with new player
        if (!matchManager.AddPlayerToMatch(client.connection))
        {
            Debug.Log("MatchManager failed to add player. Current num players in MatchManager = " + matchManager.GetNumOfPlayers());
            return;
        }

        // Start the waiting room of pre-phase
        StartCoroutine(prephaseManager.StartPrephaseWaitingRoom());
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

    /// <summary>
    /// Called when a new client connects to the server.
    /// </summary>
    public override void OnServerConnect(NetworkConnection conn)
    {
        base.OnServerConnect(conn);

        // If the new connection is a client, add a player to MatchManager
        if (!matchManager.AddPlayerToMatch(conn))
        {
            // NOTE: will fail for server's connect, when numOfPlayers = 0 (this is expected)
            Debug.Log("MatchManager failed to add player. Current num players in MatchManager = " + matchManager.GetNumOfPlayers());
        }

        // Send new player information to PrephaseManager
        prephaseManager.UpdatePrephase();
    }

    /// <summary>
    /// Called when a client disconnects from the server.
    /// </summary>
    public override void OnServerDisconnect(NetworkConnection conn)
    {
        base.OnServerDisconnect(conn);

        if (!matchManager.RemovePlayerFromMatch(conn))
        {
            Debug.Log("MatchManager failed to remove player. Current num players in MatchManager = " + matchManager.GetNumOfPlayers());
        }

        // Send new player information to PrephaseManager
        prephaseManager.UpdatePrephase();
    }
}