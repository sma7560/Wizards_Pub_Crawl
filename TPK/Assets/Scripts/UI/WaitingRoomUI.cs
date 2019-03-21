using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class WaitingRoomUI : MonoBehaviour
{
    // Managers
    private MatchManager matchManager;

    // Text elements
    private TextMeshProUGUI currentNumPlayers;
    private TextMeshProUGUI hostIP;

    /// <summary>
    /// Initialize variables.
    /// </summary>
    void Start()
    {
        // Initialize managers
        matchManager = GameObject.FindGameObjectWithTag("MatchManager").GetComponent<MatchManager>();

        // Initialize text
        currentNumPlayers = GameObject.Find("CurrentNumOfPlayersText").GetComponent<TextMeshProUGUI>();
        hostIP = GameObject.Find("IPAddressText").GetComponent<TextMeshProUGUI>();

        // Update the UI
        UpdateNumOfPlayers();
        UpdateHostIP();
    }

    // Update is called once per frame
    void Update()
    {
        UpdateNumOfPlayers();
    }

    /// <summary>
    /// Updates the "Number of the players connected" text in UI.
    /// </summary>
    private void UpdateNumOfPlayers()
    {
        currentNumPlayers.text = matchManager.GetNumOfPlayers().ToString() + " out of " + matchManager.GetMaxPlayers();
    }

    /// <summary>
    /// Updates the host IP text in UI.
    /// </summary>
    private void UpdateHostIP()
    {
        hostIP.text = NetworkManagerExtension.GetLocalIPAddress();
    }
}
