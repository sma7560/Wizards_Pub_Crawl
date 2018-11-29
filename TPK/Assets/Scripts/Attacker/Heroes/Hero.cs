using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Hero")]
public class Hero : ScriptableObject {
    public HeroType heroType;
    public string heroName;
    public GameObject modelPrefab;
    
    public int maxHealth;
    public int currentHealth;
    public int moveSpeed;
    public int atkSpeed;
    public int defense;

    
}
