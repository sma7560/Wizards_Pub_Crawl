using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityManager : MonoBehaviour {
    public Skill[] equippedSkills;
    public Skill[] knownSkills; // This should probably be a list.
    private Transform origin;
    private Rigidbody playerRigid;
	// Use this for initialization
	void Start () {
        origin = gameObject.GetComponent<Transform>();
        playerRigid = gameObject.GetComponent<Rigidbody>();
        for (int i = 0; i<equippedSkills.Length; i++) {
            if (equippedSkills[i] != null)
                equippedSkills[i].Initialize(origin, playerRigid);
        }
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.Alpha1) && equippedSkills[0] != null) {
            CastSkill(0);
        }

        if (Input.GetKeyDown(KeyCode.Alpha2) && equippedSkills[1] != null)
        {
            CastSkill(1);
        }

        if (Input.GetKeyDown(KeyCode.Alpha3) && equippedSkills[2] != null)
        {
            CastSkill(2);
        }

        if (Input.GetKeyDown(KeyCode.Alpha4) && equippedSkills[3] != null)
        {
            CastSkill(3);
        }

    }
    // Removes the skill at the denoted equipIndex and replaces it with the skill denoted by the knownSkill index number
    public void SwapSkills(int equipIndex, int knownIndex) {
        equippedSkills[equipIndex] = knownSkills[knownIndex];
    }
    public void EquipSkill(int knownIndex) {
        for (int i = 0; i < equippedSkills.Length; i++) {
            if (equippedSkills[i] == null) {
                equippedSkills[i] = knownSkills[knownIndex];
                return;
            }
        }
        //Probably do things here to make it better for the players.
        Debug.Log("Can't Add More Skills");
    }
    private void RemoveSkill(int index) {
        equippedSkills[index] = null;
    }
    private void InstantiateSkill(int indexOfSkill) {
        equippedSkills[indexOfSkill].Initialize(origin, playerRigid);
    }
    public void CastSkill(int index)
    {
        equippedSkills[index].Cast();
    }
}
