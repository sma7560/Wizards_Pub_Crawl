using UnityEngine;

// This script describes the stats for a hero
[CreateAssetMenu(menuName = "Hero")]
public class Hero : ScriptableObject {
    public HeroType heroType; // This will determine the basic attack type. This is kept as a enum to make it extendable.
    public string heroName;

    public int childIndex; // This should be used to set which child is active.
    
    // This is all set by the players 
    //public int maxHealth;
    //public int moveSpeed;
    //public int atkSpeed;
    //public int defense;
    //public int attack;

}
