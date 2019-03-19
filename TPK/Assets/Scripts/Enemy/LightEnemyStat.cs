using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Inherits from enemy stats. sets up stats for small fast enemies.
/// </summary>
public class LightEnemyStat : EnemyStats
{
    // Use this for initialization
    private void Awake()
    {
        maxHealth = 30;
        SetCurrentHealth(maxHealth);
        damage.setValue(5);
        defence.setValue(0);
        movementSpeed.setValue(6);
        idleRange = 14;
        idleHowOftenDirectionChanged = 6;
    }
}