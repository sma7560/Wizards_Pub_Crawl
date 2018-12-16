using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class PlayerUIController : MonoBehaviour
{
    public TextMeshProUGUI timeBox;
    
    private MatchManager matchManager;

    /// <summary>
    /// Initialize variables.
    /// </summary>
    void Start()
    {
        matchManager = GameObject.FindGameObjectWithTag("MatchManager").GetComponent<MatchManager>();
    }

    /// <summary>
    /// Update UI elements every frame.
    /// </summary>
    void Update()
    {
        UpdateTimeLeftUI(matchManager.GetTimeLeftInMatch());
    }

    /// <summary>
    /// Updates the match time left in player UI.
    /// </summary>
    /// <param name="timeLeft">Seconds left in the match.</param>
    public void UpdateTimeLeftUI(float timeLeft)
    {
        int minuteLeft = Mathf.FloorToInt(timeLeft / 60F);
        int secondLeft = Mathf.FloorToInt(timeLeft - minuteLeft * 60);

        if (secondLeft >= 10)
        {
            timeBox.SetText(minuteLeft.ToString() + " : " + secondLeft.ToString());
        }
        else
        {
            timeBox.SetText(minuteLeft.ToString() + " : 0" + secondLeft.ToString());
        }
    }
}
