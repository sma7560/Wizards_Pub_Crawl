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
    
    /// <returns>
    /// Returns the score.
    /// </returns>
    public int GetScore()
    {
        return score;
    }

    /// <summary>
    /// Increment the score by a value.
    /// </summary>
    /// <param name="increment">Value to increment the score by.</param>
    public void IncreaseScore(int increment)
    {
        score += increment;
    }

    /// <summary>
    /// Resets score to 0.
    /// </summary>
    public void ResetScore()
    {
        score = 0;
    }

}
