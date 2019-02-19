using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Networking;

/// <summary>
/// Includes logic related to the Pre-phase game state.
/// Functions only run on the server.
/// </summary>
public class PrephaseManager : NetworkBehaviour
{
    public enum PrephaseState
    {
        WaitingForPlayers,
        RoomFull,
        NotActive
    }

    public GameObject prephaseUI;           // Prephase UI prefab object to be used during prephase

    private readonly int timeLimit = 30;    // value which the countdown starts from when activated
    private MatchManager matchManager;      // MatchManager to get current num of players
    [SyncVar] private PrephaseState state;  // current status of the prephase
    [SyncVar] private int countdown;        // countdown timer of time left in prephase stage; -1 when prephase is not active

    /// <summary>
    /// Initialize variables.
    /// </summary>
    void Start()
    {
        if (!isServer) return;

        matchManager = GameObject.FindGameObjectWithTag("MatchManager").GetComponent<MatchManager>();
        state = PrephaseState.NotActive;
        countdown = -1;
    }

    /// <summary>
    /// Get the current state of prephase.
    /// </summary>
    /// <returns>Returns the prephase state.</returns>
    public PrephaseState GetState()
    {
        return state;
    }

    /// <returns>
    /// Returns true if the game is currently in prephase stage; else returns false.
    /// </returns>
    public bool IsCurrentlyInPrephase()
    {
        if (state == PrephaseManager.PrephaseState.WaitingForPlayers ||
            state == PrephaseManager.PrephaseState.RoomFull)
        {
            return true;
        }

        return false;
    }

    /// <summary>
    /// Checks the current status of the game and updates the prephase accordingly.
    /// </summary>
    public void UpdatePrephase()
    {
        if (!isServer) return;

        Debug.Log("PREPHASE: UpdatePrephase called");

        // Check if current number of players in the match have reached the maximum number
        if (matchManager.GetNumOfPlayers() >= matchManager.GetMaxPlayers())
        {
            // Start the prephase
            state = PrephaseState.RoomFull;
            StartCoroutine(DecreaseCountdownTimer());   // Start the prephase countdown
        }

        // Exit match if a player disconnects
        if ((state == PrephaseState.RoomFull ||
             state == PrephaseState.NotActive) &&
             matchManager.GetNumOfPlayers() < matchManager.GetMaxPlayers())
        {
            GameObject.Find("EventSystem").GetComponent<DungeonController>().QuitMatch();
        }
    }

    /// <summary>
    /// Starts the waiting room of pre-phase, in which we wait for players to join.
    /// </summary>
    public IEnumerator StartPrephaseWaitingRoom()
    {
        if (!isServer) yield return null;

        Debug.Log("PREPHASE: StartPrephaseWaitingRoom() called");

        yield return new WaitForFixedUpdate();      // Need to wait for Start() function to finish
        state = PrephaseState.WaitingForPlayers;
        countdown = timeLimit;

        UpdatePrephase();
    }

    /// <returns>
    /// Returns the current value of the prephase countdown timer.
    /// </returns>
    public int GetCountdown()
    {
        return countdown;
    }

    /// <summary>
    /// Ends the pre-phase stage and moves to the dungeon stage
    /// </summary>
    private void EndPrephase()
    {
        if (!isServer) return;

        Debug.Log("Prephase ended");

        state = PrephaseState.NotActive;
        countdown = -1;     // set countdown back to default of -1 when prephase is not active
        StartCoroutine(matchManager.DecrementMatchTime());  // Start decrementing the match timer
        //start spawning monsters
        GameObject.FindGameObjectWithTag("MatchManager").GetComponent<DungeonEnemyManager>().StartSpawn();
    }

    /// <summary>
    /// Coroutine which decrements the prephase countdown by 1 per second.
    /// </summary>
    private IEnumerator DecreaseCountdownTimer()
    {
        if (!isServer) yield return null;

        while (countdown > 0)
        {
            // Update pre-phase UI countdown element
            if (GameObject.FindGameObjectWithTag("PrephaseUI") != null)
            {
                PrephaseUI prephaseUI = GameObject.FindGameObjectWithTag("PrephaseUI").GetComponent<PrephaseUI>();
                prephaseUI.UpdateTimeLeftUI();
            }

            // Wait for 1 second
            yield return new WaitForSeconds(1);

            // Decrement countdown
            countdown--;
        }

        if (countdown <= 0)
        {
            // If countdown timer reaches 0, end the prephase
            EndPrephase();
        }
    }
}
