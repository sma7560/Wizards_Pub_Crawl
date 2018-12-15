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
    private int countdown;              // countdown timer of time left in prephase stage; -1 when prephase is not active

    // Use this for initialization
    void Start()
    {
        // Initialize variables
        matchManager = GameObject.Find("NetworkManagerV2").GetComponent<MatchManager>();
        state = PrephaseState.NotStarted;
        countdown = -1;
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
        countdown = 300;
        yield return null;
    }

    IEnumerator StartPrephaseCountdown()
    {

        yield return null;
    }

    // Decrements the countdown by 1 per second
    private IEnumerator DecreaseCountdownTimer()
    {
        for (int i = countdown; i > 0; i--)
        {
            countdown--;
            yield return new WaitForSeconds(1);
        }
    }
}
