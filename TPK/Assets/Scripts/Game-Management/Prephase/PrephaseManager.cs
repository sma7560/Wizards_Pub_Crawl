using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Networking;

/// <summary>
/// Includes logic related to the Pre-phase game state.
/// </summary>
public class PrephaseManager : NetworkBehaviour
{
    public enum PrephaseState
    {
        WaitingForPlayers,
        RoomFull,
        NotActive
    }

    public GameObject prephaseUI;       // Prephase UI prefab object to be used during prephase

    private MatchManager matchManager;  // MatchManager to get current num of players
    public PrephaseState state;        // current status of the prephase
    private int countdown;              // countdown timer of time left in prephase stage; -1 when prephase is not active

    /// <summary>
    /// Initialize variables.
    /// </summary>
    void Start()
    {
        matchManager = GetComponent<MatchManager>();
        state = PrephaseState.NotActive;
        countdown = -1;
    }

    /// <summary>
    /// Update is called once per frame
    /// </summary>
    void Update()
    {

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
        // Check if current number of players in the match have reached the maximum number
        if (matchManager.GetNumOfPlayers() == matchManager.maxPlayers)
        {
            // Start the prephase
            state = PrephaseState.RoomFull;
            StartCoroutine(DecreaseCountdownTimer());   // Start the prephase countdown
        }
    }

    /// <summary>
    /// Starts the waiting room of pre-phase, in which we wait for players to join.
    /// </summary>
    public void StartPrephaseWaitingRoom()
    {
        state = PrephaseState.WaitingForPlayers;
        countdown = 300;
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
        state = PrephaseState.NotActive;
        countdown = -1;                 // set countdown back to default of -1 when prephase is not active
        prephaseUI.SetActive(false);    // disable pre-phase UI
    }

    /// <summary>
    /// Coroutine which decrements the prephase countdown by 1 per second.
    /// </summary>
    private IEnumerator DecreaseCountdownTimer()
    {
        while (countdown > 0)
        {
            // Update pre-phase UI countdown element
            PrephaseUI prephaseUI = GameObject.FindGameObjectWithTag("PrephaseUI").GetComponent<PrephaseUI>();
            prephaseUI.UpdateTimeLeftUI();

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
