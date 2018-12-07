using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This class holds information regarding the score of the player.
/// </summary>
public class Score
{
    private int score;

    public Score()
    {
        // Initialize score to 0
        score = 0;
    }

    public int GetScore()
    {
        return score;
    }

    // Increment the score by a value
    public void IncreaseScore(int increment)
    {
        score += increment;
    }

}
