using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill : ScriptableObject {
    public string skillName;
    public string skillDescription;
    public Sprite skillIcon;
    public float skillCoolDown;
    public int skillCost;
    public CardTier cardtier;
    public Transform playerOrigin; //This is for getting the relative transform information
    public Rigidbody playerRigidBody;

    public void Initialize(Transform po, Rigidbody prb) {
        playerRigidBody = prb;
        playerOrigin = po;
    }
    public virtual void Cast() {
        Debug.Log("Casting: " + skillName);
    }
}