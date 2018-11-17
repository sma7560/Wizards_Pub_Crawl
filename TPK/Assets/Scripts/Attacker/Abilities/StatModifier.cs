using UnityEngine;

[System.Serializable]
public class StatModifier {
    public Stats statToModify;
    public int modifier;

    public int getModifier() {
        return modifier;
    }
}
