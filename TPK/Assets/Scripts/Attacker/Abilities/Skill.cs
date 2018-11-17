using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill : ScriptableObject {
    public string skillName;
    public string skillDescription;
    public Sprite skillIcon;
    public float skillCoolDown;
    public GameObject player; //This is for getting the relative transform information
}