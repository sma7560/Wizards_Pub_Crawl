using UnityEngine;

// This script describes the stats for a hero
[CreateAssetMenu(menuName = "Hero")]
public class Hero : ScriptableObject {
    public HeroType heroType; // This will determine the basic attack type. This is kept as a enum to make it extendable.
    public string heroName;
    public int childIndex; // This should be used to set which child is active.
}
