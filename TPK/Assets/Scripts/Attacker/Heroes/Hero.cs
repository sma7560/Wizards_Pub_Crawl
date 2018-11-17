using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Hero")]
public class Hero : ScriptableObject {
    public HeroType heroType;
    public string heroName;
    public GameObject model;
    public GameObject weapon;
    public GameObject projectile;
}
