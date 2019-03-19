using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Inherits from enemy stats. sets up stats for medium sized enemies.
/// </summary>
public class HeavyEnemyStat : EnemyStats
{
    // Use this for initialization
    private void Awake()
    {
        maxHealth = 80;
        SetCurrentHealth(maxHealth);
        damage.setValue(25);
        defence.setValue(3);
        movementSpeed.setValue(2);
        idleRange = 8;
        idleHowOftenDirectionChanged = 6;
    }
}