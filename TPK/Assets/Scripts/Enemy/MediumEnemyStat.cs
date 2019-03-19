using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Inherits from enemy stats. sets up stats for medium sized enemies.
/// </summary>
public class MediumEnemyStat : EnemyStats
{
	// Use this for initialization
	private void Awake () {
        maxHealth = 50;
        SetCurrentHealth(maxHealth);
        damage.setValue(10);
        defence.setValue(1);
        movementSpeed.setValue(4);
        idleRange = 9;
        idleHowOftenDirectionChanged = 5;
    }
}
