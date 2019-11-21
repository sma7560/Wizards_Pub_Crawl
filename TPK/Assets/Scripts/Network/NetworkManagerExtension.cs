using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System.Net;
using System.Net.Sockets;
using System;
using TMPro;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

/// <summary>
/// Extends the default functionality of NetworkManager.
/// </summary>
public class NetworkManagerExtension : NetworkManager
{
    public GameObject matchManagerPrefab;
    private MatchManager matchManager;
    private PrephaseManager prephaseManager;

    private readonly int portNumber = 7777;
    private bool doNotDisplayTimeoutError = false;  // will no longer display timeout error if coroutine is running

    /// <summary>
    /// Sets up the host via getting the local IP address and using that as host address.
    /// </summary>
    public void StartUpHost()
    {
        // Check if host already exists
        if (DoesHostExist())
        {
            CreateErrorPopup("New Match Error", "Could not create a new match, as another match is already being hosted on this IP.");
            GameObject.Find("HostButton").GetComponent<Button>().interactable = true;
            GameObject.Find("JoinMatchButton").GetComponent<Button>().interactable = true;
            return;
        }

        // Create MatchManager object on server
        GameObject managerPrefab = Instantiate(matchManagerPrefab);
        matchManager = managerPrefab.GetComponent<MatchManager>();
        prephaseManager = managerPrefab.GetComponent<PrephaseManager>();

        // Start host in network
        SetPort();
        networkAddress = GetLocalIPAddress();
        NetworkServer.Reset();
        NetworkClient client = StartHost();
        NetworkServer.Spawn(matchManager.gameObject);

        // Update MatchManager with new player
        if (!matchManager.AddPlayerToMatch(client.connection))
        {
            Debug.Log("ERROR: MatchManager failed to add player. Current num players in MatchManager = " + matchManager.GetNumOfPlayers());
            return;
        }

        // Start the waiting room of pre-phase
        StartCoroutine(prephaseManager.StartPrephaseWaitingRoom());
    }

	public void StartUpSolo()
	{

		// Create MatchManager object on server
		GameObject managerPrefab = Instantiate(matchManagerPrefab);
		matchManager = managerPrefab.GetComponent<MatchManager>();
		prephaseManager = managerPrefab.GetComponent<PrephaseManager>();
		matchManager.SoloGame ();

		// Start host in network
		SetPort();
		networkAddress = GetLocalIPAddress();
		NetworkServer.Reset();
		NetworkClient client = StartHost();
		NetworkServer.Spawn(matchManager.gameObject);

		// Update MatchManager with new player
		if (!matchManager.AddPlayerToMatch(client.connection))
		{
			Debug.Log("ERROR: MatchManager failed to add player. Current num players in MatchManager = " + matchManager.GetNumOfPlayers());
			return;
		}
			
		// Start the waiting room of pre-phase
		StartCoroutine(prephaseManager.StartPrephaseWaitingRoom());

		Debug.Log ("here");

	}

    /// <summary>
    /// Join a game based on a designated IP address.
    /// </summary>
    public void JoinGame()
    {
        // Join the match
        SetIPAddress();
        SetPort();
        StartClient();

        // Check for timeout
        StartCoroutine(CheckForTimeout());
    }

    /// <summary>
    /// Called on the server when a new client connects.
    /// </summary>
    public override void OnServerConnect(NetworkConnection conn)
    {
        base.OnServerConnect(conn);

        doNotDisplayTimeoutError = true;
        matchManager.AddPlayerToMatch(conn);    // Add a player to MatchManager
        prephaseManager.UpdatePrephase();       // Send new player information to PrephaseManager
    }

    /// <summary>
    /// Called on clients when a new client connects.
    /// </summary>
    public override void OnClientConnect(NetworkConnection conn)
    {
        base.OnClientConnect(conn);

        doNotDisplayTimeoutError = true;
    }

    /// <summary>
    /// Called on the server when a player disconnects.
    /// </summary>
    public override void OnServerDisconnect(NetworkConnection conn)
    {
        base.OnServerDisconnect(conn);

        doNotDisplayTimeoutError = true;
        matchManager.RemovePlayerFromMatch(conn);
        prephaseManager.UpdatePrephase();   // Send new player information to PrephaseManager
        if (!matchManager.HasMatchEnded())
        {
            CreateErrorPopup("Disconnected", "You have been disconnected from the match.");
        }
        RemoveAllAnnouncements();
    }

    /// <summary>
    /// Called on clients when a player disconnects.
    /// </summary>
    public override void OnClientDisconnect(NetworkConnection conn)
    {
        doNotDisplayTimeoutError = true;
        RemoveAllAnnouncements();
        if (!matchManager.HasMatchEnded())
        {
            CreateErrorPopup("Disconnected", "You have been disconnected from the match.");
        }
    }

    /// <returns>
    /// Returns the local IP address.
    /// </returns>
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
    /// Destroys all announcement objects.
    /// </summary>
    public void RemoveAllAnnouncements()
    {
        GameObject[] announcements = GameObject.FindGameObjectsWithTag("Announcement");
        foreach (GameObject announcement in announcements)
        {
            Destroy(announcement);
        }
    }

    public void SetDoNotDisplayTimeoutError(bool shouldNotDisplay)
    {
        doNotDisplayTimeoutError = shouldNotDisplay;
    }

    /// <summary>
    /// Sets up the IP address via looking for the input text.
    /// If none was submitted it defaults to localhost.
    /// </summary>
    private void SetIPAddress()
    {
        // Get the IP address text box text
        string ipAddress = GameObject.Find("IPText").GetComponent<Text>().text;

        //Defaulting IP address to "localhost"
        if (ipAddress.Length == 0)
        {
            ipAddress = "localhost";
        }

        // Set the IP address
        networkAddress = ipAddress;
    }

    /// <summary>
    /// Setting up the port for the game.
    /// </summary>
    private void SetPort()
    {
        networkPort = portNumber;
    }

    /// <returns>
    /// Returns whether or not a hosts already exists.
    /// </returns>
    private bool DoesHostExist()
    {
        SetPort();
        NetworkTransport.Init();
        HostTopology topology = new HostTopology(new ConnectionConfig(), 1);
        int hostId = NetworkTransport.AddHost(topology, portNumber);
        if (hostId >= 0) NetworkTransport.RemoveHost(hostId);

        // If AddHost() returns a negative number, then it failed; host already exists
        if (hostId < 0)
        {
            return true;
        }

        return false;
    }

    /// <summary>
    /// Instantiates an error popup if one does not already exist.
    /// </summary>
    /// <param name="title">Title of the error popup.</param>
    /// <param name="description">Description of the error.</param>
    private void CreateErrorPopup(string title, string description)
    {
        // Do nothing if another error popup is already shown
        if (GameObject.FindGameObjectWithTag("Error") != null) return;

        // Instantiate error popup and set the title and description
        GameObject error = Instantiate(Resources.Load("Menu&UI Prefabs/ErrorPopup")) as GameObject;
        DontDestroyOnLoad(error);
        GameObject.Find("ErrorTitle").GetComponent<TextMeshProUGUI>().text = title;
        GameObject.Find("ErrorText").GetComponent<TextMeshProUGUI>().text = description;
    }

    /// <summary>
    /// Called whenever the player clicks "Join Match".
    /// Checks if the player is still trying to join a match. If so, times out and displays an error popup.
    /// </summary>
    /// <param name="title">Title of the timeout error popup.</param>
    /// <param name="description">Description of the timeout error popup.</param>
    private IEnumerator CheckForTimeout()
    {
        doNotDisplayTimeoutError = false;

        // Setup buttons
        GameObject.Find("JoinMatchText").GetComponent<TextMeshProUGUI>().text = "SEARCHING...";
        GameObject.Find("JoinMatchButton").GetComponent<Button>().interactable = false;
        EventSystem.current.SetSelectedGameObject(null);
        GameObject backButton = GameObject.Find("BackButton");
        backButton.SetActive(false);

        // Wait for timeout period
        yield return new WaitForSeconds(5);

        if (!SceneManager.GetActiveScene().name.Equals(onlineScene) && !doNotDisplayTimeoutError)
        {
            // Client did not load the online scene; instantiate timeout error popup
            CreateErrorPopup("Timeout Error", "Timeout has occurred while trying to connect to a match; no match has been found. Please create a new match first.");

            // Reset buttons
            GameObject.Find("JoinMatchButton").GetComponent<Button>().interactable = true;
            GameObject.Find("JoinMatchText").GetComponent<TextMeshProUGUI>().text = "JOIN MATCH";
            backButton.SetActive(true);
        }
    }
}