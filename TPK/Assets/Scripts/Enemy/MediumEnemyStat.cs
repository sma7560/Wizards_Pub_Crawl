using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MediumEnemyStat : EnemyStats
{
	// Use this for initialization
	private void Awake () {
        maxHealth = 50;
        SetCurrentHealth(maxHealth);
        damage.setValue(10);
        defence.setValue(1);
        movementSpeed.setValue(5);
    }
}
