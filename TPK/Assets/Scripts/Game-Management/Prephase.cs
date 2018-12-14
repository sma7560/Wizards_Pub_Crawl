using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Prephase : MonoBehaviour
{
    private enum PrephaseState
    {
        WaitingForPlayers,
        RoomFull,
        NotStarted
    }

    public GameObject prephaseUI;       // Prephase UI prefab object to be used during prephase

    private MatchManager matchManager;  // MatchManager to get current num of players
    private PrephaseState state;        // current status of the prephase

    // Use this for initialization
    void Start()
    {
        // Initialize variables
        matchManager = GameObject.Find("NetworkManagerV2").GetComponent<MatchManager>();
        state = PrephaseState.NotStarted;
    }

    // Update is called once per frame
    void Update()
    {

    }

    IEnumerator UpdatePrephaseStatus()
    {
        yield return null;
    }

    IEnumerator StartPrephase()
    {
        state = PrephaseState.WaitingForPlayers;
        Instantiate(prephaseUI);    // Start the prephase UI
        yield return null;
    }
}
