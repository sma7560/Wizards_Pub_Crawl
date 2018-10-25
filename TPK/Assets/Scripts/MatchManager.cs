using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MatchManager : MonoBehaviour {
    private int maxAttackers = 3;

    public int currentAttacker;
    public bool defenderExist;


	// Use this for initialization
	void Start () {
        currentAttacker = 0;
        defenderExist = false;

    }

    // Update is called once per frame
    void Update () {
		
	}

    // Return a boolean based on success of addition.
    public bool AddDefender() {
        if (defenderExist) {
            return false;
        }
        return defenderExist = true;
        
    }

    public bool AddAttacker() {
        if (currentAttacker < maxAttackers)
        {
            currentAttacker++;
            return true;
        }
        return false;
    }
}
